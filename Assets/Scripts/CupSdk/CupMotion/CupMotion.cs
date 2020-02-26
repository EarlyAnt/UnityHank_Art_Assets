using Cup.Utils.android;
using CupSdk.CupMotion.Listener;
using UnityEngine;

namespace CupSdk.CupMotion
{
    public class CupMotion
    {
        private AndroidJavaObject javaCupMotion;
        public CupMotion() {
            javaCupMotion = new AndroidJavaObject("com.bowhead.sheldon.motion.CupMotion",AndroidContextHolder.GetAndroidContext());
        }

        public bool startMonitor(AndroidJavaObject handler){
            return javaCupMotion.Call<bool>("startMonitor",handler);
        }

        public bool startMonitor(){
            return javaCupMotion.Call<bool>("startMonitor");
        }

        public void stopMonitor(){
            javaCupMotion.Call("stopMonitor");
        }

        public void registerPoseListener(PoseListener mPostListener){
            javaCupMotion.Call("registerPoseListener",mPostListener);
        }

        public void unregisterPoseListener(){
            javaCupMotion.Call("unregisterPoseListener");
        }

        public void registerTiltListener(TiltListener mTiltListener){
            javaCupMotion.Call("registerTiltListener",mTiltListener);
        }

        public void unregisterTiltListener(){
            javaCupMotion.Call("unregisterTiltListener");
        }

        public void registerShakeListener(ShakeListener mShakeListener){
            javaCupMotion.Call("registerShakeListener",mShakeListener);
        }

        public void unregisterShakeListener(){
            javaCupMotion.Call("unregisterShakeListener");
        }

        public void unregisterAllListener(){
            javaCupMotion.Call("unregisterAllListener");
        }

      
    }
}