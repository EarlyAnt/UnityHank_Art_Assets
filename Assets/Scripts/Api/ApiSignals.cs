using Gululu;
using strange.extensions.signal.impl;

namespace Hank.Api
{
    public class ServicePingServiceBackSignal : Signal<DataBase>
    {
    }

    public class ServicePingServiceErrBackSignal : Signal<ResponseErroInfo>
    {
    }


    public class ServiceRegisterCupBackSignal : Signal<RegisterResponseData>
    {
    }

    public class ServiceRegisterCupErrBackSignal : Signal<ResponseErroInfo>
    {
    }

    public class ServiceHeartBeatBackSignal : Signal<HeartBeatResponse>
    {
    }

    public class ServiceHeartBeatErrBackSignal : Signal<ResponseErroInfo>
    {
    }

    public class ServiceGetValueLongBackSignal : Signal<string>
    {
    }

    public class ServiceGetValueLongErrBackSignal : Signal<ResponseErroInfo>
    {
    }

    public class ServiceSendAckBackSignal : Signal<string>
    {
    }

    public class ServiceSendAckErrBackSignal : Signal<ResponseErroInfo>
    {
    }

    public class ServiceTmallAuthBackSignal : Signal<TmallAuthResponse>
    {
    }

    public class ServiceTmallAuthErrBackSignal : Signal<ResponseErroInfo>
    {
    }

    public class OrderReceiptSignal : Signal<OrderReceipt>
    {
    }

    public class OrderPaymentSignal : Signal<bool>
    {
    }
}