using CupSdk.Tmall.AIAudioStatusListener.Result;
using CupSdk.Tmall.AIAudioStatusListener.Utils;
using UnityEngine;

namespace CupSdk.Tmall.AIAudioStatusListener
{
    public class ConnectionListener : AndroidJavaProxy
    {
        public ConnectionListener() : base("com.bowhead.sheldon.aiaudio.AIAudio$ConnectionListener")
        {
        }

        public void onNotify(AndroidJavaObject status){
            ConnectionListenerResult result = ConnectionListenerUtils.Notofy2Result(status);
            Loom.QueueOnMainThread(()=>{
                mListener(result);
            });
        }

        private ConnectionListenerNotify mListener;
        public void setListener(ConnectionListenerNotify listener){
            listener = mListener;

        }
    }

    public delegate void ConnectionListenerNotify(ConnectionListenerResult status);
}