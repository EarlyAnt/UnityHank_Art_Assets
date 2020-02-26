using System.Collections.Generic;
using BestHTTP;
using Gululu;
using Gululu.net;

namespace Hank.Api
{
    public class GetValueLongService : BaseService,IGetValueLongService
    {
        [Inject]
        public ServiceGetValueLongBackSignal serviceGetValueLongBackSignal { get; set; }

        [Inject]
        public ServiceGetValueLongErrBackSignal serviceGetValueLongErrBackSignal { get; set; }
        [Inject]
        public IGetValueLongModel getValueLongModel { get; set; }

       
        public void getValueLong(string url, AllSilenceTimeSectionFromServer body)
        {
            // Dictionary<string, string> head = new Dictionary<string, string>();

            string strbody = mJsonUtils.Json2String(body);

            // mGululuNetwork.sendRequest(url, head, strbody, (r) =>
            // {
            //     serviceGetValueLongBackSignal.Dispatch(r);
            //     getValueLongModel.result = r;

            // }, (e) =>
            // {
            //     serviceGetValueLongErrBackSignal.Dispatch(e);
            // }, HTTPMethods.Post);

            mNativeOkHttpMethodWrapper.post(url, "", strbody, (r) =>
            {
                serviceGetValueLongBackSignal.Dispatch(r);
                getValueLongModel.result = r;

            }, (e) =>
            {
                serviceGetValueLongErrBackSignal.Dispatch(e);
            });
        }
    }
}