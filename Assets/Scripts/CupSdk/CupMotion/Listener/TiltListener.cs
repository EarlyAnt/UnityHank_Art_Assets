using UnityEngine;

namespace CupSdk.CupMotion.Listener
{
    public class TiltListener : AndroidJavaProxy
    {
        public TiltListener() : base("com.bowhead.sheldon.motion.CupMotion$TiltListener"){}

        public void onTiltStatusChange(int status){
            
            Loom.QueueOnMainThread(()=>{
                mTiltStatusChange(status);
            });
        }

        private TiltStatusChange mTiltStatusChange;

        public void setListener(TiltStatusChange tiltStatusChange){
            mTiltStatusChange = tiltStatusChange;
        }
    }

    public delegate void TiltStatusChange(int status);
}