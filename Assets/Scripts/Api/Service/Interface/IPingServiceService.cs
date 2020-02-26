using System.Collections.Generic;
using Gululu;
using Gululu.net;

namespace Hank.Api
{
    public interface IPingServiceService 
    { 
        ServicePingServiceBackSignal servicePingServiceBackSignal { get; set; }

        ServicePingServiceErrBackSignal servicePingServiceErrBackSignal { get; set; }

        IPingServiceModel modelPingService { set; get; }

        void PingService();
    }
}
