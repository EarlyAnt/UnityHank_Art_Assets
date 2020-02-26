using System;
using System.Collections.Generic;
using System.Text;
using BestHTTP;
using Cup.Utils.android;
using Gululu.LocalData.Agents;
using Hank.Api;
using UnityEngine;

namespace Gululu.net
{
    //�ѽ�����������ͬ������
    public class UnlockedPetsUtils : IUnlockedPetsUtils
    {
        /// <summary>
        /// ͬ��������
        /// </summary>
        class ActionData
        {
            public DateTime RegisterTime { get; set; }//����ע��ʱ��
            public string Header { get; set; }
            public string Body { get; set; }
            public int SendTimes { get; set; }//���ʹ���(Ĭ��3�Σ�3��ʧ�ܺ󣬲����ط�)
            public Action<Result> OnSuccess { get; set; }//http����ɹ�ʱ�ص�
            public Action<Result> OnFailed { get; set; }//http����ʧ��ʱ�ص�
        }

        [Inject]
        public IUrlProvider UrlProvider { get; set; }
        [Inject]
        public IJsonUtils JsonUtils { get; set; }
        [Inject]
        public ILocalCupAgent LocalCupAgent { get; set; }
        [Inject]
        public ILocalPetInfoAgent LocalPetInfoAgent { get; set; }
        [Inject]
        public ILocalChildInfoAgent LocalChildInfoAgent { get; set; }
        [Inject]
        public ILocalUnlockedPetsAgent LocalUnlockedPetsAgent { get; set; }
        [Inject]
        public INativeOkHttpMethodWrapper mNativeOkHttpMethodWrapper { get; set; }
        [Inject]
        public GululuNetworkHelper GululuNetworkHelper { get; set; }
        private List<ActionData> actionDatas = new List<ActionData>();
        private ActionData lastActionData = null;

        //����ͬ��
        public void SyncUnlockedPets(Action<Result> callBack = null, Action<Result> errCallBack = null)
        {
            Header header = new Header();
            header.headers = new List<HeaderData>();
            header.headers.Add(new HeaderData() { key = "Gululu-Agent", value = GululuNetworkHelper.GetAgent() });
            header.headers.Add(new HeaderData() { key = "udid", value = GululuNetworkHelper.GetUdid() });
            header.headers.Add(new HeaderData() { key = "Accept-Language", value = GululuNetworkHelper.GetAcceptLang() });
            header.headers.Add(new HeaderData() { key = "Content-Type", value = "application/json" });
            string strHeader = this.JsonUtils.Json2String(header);

            List<string> unlockedPets = this.LocalUnlockedPetsAgent.GetUnlockedPets();
            if (unlockedPets == null || unlockedPets.Count == 0)
            {//��������Ҳ����Ѿ������ĳ����Ĭ�ϳ������Ϊ��һ���ѽ�������
                unlockedPets = new List<string>();
                unlockedPets.Add(this.LocalPetInfoAgent.getCurrentPet());
            }
            LocalPetsInfo body = new LocalPetsInfo()
            {
                current_pet = this.LocalPetInfoAgent.getCurrentPet(),
                unlocked_pets = unlockedPets
            };
            string strBody = this.JsonUtils.Json2String(body);
            Debug.Log("--------UnlockedPetsUtils.SyncUnlockedPets.Prepare data--------");

            ActionData newActionData = new ActionData()
            {
                Header = strHeader,
                Body = strBody,
                SendTimes = 3,
                OnSuccess = callBack,
                OnFailed = errCallBack
            };

            //���������Ƿ��Ѿ�������
            if (this.lastActionData != null && this.lastActionData.Body == strBody)
            {//�����������ݣ�ֻ��Ҫ���������Ƿ�ѹջ
                Debug.LogFormat("--------UnlockedPetsUtils.SyncUnlockedPets.Ignore same data--------Header: {0}, Body: {1}", strHeader, strBody);
                return;//��������������һ�����ݵ�Body��ͬ��ֱ�Ӻ���
            }
            else
            {//������û�����ݣ�����Ҫ����ѹջ����������������ͬ������
                Debug.LogFormat("--------UnlockedPetsUtils.SyncUnlockedPets.Append new data and execute--------Header: {0}, Body: {1}", strHeader, strBody);
                this.lastActionData = newActionData;
                this.actionDatas.Add(newActionData);
                this.SyncUnlockedPets();
            }
        }
        //����ͬ��
        private void SyncUnlockedPets()
        {
            if (this.actionDatas.Count > 0)
            {
                ActionData actionData = this.actionDatas[0];
                Debug.LogFormat("--------UnlockedPetsUtils.SyncUnlockedPets--------Header: {0}, Body: {1}, SendTimes: {2}", actionData.Header, actionData.Body, actionData.SendTimes);
                mNativeOkHttpMethodWrapper.post(this.UrlProvider.GetUnlockedPets(this.LocalChildInfoAgent.getChildSN(), CupBuild.getCupSn()), actionData.Header, actionData.Body, (result) =>
                {
                    Debug.LogFormat("--------UnlockedPetsUtils.SyncUnlockedPets.GotResponse--------{0}", result);
                    RemotePetsInfo remotePetsInfo = this.JsonUtils.String2Json<RemotePetsInfo>(result);
                    this.LocalUnlockedPetsAgent.SaveUnlockedPets(remotePetsInfo.pets);
                    if (actionData.OnSuccess != null) Loom.QueueOnMainThread(() => actionData.OnSuccess(Result.Success(result)));
                    Debug.LogFormat("--------UnlockedPetsUtils.SyncUnlockedPets.Success--------\n{0}", result);
                    //�������
                    if (this.actionDatas.Count > 0)
                    {
                        this.actionDatas.RemoveAt(0);//�Ƴ��Ѿ�ִ�гɹ�������
                        if (this.actionDatas.Count > 0)//ִ����һ������
                            this.SyncUnlockedPets();
                    }

                }, (errorResult) =>
                {
                    Debug.LogFormat("--------UnlockedPetsUtils.SyncUnlockedPets.Error--------{0}", Result.Error(errorResult.ErrorInfo));
                    if (actionData.OnFailed != null) Loom.QueueOnMainThread(() => actionData.OnFailed(Result.Error(errorResult.ErrorInfo)));
                    //�������
                    if (this.actionDatas.Count > 0)
                    {
                        if (this.actionDatas[0].SendTimes > 0)
                        {//�ظ��ϴ�(���3��)
                            this.actionDatas[0].SendTimes -= 1;
                            Debug.LogFormat("--------UnlockedPetsUtils.SyncUnlockedPets.Repeat--------SendTimes: {0}, Body: {1}", actionData.SendTimes, actionData.Body);
                            this.SyncUnlockedPets();
                        }
                        else
                        {//3���ش�ʧ�ܷ���
                            this.actionDatas.RemoveAt(0);
                            Debug.LogFormat("--------UnlockedPetsUtils.SyncUnlockedPets.Abandon--------SendTimes: {0}, Body: {1}", actionData.SendTimes, actionData.Body);
                        }
                    }
                });
            }
        }
    }
}