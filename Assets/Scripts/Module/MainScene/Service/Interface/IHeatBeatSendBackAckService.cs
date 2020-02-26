using System.Collections.Generic;
using Hank.Action;
using Hank.Api;

namespace Hank.MainScene
{
    public interface IHeatBeatSendBackAckService
    { 
        ServiceHeatBeatSendBackAckBackSignal serviceHeatBeatSendBackAckBackSignal { get; set; }

        ServiceHeatBeatSendBackAckErrBackSignal serviceHeatBeatSendBackAckErrBackSignal { get; set; }

        IHeatBeatSendBackAckModel modelHeatBeatSendBackAck { set; get; }

        void HeatBeatSendBackAck(ActionPostBody paras, string address);
    }
}
