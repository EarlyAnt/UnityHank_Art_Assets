using CupSdk.Tmall.AIAudioStatusListener.Result;
using CupSdk.Tmall.AIAudioStatusListener.Utils;
using UnityEngine;

namespace CupSdk.Tmall.AIAudioStatusListener
{
    public class AudioListener : AndroidJavaProxy
    {
        public AudioListener() : base("com.bowhead.sheldon.aiaudio.AIAudio$AudioListener"){}

        public void onNotify(AndroidJavaObject status){
            TmallAudioListenerResult audio = ActiveListenerUtils.Notofy2Audio(status);
            Loom.QueueOnMainThread(()=>{
                mListener(audio);
            });
        }

        private AudioListenerNotify mListener;
        public void setListener(AudioListenerNotify listener){
            mListener = listener;
        }
    }

    public delegate void AudioListenerNotify(TmallAudioListenerResult status);
}