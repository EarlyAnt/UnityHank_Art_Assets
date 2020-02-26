using Gululu;
using Gululu.net;

namespace Hank.Api
{
    public class BaseService : IBaseService
    {
        [Inject]
        public IJsonUtils mJsonUtils { set; get; }

        [Inject]
        public IUrlProvider mUrlProvider { set; get; }

        [Inject]
        public IAuthenticationUtils mAuthenticationUtils { set; get; }

        [Inject]
        public IValidateResponseData mValidateResponseData{ get; set; }

        [Inject]
        public INativeOkHttpMethodWrapper mNativeOkHttpMethodWrapper{ get; set; }
    }
}