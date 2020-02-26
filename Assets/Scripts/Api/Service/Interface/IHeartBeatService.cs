using System.Collections.Generic;
using Gululu;
using Gululu.net;

namespace Hank.Api
{
    public interface IHeartBeatService
    { 
        ServiceHeartBeatBackSignal serviceHeartBeatBackSignal { get; set; }

        ServiceHeartBeatErrBackSignal serviceHeartBeatErrBackSignal { get; set; }

        IHeartBeatModel modelHeartBeat { set; get; }

        void HeartBeat(HeartBeatBody paras);
        
    }
}
