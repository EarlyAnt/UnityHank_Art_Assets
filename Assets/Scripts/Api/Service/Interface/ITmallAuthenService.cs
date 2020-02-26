using Gululu.LocalData.Agents;

namespace Hank.Api
{
    public interface ITmallAuthenService
    {
        ServiceTmallAuthBackSignal mTmallAuthBackSignal{ get; set; }

        ServiceTmallAuthErrBackSignal TmallAuthErrBackSignal{ get; set; }
        ITmallAuthenModel mTmallAuthenModel{ get; set; }

        ILocalChildInfoAgent mLocalChildInfoAgent{ get; set; }

        void checkAuthen();
    }
}