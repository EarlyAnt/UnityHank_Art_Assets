using Newtonsoft.Json;
using UnityEngine;

namespace Gululu
{
    public class NativeOkHttpMethodWrapperCallBack : AndroidJavaProxy
    {
        public NativeOkHttpMethodWrapperCallBack() : base("com.bowhead.hank.network.OKHttpMethodWrapper$CallBack"){}

        public void onResponse(string result){

            GuLog.Info("NativeOkHttpMethodWrapperCallBack: onResponse: "+result);

            NativeResponseData responseData = JsonConvert.DeserializeObject<NativeResponseData>(result);
            
            if(responseData.isSuccess == 1){
                if(mNativeOnResponse != null){
                    mNativeOnResponse(responseData.body);
                }
            }else{
                if(mNativeOnFailure != null){
                    mNativeOnFailure(ResponseErroInfo.GetErrorInfo(responseData.code,responseData.body));
                }
            }
        }

        public void onFailure(){
            GuLog.Info("NativeOkHttpMethodWrapperCallBack: onFailure");
            if(mNativeOnFailure != null){
                mNativeOnFailure(ResponseErroInfo.GetErrorInfo(-1,"http io error"));
            }
        }

        NativeOnResponse mNativeOnResponse;
        NativeOnFailure mNativeOnFailure;
        public void setCallback(NativeOnResponse nativeOnResponse, NativeOnFailure nativeOnFailure){
            mNativeOnResponse = nativeOnResponse;
            mNativeOnFailure = nativeOnFailure;
        }
    }

    public delegate void NativeOnResponse(string result);

    public delegate void NativeOnFailure(ResponseErroInfo responseErroInfo);
}