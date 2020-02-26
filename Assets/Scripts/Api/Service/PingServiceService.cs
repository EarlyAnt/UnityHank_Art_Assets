using BestHTTP;
using Cup.Utils.android;
using Gululu;
using Gululu.net;
using LitJson;
using System.Collections.Generic;

namespace Hank.Api
{
    public class PingServiceService : BaseService,IPingServiceService
    {
        [Inject]
        public ServicePingServiceBackSignal servicePingServiceBackSignal { get; set; }

        [Inject]
        public ServicePingServiceErrBackSignal servicePingServiceErrBackSignal { get; set; }

        [Inject]
        public IPingServiceModel modelPingService { set; get; }

        public void PingService()
        {
            // Dictionary<string, string> head = new Dictionary<string, string>();

            string url = mUrlProvider.getHealthUrl(CupBuild.getCupSn());

            mNativeOkHttpMethodWrapper.get(url,"",successCallBack,errorCallBack);

            // mGululuNetwork.sendRequest(url, head, successCallBack, errorCallBack, HTTPMethods.Get);
        }

        public void successCallBack(string result)
        {
            DataBase info = mJsonUtils.String2Json<DataBase>(result);
            modelPingService.responsePingService = info;
            Loom.QueueOnMainThread(()=>{
               
                servicePingServiceBackSignal.Dispatch(info);
                
            });
        }

        public void errorCallBack(ResponseErroInfo errorInfo)
        {
            Loom.QueueOnMainThread(()=>{
                servicePingServiceErrBackSignal.Dispatch(errorInfo);
            });
        }
    }
}