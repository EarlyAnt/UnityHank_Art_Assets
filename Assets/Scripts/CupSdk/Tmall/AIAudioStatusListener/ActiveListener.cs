using UnityEngine;

namespace CupSdk.Tmall.AIAudioStatusListener
{
    public class ActiveListener : AndroidJavaProxy
    {
        public ActiveListener() : base("com.bowhead.sheldon.aiaudio.AIAudio$ActiveListener"){}

        public void onNotify(string status){
            Loom.QueueOnMainThread(()=>{
                mListener(status);
            });
        }

        private ActiveListenerNotify mListener;
        public void setListener(ActiveListenerNotify listener){
            mListener = listener;
        }
    }


    public delegate void  ActiveListenerNotify(string status);
}