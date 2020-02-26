using UnityEngine;

namespace CupSdk.Tmall.AIAudioStatusListener
{
    public class Context3rdListener : AndroidJavaProxy
    {
        public Context3rdListener() : base("com.bowhead.sheldon.aiaudio.AIAudio$Context3rdListener"){}

        public void onNotify(string status){
            Loom.QueueOnMainThread(()=>{
                mListener(status);
            });
        }

        private Context3rdListenerNotify mListener;
        public void setListener(Context3rdListenerNotify listener){
            mListener = listener;
        }  
    }

    public delegate void Context3rdListenerNotify(string status);
}