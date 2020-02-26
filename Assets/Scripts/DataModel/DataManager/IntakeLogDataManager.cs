using System;
using System.Collections.Generic;
using CupSdk.BaseSdk.Water;
using Gululu.LocalData.Agents;
using Gululu.LocalData.RequstBody;
using Gululu.net;
using Hank.Api;
using UnityEngine;

namespace Gululu.LocalData.DataManager
{
    public class IntakeLogDataManager : IIntakeLogDataManager
    {
        [Inject]
        public ILocalChildInfoAgent mLocalChildInfoAgent { get; set; }

        [Inject]
        public ILocalIntakeLogsData mLocalIntakeLogsData { get; set; }


        [Inject]
        public IUploadChildIntakeLogsService mUploadChildIntakeLogsService { get; set; }

        public void notifyNetworkStatus()
        {
            Debug.Log("<><>IntakeLogDataManager<>Netnotify<>");
            uploadBackupData();
        }

        public void addIntakeLog(string type, int vl, long time_stamp)
        {
            Debug.Log("addIntakeLog type:" + type + " vl:" + vl + " time_stamp:" + time_stamp);

            string currentChildSN = mLocalChildInfoAgent.getChildSN();
            if (currentChildSN != string.Empty)
                mLocalIntakeLogsData.addIntakeLog(currentChildSN, type, vl, time_stamp);
            uploadBackupData();
        }


        public void uploadBackupData()
        {
            if (isNetworkEnable())
            {
                upload((cb) => { }, (ecb) => { });
            }
        }

        public void upload(Action<UpLoadIntakeLogsResponseData> callBack, Action<ResponseErroInfo> errCallBack)
        {
            GuLog.Info("IntakeLogDataManager upload");
            string currentChildSN = mLocalChildInfoAgent.getChildSN();
            if (currentChildSN == string.Empty)
            {
                errCallBack(ResponseErroInfo.GetErrorInfo(0,"upload resource is empty"));
                return;
            }
            List<LogItem> info = mLocalIntakeLogsData.getAllIntakeLogsInfo(currentChildSN);

            if (info != null && info.Count > 0)
            {
                GuLog.Info("IntakeLogDataManager havedata");
                IntakeLogs mIntakeLogs = new IntakeLogs();
                mIntakeLogs.logs = info;


                mUploadChildIntakeLogsService.uploadIntakeLogs(mIntakeLogs, currentChildSN, (cb) =>
                {
                    mLocalIntakeLogsData.deleteIntakeLogsInfo(currentChildSN);
                    callBack(cb);
                }, (ecb) =>
                {
                    errCallBack(ecb);
                });
            }else{
                 errCallBack(ResponseErroInfo.GetErrorInfo(0,"upload resource is empty"));
            }


        }

        public bool isNetworkEnable()
        {
            return Application.internetReachability != NetworkReachability.NotReachable;
        }
    }
}