using LitJson;
using System.Collections.Generic;
using Gululu;
using Hank.Api;
using Gululu.LocalData.Agents;
using Hank.Action;

namespace Hank.MainScene
{
    public class HeatBeatSendBackAckService : BaseService, IHeatBeatSendBackAckService
    {
        [Inject]
        public ServiceHeatBeatSendBackAckBackSignal serviceHeatBeatSendBackAckBackSignal { get; set; }

        [Inject]
        public ServiceHeatBeatSendBackAckErrBackSignal serviceHeatBeatSendBackAckErrBackSignal { get; set; }

        [Inject]
        public IHeatBeatSendBackAckModel modelHeatBeatSendBackAck { set; get; }


        public void HeatBeatSendBackAck(ActionPostBody paras,string address)
        {
            string strbody = mJsonUtils.Json2String(paras);

            mNativeOkHttpMethodWrapper.post(address,"" , strbody, (r) => {
                HeatBeatSendBackAckResponse info = mJsonUtils.String2Json<HeatBeatSendBackAckResponse>(r);

                modelHeatBeatSendBackAck.responseHeatBeatSendBackAck = info;

                serviceHeatBeatSendBackAckBackSignal.Dispatch(modelHeatBeatSendBackAck.responseHeatBeatSendBackAck);

            }, (e) => {
                serviceHeatBeatSendBackAckErrBackSignal.Dispatch(e);
            });

        }
    }
}