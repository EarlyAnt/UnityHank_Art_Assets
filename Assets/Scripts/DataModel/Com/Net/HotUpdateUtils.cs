using Assets.Script.Global;
using Cup.Utils.android;
using Gululu.LocalData.Agents;
using Hank.Api;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Gululu.net
{
    //�ȸ����ݻ�ȡ����
    public class HotUpdateUtils : IHotUpdateUtils
    {
        [Inject]
        public IUrlProvider UrlProvider { get; set; }
        [Inject]
        public IJsonUtils JsonUtils { get; set; }
        [Inject]
        public ILocalCupAgent LocalCupAgent { get; set; }
        [Inject]
        public INativeOkHttpMethodWrapper mNativeOkHttpMethodWrapper { get; set; }
        [Inject]
        public GululuNetworkHelper GululuNetworkHelper { get; set; }
        private UpdateInfo newestVersion = null;
        private string fileName = string.Format("{0}/UpdateRecord.json", Application.persistentDataPath);

        //��ȡ��������Դ��������
        public void GetUpdateInfo(Action<UpdateInfos> callBack = null, Action<string> errCallBack = null)
        {
            Header header = new Header();
            header.headers = new List<HeaderData>();
            header.headers.Add(new HeaderData() { key = "Gululu-Agent", value = GululuNetworkHelper.GetAgent() });
            header.headers.Add(new HeaderData() { key = "udid", value = GululuNetworkHelper.GetUdid() });
            header.headers.Add(new HeaderData() { key = "Accept-Language", value = GululuNetworkHelper.GetAcceptLang() });
            header.headers.Add(new HeaderData() { key = "Content-Type", value = "application/json" });
            string strHeader = this.JsonUtils.Json2String(header);

            UpdateRecord updateRecord = this.ReadUpdateRecord();//��ȡ���ظ��¼�¼
            UpdateRequest body = new UpdateRequest()
            {
                partner = AppInfo.Instance.Channel,
                apk_ver_code = AppInfo.Instance.VersionCode,
                res_ver_code = updateRecord != null ? updateRecord.ResVersionCode : 0//������ظ��¼�¼Ϊ�գ���Դ�汾��Ĭ��Ϊ0
            };
            string strBody = this.JsonUtils.Json2String(body);

            string url = this.UrlProvider.GetUpdateInfo(CupBuild.getCupSn());
            Debug.LogFormat("<><HotUpdateUtils.GetUpdateInfo>Header: {0}, Body: {1}, Url: {2}", strHeader, strBody, url);
            mNativeOkHttpMethodWrapper.post(url, strHeader, strBody, (result) =>
            {
                Debug.LogFormat("<><HotUpdateUtils.GetUpdateInfo>GotResponse: {0}", result);
                UpdateInfos updateInfos = this.JsonUtils.String2Json<UpdateInfos>(result);
                if (updateInfos != null && callBack != null)
                {
                    this.SetNewsestVersion(updateInfos);
                    callBack(updateInfos);
                }
                else if (errCallBack != null)
                {
                    string errorText = "Response data 'updateInfo' is null";
                    Debug.LogErrorFormat("<><HotUpdateUtils.GetUpdateInfo>Invalid data error: {0}", errorText);
                    errCallBack(errorText);
                }
            }, (errorResult) =>
            {
                if (errorResult != null && errCallBack != null)
                {
                    Debug.LogErrorFormat("<><HotUpdateUtils.GetUpdateInfo>Status error: {0}", errorResult.ErrorInfo);
                    errCallBack(errorResult.ErrorInfo);
                }
            });
        }
        //��ȡ������Դ���¼�¼
        public UpdateRecord ReadUpdateRecord()
        {
            UpdateRecord updateRecord = null;
            if (!File.Exists(this.fileName))
            {
                updateRecord = new UpdateRecord()
                {
                    CupSN = CupBuild.getCupSn(),
                    CupType = "Go2",
                    TimeStamp = "",
                    ApkVersion = AppInfo.Instance.Version,
                    ApkVersionCode = AppInfo.Instance.VersionCode,
                    ResVersionCode = 0,
                    FileList = new List<LocalFileInfo>()
                };
            }
            else
            {
                try
                {
                    string content = "";
                    using (FileStream fs = new FileStream(this.fileName, FileMode.Open, FileAccess.Read))
                    {
                        using (StreamReader sr = new StreamReader(fs))
                        {
                            content = sr.ReadToEnd();
                            sr.Close();
                        }
                        fs.Close();
                    }
                    updateRecord = this.JsonUtils.String2Json<UpdateRecord>(content);
                }
                catch (Exception ex)
                {
                    Debug.LogErrorFormat("<><HotUpdateUtils.ReadUpdateRecord>Error: {0}", ex.Message);
                }
            }
            return updateRecord;
        }
        //���汾����Դ���¼�¼
        public void SaveUpdateRecord(UpdateRecord updateRecord)
        {
            if (updateRecord == null)
            {
                Debug.LogErrorFormat("<><HotUpdateUtils.SaveUpdateRecord>Paramter 'updateRecord' is null");
                return;
            }
            else if (this.newestVersion == null)
            {
                Debug.LogErrorFormat("<><HotUpdateUtils.SaveUpdateRecord>server version info is null");
                return;
            }

            try
            {
                //��¼�ӷ������ϻ�ȡ�������µģ����ɹ�������ϵİ汾�İ汾�ź�ʱ���
                updateRecord.ResVersionCode = this.newestVersion.res_ver_code;
                updateRecord.TimeStamp = this.newestVersion.create_time;
                //������¼�¼
                string content = this.JsonUtils.Json2String(updateRecord);
                using (FileStream fs = new FileStream(this.fileName, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        sw.Write(content);
                        sw.Close();
                    }
                    fs.Close();
                }
                Debug.Log("<><HotUpdateUtils.SaveUpdateRecord>OK + OK");
            }
            catch (Exception ex)
            {
                Debug.LogErrorFormat("<><HotUpdateUtils.SaveUpdateRecord>Error: {0}", ex.Message);
            }
        }
        //��¼���°汾
        private void SetNewsestVersion(UpdateInfos updateInfos)
        {
            if (updateInfos != null && updateInfos.res_list != null)
            {
                List<UpdateInfo> updateInfoList = updateInfos.res_list.OrderByDescending(t => t.res_ver_code).ToList();//������Դ�汾�ŵ����������θ�������
                foreach (var updateInfo in updateInfoList)
                {
                    if (updateInfo.apk_ver_code <= AppInfo.Instance.VersionCode)
                    {
                        Debug.LogFormat("<><HotUpdateUtils.SetNewsestVersion>apk_ver: {0}, apk_ver_code: {1}, res_ver_code: {2}", updateInfo.apk_ver, updateInfo.apk_ver_code, updateInfo.res_ver_code);
                        this.newestVersion = updateInfo;
                        break;
                    }
                }
            }
        }
    }
}