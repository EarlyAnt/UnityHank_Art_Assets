using Gululu;
using Hank.Api;
using strange.extensions.signal.impl;

namespace Hank.MainScene
{
    public class MediatorTmallAuthSignal : Signal<TmallAuthResponse>{}

    public class MediatorTmallAuthErrSignal : Signal<ResponseErroInfo>{}

    public class StartTmallAuthCommandSignal : Signal{}
}