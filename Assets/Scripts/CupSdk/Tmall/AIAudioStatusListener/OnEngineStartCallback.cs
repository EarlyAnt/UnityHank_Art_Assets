using UnityEngine;

namespace CupSdk.Tmall.AIAudioStatusListener
{
    public class OnEngineStartCallback : AndroidJavaProxy
    {
        public OnEngineStartCallback() : base("com.bowhead.sheldon.aiaudio.AIAudio$EngineStatusListener"){}

        public void onEngineConnected(){
            if(mAiEngineConnected != null){
                mAiEngineConnected();
            }
        }

        public void onEngineDisconnected(){
            if(mAiEngineDisconnected != null){
                mAiEngineDisconnected();
            }
        }

        private AiEngineConnected mAiEngineConnected;
        private AiEngineDisconnected mAiEngineDisconnected;
        public void setListener(AiEngineConnected mAiEngineConnected, AiEngineDisconnected mAiEngineDisconnected){
            this.mAiEngineConnected = mAiEngineConnected;
            this.mAiEngineDisconnected = mAiEngineDisconnected;
        }
    }

    public delegate void AiEngineConnected();

    public delegate void AiEngineDisconnected();
}