using Gululu;
using Hank.Action;
using Hank.Api;
using strange.extensions.command.impl;
using System.Collections.Generic;

namespace Hank.MainScene
{
    public class HeatBeatSendBackAckCommand : Command
    {
        [Inject]
        public IHeatBeatSendBackAckService getHeatBeatSendBackAckService { get; set; }

        [Inject]
        public MediatorHeatBeatSendBackAckErrSignal mediatorHeatBeatSendBackAckErrSignal{ get; set; }

        [Inject]
        public MediatorHeatBeatSendBackAckSignal mediatorHeatBeatSendBackAckSignal { get; set; }

        [Inject]
        public ActionPostBody parms { set; get; }

        [Inject]
        public string address { set; get; }

        public override void Execute()
        {
            getHeatBeatSendBackAckService.serviceHeatBeatSendBackAckBackSignal.AddListener(HeatBeatSendBackAckResult);
            getHeatBeatSendBackAckService.serviceHeatBeatSendBackAckErrBackSignal.AddListener(HeatBeatSendBackAckResultErr);

            getHeatBeatSendBackAckService.HeatBeatSendBackAck(parms, address);            

            Retain();
        }

        public void HeatBeatSendBackAckResult(HeatBeatSendBackAckResponse result){
            getHeatBeatSendBackAckService.serviceHeatBeatSendBackAckBackSignal.RemoveListener(HeatBeatSendBackAckResult);
            getHeatBeatSendBackAckService.serviceHeatBeatSendBackAckErrBackSignal.RemoveListener(HeatBeatSendBackAckResultErr);

            mediatorHeatBeatSendBackAckSignal.Dispatch(result);
            Release();
        }

        public void HeatBeatSendBackAckResultErr(ResponseErroInfo result)
        {
            getHeatBeatSendBackAckService.serviceHeatBeatSendBackAckBackSignal.RemoveListener(HeatBeatSendBackAckResult);
            getHeatBeatSendBackAckService.serviceHeatBeatSendBackAckErrBackSignal.RemoveListener(HeatBeatSendBackAckResultErr);
            mediatorHeatBeatSendBackAckErrSignal.Dispatch(result);
            Release();
        }
    }
}
