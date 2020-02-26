using CupSdk.Tmall.AIAudioStatusListener.Result;
using CupSdk.Tmall.AIAudioStatusListener.Utils;
using UnityEngine;

namespace CupSdk.Tmall.AIAudioStatusListener
{
    public class OnLineListener : AndroidJavaProxy
    {
        public OnLineListener() : base("com.bowhead.sheldon.aiaudio.AIAudio$OnLineListener"){}

        public void onNotify(AndroidJavaObject status){
            OnLineListenerResult result = OnLineListenerUtils.Notofy2Result(status);
            Loom.QueueOnMainThread(()=>{
                mListener(result);
            });
            
        }

        private OnLineListenerNotify mListener;
        public void setListener(OnLineListenerNotify listener){
            mListener = listener;
        }
    }

    public delegate void OnLineListenerNotify(OnLineListenerResult status);
}