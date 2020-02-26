using UnityEngine;

namespace CupSdk.CupMotion.Listener
{
    public class ShakeListener : AndroidJavaProxy
    {
        public ShakeListener() : base("com.bowhead.sheldon.motion.CupMotion$ShakeListener"){}

        public void onShake(){
            Loom.QueueOnMainThread(()=>{
                mShake();
            });
        }

        private Shake mShake;

        public void setListener(Shake shake){
            mShake = shake;
        }

    }

    public delegate void Shake();
}