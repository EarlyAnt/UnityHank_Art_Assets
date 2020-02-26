using System.Collections.Generic;
using Gululu;
using Gululu.net;

namespace Hank.Api
{
    public interface ISendAckService
    { 
        ServiceSendAckBackSignal serviceSendAckBackSignal { get; set; }

        ServiceSendAckErrBackSignal serviceSendAckErrBackSignal { get; set; }

        ISendAckModel mSendAckModel { set; get; }

        void sendAck(string url,ActionPostBody body);
    }
}
