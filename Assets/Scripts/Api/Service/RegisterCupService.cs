using BestHTTP;
using Cup.Utils.android;
using Gululu;
using Gululu.LocalData.Agents;
using Gululu.LocalData.DataManager;
using Gululu.net;
using LitJson;
using System.Collections.Generic;
using UnityEngine;

namespace Hank.Api
{
    public class RegisterCupService : BaseService, IRegisterCupService
    {
        [Inject]
        public ServiceRegisterCupBackSignal serviceRegisterCupBackSignal { get; set; }
        [Inject]
        public ServiceRegisterCupErrBackSignal serviceRegisterCupErrBackSignal { get; set; }
        [Inject]
        public IRegisterCupModel modelRegisterCup { set; get; }
        [Inject]
        public ILocalChildInfoAgent mLocalChildInfoAgent { set; get; }
        [Inject]
        public ILocalCupAgent mLocalCupAgent { set; get; }
        [Inject]
        public ILocalPetInfoAgent LocalPetInfoAgent { get; set; }
        [Inject]
        public ILocalUnlockedPetsAgent LocalUnlockedPetsAgent { get; set; }
        [Inject]
        public ITimeManager mTimeManager { get; set; }
        [Inject]
        public INativeDataManager mNativeDataManager { get; set; }
        [Inject]
        public IPlayerDataManager PlayerDataManager { get; set; }

        public void RegisterCup(RegisterCupBody body)
        {
            Dictionary<string, string> head = new Dictionary<string, string>();

            string strbody = mJsonUtils.Json2String(body);

            sendRegisterCupRequest(head, strbody);
        }

        private void saveData(RegisterResponseData info)
        {
            if (!string.IsNullOrEmpty(info.x_child_sn))
            {
                //Debug.LogFormat("--------SaveSN--------RegisterCupService.saveData: {0}", info.x_child_sn);
                mLocalChildInfoAgent.saveChildSN(info.x_child_sn);
                FlurryUtil.LogEvent("Pair_Get_Childsn_Sunccess_Event");
            }
            else {
                FlurryUtil.LogEvent("Pair_Get_Childsn_Fail_Event");
                GuLog.Warning("pair data childsn is null");
            }

            //删除本地数据
            this.LocalPetInfoAgent.Clear();
            GuLog.Debug("<><RegisterCupService>Clear currentPet");
            this.LocalUnlockedPetsAgent.Clear();
            GuLog.Debug("<><RegisterCupService>Clear unlockedPets");

            //保存网络数据
            //mLocalCupAgent.saveCupToken(CupBuild.getCupSn(), info.token);
            mNativeDataManager.saveToken(info.token);
            mLocalCupAgent.saveCupID(CupBuild.getCupSn(), info.x_cup_sn);
            LocalPetInfoAgent.saveCurrentPet(info.pet_model);
            //重新保存语言数据
            this.PlayerDataManager.SaveLanguage(this.PlayerDataManager.Language);

            if (info.timestamp > 0)
            {
                mTimeManager.setServerTime(info.timestamp * (1000L));
            }

            if (!string.IsNullOrEmpty(info.timezone))
            {
                mTimeManager.setTimeZone(info.timezone);
            }
        }

        public void sendRegisterCupRequest(Dictionary<string, string> head, string strbody)
        {
            Header headerentity = new Header();

            headerentity.headers = new List<HeaderData>();

            if (head != null)
            {
                foreach (KeyValuePair<string, string> headr in head)
                {
                    HeaderData data = new HeaderData();
                    data.key = headr.Key;
                    data.value = headr.Value;
                    headerentity.headers.Add(data);
                }
            }

            string strHeader = mJsonUtils.Json2String(headerentity);


            mNativeOkHttpMethodWrapper.post(mUrlProvider.getRegisterUrl(AppData.GetAppName(), CupBuild.getCupSn()), strHeader, strbody, (result) =>
            {
                try
                {
                    GuLog.Info("Register response data :" + result);
                    RegisterResponseData info = mJsonUtils.String2Json<RegisterResponseData>(result);
                    modelRegisterCup.responseRegisterCup = info;

                    Loom.QueueOnMainThread(() =>
                    {
                        if (mValidateResponseData.isValidRegisterResponseData(info))
                        {
                            //Debug.LogFormat("--------SaveSN--------RegisterCupService.sendRegisterCupRequest: {0}", info.x_child_sn);
                            saveData(info);
                            serviceRegisterCupBackSignal.Dispatch(info);
                        }
                        else
                        {
                            Debug.LogError("response data is invalid:" + result);
                            serviceRegisterCupErrBackSignal.Dispatch(ResponseErroInfo.GetErrorInfo(GululuErrorCode.RESPONSE_DATA_ERROR, "response data is invalid"));

                        }
                    });
                }
                catch
                {
                    Loom.QueueOnMainThread(() =>
                    {
                        Debug.LogError("response data is error:" + result);
                        serviceRegisterCupErrBackSignal.Dispatch(ResponseErroInfo.GetErrorInfo(GululuErrorCode.RESPONSE_DATA_ERROR, "response data is error"));
                    });
                }
            }, (errorResult) =>
            {
                Loom.QueueOnMainThread(() =>
                {
                    serviceRegisterCupErrBackSignal.Dispatch(errorResult);
                });
            });
        }

    }
}