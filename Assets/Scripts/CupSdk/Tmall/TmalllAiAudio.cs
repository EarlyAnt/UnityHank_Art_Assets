using Cup.Utils.android;
using CupSdk.Tmall.AIAudioStatusListener;
using UnityEngine;

namespace CupSdk.Tmall
{
    public class TmalllAiAudio
    {
        private AndroidJavaObject context;
        private AndroidJavaObject AIAudio;
        public TmalllAiAudio(){
            // AndroidJavaClass aiAudioFactory = new AndroidJavaClass("com.bowhead.sheldon.aiaudio.AIAudioFactory");
	        // AIAudio = aiAudioFactory.CallStatic<AndroidJavaObject>("getTmall");
            context = AndroidContextHolder.GetAndroidContext();
            AIAudio = new AndroidJavaObject("com.bowhead.sheldon.aiaudio.AIAudio",context);
            
        }

        public bool startEngine(AiEngineConnected mAiEngineConnected, AiEngineDisconnected mAiEngineDisconnected){
           

            OnEngineStartCallback mOnEngineStartCallback = new OnEngineStartCallback();
            mOnEngineStartCallback.setListener(mAiEngineConnected,mAiEngineDisconnected);
            
            return AIAudio.Call<bool>("startEngine",mOnEngineStartCallback);
        }

         public void stopEngine(){
            AIAudio.Call("stopEngine");
        }


        public void startRecognize(bool isVad,IAIAudioCallbackListener listener){
            AIAudioListener mAIAudioListener = new AIAudioListener();
            mAIAudioListener.setListener(listener);
            AIAudio.Call("startRecognize",isVad,mAIAudioListener);
        }


       
        // private void startService(IAIAudioCallbackListener listener, bool isVad){
        //     AIAudioCallback callBack = new AIAudioCallback();
        //     callBack.setListener(listener);
        //     AIAudio.Call("startService",context, callBack,isVad);
        // }

       

        public void stopRecognize(){
            AIAudio.Call("stopRecognize");
        }

        public void stopPlay(){
            AIAudio.Call("stopPlay");
        }


        public void registerOnLineListener(OnLineListenerNotify listener){
            OnLineListener mOnLineListener = new OnLineListener();
            mOnLineListener.setListener(listener);
            AIAudio.Call("registerOnLineListener",mOnLineListener);
        }


        public void registerConnectionListener(ConnectionListenerNotify listener){
            ConnectionListener mConnectionListener = new ConnectionListener();
            mConnectionListener.setListener(listener);
            AIAudio.Call("registerConnectionListener",mConnectionListener);
        }


        public void registerActiveListener(ActiveListenerNotify listener){
            ActiveListener mActiveListener =  new ActiveListener();
            mActiveListener.setListener(listener);
            AIAudio.Call("registerActiveListener",mActiveListener);
        }

        public void registerContext3rdListener(Context3rdListenerNotify listener){
            Context3rdListener mContext3rdListener = new Context3rdListener();
            mContext3rdListener.setListener(listener);
            AIAudio.Call("registerContext3rdListener",mContext3rdListener);
        }

        public void registerPushMsgListener(PushMsgListenerNotify listener){
            PushMsgListener mPushMsgListener = new PushMsgListener();
            mPushMsgListener.setListener(listener);
            AIAudio.Call("registerPushMsgListener",mPushMsgListener);
        }

        public void registerUserBindListener(UserBindListenerNotify listener){
            UserBindListener mUserBindListener = new UserBindListener();
            mUserBindListener.setListener(listener);
            AIAudio.Call("registerUserBindListener",mUserBindListener);
        }

        public void registerQRCodeListener(QRCodeListenerNotify listener){
            QRCodeListener mQRCodeListener = new QRCodeListener();
            mQRCodeListener.setListener(listener);
            AIAudio.Call("registerQRCodeListener",mQRCodeListener);
        }

        public void registerAudioListener(AudioListenerNotify listener){
            AIAudioStatusListener.AudioListener mAudioListener = new AIAudioStatusListener.AudioListener();
            mAudioListener.setListener(listener);
            AIAudio.Call("registerAudioListener",mAudioListener);
        }

        public void registerTTSListener(TTSListenerNotify listener){
            Debug.Log("<><>AIAudioStatus registerTTSListener<><>");
            TTSListener mTTSListener = new TTSListener();
            mTTSListener.setListener(listener);
            AIAudio.Call("registerTTSListener",mTTSListener);
        }

        public void unregisterAllListener(){
            AIAudio.Call("unregisterAllListener");
        }

    }
}