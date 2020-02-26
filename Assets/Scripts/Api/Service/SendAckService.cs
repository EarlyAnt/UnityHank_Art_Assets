using System.Collections.Generic;
using BestHTTP;
using Gululu;
using Gululu.net;

namespace Hank.Api
{
    public class SendAckService : BaseService,ISendAckService
    {
        [Inject]
        public ServiceSendAckBackSignal serviceSendAckBackSignal { get; set; }

        [Inject]
        public ServiceSendAckErrBackSignal serviceSendAckErrBackSignal  { get; set; }

        [Inject]
        public ISendAckModel mSendAckModel  { get; set; }
        

        public void sendAck(string url, ActionPostBody body)
        {
            Dictionary<string, string> head = new Dictionary<string, string>();

            string strbody = mJsonUtils.Json2String(body);

            // mGululuNetwork.sendRequest(url, head, strbody, (r) =>
            // {

                // GuLog.Debug("ackRet:" + r);
                // serviceSendAckBackSignal.Dispatch(r);
                // mSendAckModel.result = r;

            // }, (e) =>
            // {
            //     serviceSendAckErrBackSignal.Dispatch(e);
            // }, HTTPMethods.Post);


            mNativeOkHttpMethodWrapper.post(url,"",strbody,(result)=>{
                GuLog.Debug("ackRet:" + result);
                serviceSendAckBackSignal.Dispatch(result);
                mSendAckModel.result = result;
            },(errorResult)=>{
                serviceSendAckErrBackSignal.Dispatch(errorResult);
            });
        }
    }
}