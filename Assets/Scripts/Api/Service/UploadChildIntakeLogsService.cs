using System;
using System.Collections.Generic;
using BestHTTP;
using Cup.Utils.android;
using Gululu;
using Gululu.LocalData.RequstBody;
using Gululu.net;
using Hank.Api;

namespace Hank.Api
{
    public class UploadChildIntakeLogsService : IUploadChildIntakeLogsService
    {
        [Inject]
        public IJsonUtils mJsonUtils { get; set; }

        [Inject]
        public IGululuNetwork mGululuNetwork { get; set; }

        [Inject]
        public IUrlProvider mUrlProvider { get; set; }
        
        [Inject]
        public IUploadChildIntakeLogsModel mUploadChildIntakeLogsModel{ get; set; }

        [Inject]
        public INativeOkHttpMethodWrapper mNativeOkHttpMethodWrapper{ get; set; }

        public void uploadIntakeLogs(IntakeLogs mIntakeLogs, string current_child_sn, Action<UpLoadIntakeLogsResponseData> callBack, Action<ResponseErroInfo> errCallBack)
        {

            GuLog.Info("UploadChildIntakeLogsService uploadIntakeLogs");
            Dictionary<string, string> head = new Dictionary<string, string>();

            string body = mJsonUtils.Json2String(mIntakeLogs);

            // mGululuNetwork.sendRequest(mUrlProvider.getChildIntakeLogUrl(current_child_sn,CupBuild.getCupSn()), head, body, (result) =>
            // {
               
                // UpLoadIntakeLogsResponseData info = mJsonUtils.String2Json<UpLoadIntakeLogsResponseData>(result);
                //  mUploadChildIntakeLogsModel.mUpLoadIntakeLogsResponseData = info;
                // callBack(info);
                
            // }, (ecb) =>
            // {
            //     errCallBack(ecb);
            // }, HTTPMethods.Post);

            mNativeOkHttpMethodWrapper.post(mUrlProvider.getChildIntakeLogUrl(current_child_sn,CupBuild.getCupSn()),"",body,(result)=>{
                GuLog.Info("UploadChildIntakeLogsService onresponse");
                UpLoadIntakeLogsResponseData info = mJsonUtils.String2Json<UpLoadIntakeLogsResponseData>(result);
                 mUploadChildIntakeLogsModel.mUpLoadIntakeLogsResponseData = info;
                callBack(info);
            },(errorResult)=>{
                GuLog.Info("UploadChildIntakeLogsService faliure");
                errCallBack(errorResult);
            });
        }
    }
}