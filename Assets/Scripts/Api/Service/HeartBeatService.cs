using BestHTTP;
using Cup.Utils.android;
using Gululu;
using Gululu.LocalData.Agents;
using Gululu.net;
using LitJson;
using System.Collections.Generic;

namespace Hank.Api
{
    public class HeartBeatService : BaseService,IHeartBeatService
    {
        [Inject]
        public ServiceHeartBeatBackSignal serviceHeartBeatBackSignal { get; set; }

        [Inject]
        public ServiceHeartBeatErrBackSignal serviceHeartBeatErrBackSignal { get; set; }

        [Inject]
        public IHeartBeatModel modelHeartBeat { set; get; }

        public void HeartBeat(HeartBeatBody body)
        {
            // Dictionary<string, string> head = new Dictionary<string, string>();
            
            string prod_name = AppData.GetAppName();

            string x_cup_sn = CupBuild.getCupSn() ;

            string strbody = mJsonUtils.Json2String(body);

            GuLog.Info("<><HeartBeatService> x_cup_sn:" + x_cup_sn + "  strbody:" + strbody);


            // mGululuNetwork.sendRequest(mUrlProvider.HeartBeatUrl(prod_name, x_cup_sn), head, strbody, (r) => {

            //     GuLog.Debug("<><HeartBeatService> Succeed:" + r);
            //     HeartBeatResponse info = mJsonUtils.String2Json<HeartBeatResponse>(r);

            //     modelHeartBeat.responseHeartBeat = info;

            //     serviceHeartBeatBackSignal.Dispatch(modelHeartBeat.responseHeartBeat);

            // }, (e) => {
            //     GuLog.Debug("<><HeartBeatService> Error:" + e.ErrorCode + e.ErrorInfo);
            //     serviceHeartBeatErrBackSignal.Dispatch(e);
            // }, HTTPMethods.Post);


            mNativeOkHttpMethodWrapper.post(mUrlProvider.HeartBeatUrl(prod_name, x_cup_sn), "", strbody, (r) => {

                GuLog.Info("<><HeartBeatService> Succeed:" + r);
                HeartBeatResponse info = mJsonUtils.String2Json<HeartBeatResponse>(r);

                modelHeartBeat.responseHeartBeat = info;

                serviceHeartBeatBackSignal.Dispatch(modelHeartBeat.responseHeartBeat);

            }, (e) => {
                GuLog.Info("<><HeartBeatService> Error:" + e.ErrorCode + e.ErrorInfo);
                serviceHeartBeatErrBackSignal.Dispatch(e);
            });


        }
    }
}