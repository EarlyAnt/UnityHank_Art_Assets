using Cup.Utils.android;
using UnityEngine;

namespace Cup.Utils.ble
{
    public class P2PManager
    {
        public AndroidJavaObject nativeP2PManager;
        public P2PManager(P2PSuccess success,P2PFail fail){
            AndroidJavaClass nativeP2PManagerBuilder = new AndroidJavaClass("com.bowhead.hank.ble.P2PManagerBuilder");

            NativeP2PCallBack mNativeP2PCallBack = new NativeP2PCallBack();
            mNativeP2PCallBack.setListener(success,fail);

            nativeP2PManager = nativeP2PManagerBuilder.CallStatic<AndroidJavaObject>("createP2PManager",AndroidContextHolder.GetAndroidContext(),mNativeP2PCallBack);
        }

        public void startP2P(string childsn, string cupSn, string petType,int scanTimeOut,int waitTimeOut ){
            nativeP2PManager.Call("startP2P", childsn,cupSn,petType,scanTimeOut,waitTimeOut);
        }

        public void closeP2P(){
            nativeP2PManager.Call("closeP2P");
        }
    }
}