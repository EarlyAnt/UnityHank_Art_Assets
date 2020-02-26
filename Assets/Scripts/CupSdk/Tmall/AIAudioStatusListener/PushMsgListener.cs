using CupSdk.Tmall.AIAudioStatusListener.Result;
using CupSdk.Tmall.AIAudioStatusListener.Utils;
using UnityEngine;

namespace CupSdk.Tmall.AIAudioStatusListener
{
    public class PushMsgListener : AndroidJavaProxy
    {
        public PushMsgListener() : base("com.bowhead.sheldon.aiaudio.AIAudio$PushMsgListener"){}

        public void onNotify(AndroidJavaObject status){
            AIAudioData result = PushMsgListenerUtils.Notofy2Result(status);
            Loom.QueueOnMainThread(()=>{
                mListener(result);
            });
        }

        private PushMsgListenerNotify mListener;

        public void setListener(PushMsgListenerNotify listener){
            mListener = listener;
        }

    }
    

    public delegate void PushMsgListenerNotify(AIAudioData status);
}