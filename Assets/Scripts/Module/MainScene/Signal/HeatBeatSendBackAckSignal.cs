using System.Collections.Generic;
using Gululu;
using Hank.Api;
using strange.extensions.signal.impl;

namespace Hank.MainScene
{
    public class StartHeatBeatSendBackAckCommandSignal : Signal<ActionPostBody,string>
    {
    }
	public class ServiceHeatBeatSendBackAckBackSignal : Signal<HeatBeatSendBackAckResponse>
    {
    }

    public class ServiceHeatBeatSendBackAckErrBackSignal : Signal<ResponseErroInfo>
    {
    }

    public class MediatorHeatBeatSendBackAckSignal : Signal<HeatBeatSendBackAckResponse>
    {
    }
	
    public class MediatorHeatBeatSendBackAckErrSignal : Signal<ResponseErroInfo>
    {
    }
}