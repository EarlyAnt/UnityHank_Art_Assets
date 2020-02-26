using System.Collections.Generic;
using Gululu;
using Gululu.net;

namespace Hank.Api
{
    public interface IGetValueLongService
    { 
        ServiceGetValueLongBackSignal serviceGetValueLongBackSignal { get; set; }

        ServiceGetValueLongErrBackSignal serviceGetValueLongErrBackSignal { get; set; }

        IGetValueLongModel getValueLongModel { set; get; }

        void getValueLong(string url,AllSilenceTimeSectionFromServer body);
    }
}
