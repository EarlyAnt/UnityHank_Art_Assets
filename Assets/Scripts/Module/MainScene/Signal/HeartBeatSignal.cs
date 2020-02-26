using System.Collections.Generic;
using Gululu;
using Hank.Action;
using Hank.Api;
using strange.extensions.signal.impl;

namespace Hank.MainScene
{
    public class StartHeartBeatCommandSignal : Signal
    {
    }

    public class MediatorHeartBeatSignal : Signal<HeartBeatResponse>
    {
    }
	
    public class MediatorHeartBeatErrSignal : Signal<ResponseErroInfo>
    {
    }
}