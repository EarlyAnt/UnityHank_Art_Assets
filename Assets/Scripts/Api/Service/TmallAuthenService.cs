using Cup.Utils.android;
using Gululu;
using Gululu.LocalData.Agents;

namespace Hank.Api
{
    public class TmallAuthenService : BaseService,ITmallAuthenService
    {
        [Inject]
        public ServiceTmallAuthBackSignal mTmallAuthBackSignal{get;set;}

        [Inject]
        public ServiceTmallAuthErrBackSignal TmallAuthErrBackSignal {get;set;}

        [Inject]
        public ITmallAuthenModel mTmallAuthenModel {get;set;}

        [Inject]
        public ILocalChildInfoAgent mLocalChildInfoAgent { set; get; }
        public void checkAuthen()
        {
            string url = mUrlProvider.getTmallAuthUrl(mLocalChildInfoAgent.getChildSN());

            mNativeOkHttpMethodWrapper.get(url,"",successCallBack,errorCallBack);
        }

        public void successCallBack(string result)
        {
            TmallAuthResponse tmallAuthResponse = mJsonUtils.String2Json<TmallAuthResponse>(result);
            mTmallAuthenModel.mTmallAuthResponse = tmallAuthResponse;
            Loom.QueueOnMainThread(()=>{
               mTmallAuthBackSignal.Dispatch(tmallAuthResponse);
            });
        }

        public void errorCallBack(ResponseErroInfo errorInfo)
        {
            Loom.QueueOnMainThread(()=>{
                TmallAuthErrBackSignal.Dispatch(errorInfo);
            });
        }
    }
}