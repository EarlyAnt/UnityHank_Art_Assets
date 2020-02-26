using CupSdk.Tmall.AIAudioStatusListener.Result;
using CupSdk.Tmall.AIAudioStatusListener.Utils;
using UnityEngine;

namespace CupSdk.Tmall.AIAudioStatusListener
{
    public class TTSListener : AndroidJavaProxy
    {
        public TTSListener() : base("com.bowhead.sheldon.aiaudio.AIAudio$TTSListener"){}

        public void onNotify(AndroidJavaObject status){
            Debug.Log("<><>TTSListener<>onNotify<>");
            TTS result = TTSListenerUtils.Notofy2Result(status);
            Loom.QueueOnMainThread(()=>{
                mListener(result);
            });
        }

        private TTSListenerNotify mListener;
        public void setListener(TTSListenerNotify listener){
            mListener = listener;
        }
    }

    public delegate void TTSListenerNotify(TTS status);
}