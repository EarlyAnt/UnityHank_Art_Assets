using System;
using Gululu.LocalData.Agents;
using Hank.Api;

namespace Gululu.LocalData.DataManager
{
    public interface IIntakeLogDataManager
    {

        ILocalChildInfoAgent mLocalChildInfoAgent { get; set; }

        ILocalIntakeLogsData mLocalIntakeLogsData { get; set; }

        IUploadChildIntakeLogsService mUploadChildIntakeLogsService{ get; set; }
        void notifyNetworkStatus();

        void addIntakeLog(string type, int vl, long time_stamp);


        void uploadBackupData();

        void upload(Action<UpLoadIntakeLogsResponseData> callBack, Action<ResponseErroInfo> errCallBack);

        bool isNetworkEnable();
    }
}