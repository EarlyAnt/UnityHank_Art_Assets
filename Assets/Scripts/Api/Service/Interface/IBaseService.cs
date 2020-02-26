using Gululu;
using Gululu.net;

namespace Hank.Api
{
    public interface IBaseService
    {
        [Inject]
        IJsonUtils mJsonUtils { set; get; }

        [Inject]
        IUrlProvider mUrlProvider { set; get; }

        [Inject]
        IAuthenticationUtils mAuthenticationUtils { set; get; }

        [Inject]
        IValidateResponseData mValidateResponseData { set; get; }

        [Inject]
        INativeOkHttpMethodWrapper mNativeOkHttpMethodWrapper{ get; set; }
    }
}