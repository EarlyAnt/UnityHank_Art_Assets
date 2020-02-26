using System;
using Gululu;
using Gululu.LocalData.RequstBody;
using Gululu.net;
using Hank.Api;

namespace Hank.Api
{
    public interface IUploadChildIntakeLogsService
    {
        IJsonUtils mJsonUtils { get; set; }

        IGululuNetwork mGululuNetwork { get; set; }

        IUrlProvider mUrlProvider { get; set; }

        IUploadChildIntakeLogsModel mUploadChildIntakeLogsModel { get; set; }

        void uploadIntakeLogs(IntakeLogs mIntakeLogs, string current_child_sn, Action<UpLoadIntakeLogsResponseData> callBack, Action<ResponseErroInfo> errCallBack);
    }
}