using UnityEngine;

namespace UnityHank.Assets.Script.Utils.Cup.android
{
    public class HeartbeatListener : AndroidJavaProxy
    {
        public HeartbeatListener() : base("com.bowhead.hank.HeartBeatManager$HeartBeatStartListener"){}

        public void onStart(){

            GuLog.Info("HeartbeatListener onStart");
            if(mHeartBeatReveive != null){
                mHeartBeatReveive();
            }
        }

        private HeartBeatReveive mHeartBeatReveive;
        public void setListener(HeartBeatReveive mHeartBeatReveive){
            this.mHeartBeatReveive = mHeartBeatReveive;
        }
    }

    public delegate void HeartBeatReveive();
}