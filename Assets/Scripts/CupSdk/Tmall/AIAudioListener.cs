using CupSdk.Tmall.AIAudioStatusListener.Result;
using CupSdk.Tmall.AIAudioStatusListener.Utils;
using UnityEngine;

namespace CupSdk.Tmall
{
    public class AIAudioListener : AndroidJavaProxy
    {
        public AIAudioListener() : base("com.bowhead.sheldon.aiaudio.AIAudio$Callback"){}

        public void onShow(){
            
            Loom.QueueOnMainThread(() => { 
                Debug.Log("<><>onShow<><>");
                mListener.onShow();
            });
        }

        public void onRecordStart(){
            
            Loom.QueueOnMainThread(() =>
            {
                Debug.Log("<><>onRecordStart<><>");
                mListener.onRecordStart();
            });
        }

//        void onStreaming();

        public void onVolume(int volume){
           
            Loom.QueueOnMainThread(() =>
            {
                 //Debug.Log("<><>onVolume<><>");
                mListener.onVolume(volume);
            });
        }

        public void onRecordStop(){
            
            Loom.QueueOnMainThread(() =>
            {
                Debug.Log("<><>onRecordStop<><>");
                mListener.onRecordStop();
            });
        }

        public void onRecognizeResult(AndroidJavaObject data){
             //Debug.Log("<><>onRecognizeResult<><>1");
            AIAudioData aIAudioData = PushMsgListenerUtils.Notofy2Result(data);
            Loom.QueueOnMainThread(() =>
            {
                //Debug.Log("<><>onRecognizeResult<><>");
                mListener.onRecognizeResult(aIAudioData);
            });
        }

        public void hideUi(){
            Loom.QueueOnMainThread(() =>
            {
                mListener.hideUi();
            });
        }

        public bool isUiShowing(){
            //Debug.Log("<><>isUiShowing<><>");
            return mListener.isUiShowing();
        }

        public void onStreaming(string streamText, bool isFinish){
            Loom.QueueOnMainThread(() =>
            {
                mListener.onStreaming(streamText,isFinish);
            });
        }
        private IAIAudioCallbackListener mListener;
        
        public void setListener(IAIAudioCallbackListener listener){
            mListener = listener;
        }

        public void onError(string error){
            GuLog.Info("AIAudioListener onError: "+error);
            mListener.onError(error);


        }
    }
}