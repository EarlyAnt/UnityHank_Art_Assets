using System.Collections.Generic;
using Gululu;
using strange.extensions.signal.impl;

namespace Hank.MainScene
{
    public class StartPostGameDataCommandSignal : Signal<string>
    {
    }
	public class ServicePostGameDataBackSignal : Signal<PostGameDataResponse>
    {
    }

    public class ServicePostGameDataErrBackSignal : Signal<ResponseErroInfo>
    {
    }

    public class MediatorPostGameDataSignal : Signal<PostGameDataResponse>
    {
    }
	
    public class MediatorPostGameDataErrSignal : Signal<ResponseErroInfo>
    {
    }
}