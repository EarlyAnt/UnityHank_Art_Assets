using UnityEngine;
using CupSdk.Tmall.AIAudioStatusListener;

namespace CupSdk.Tmall
{
    public class AIAudioStatus
    {
        AndroidJavaClass javaAIAudioStatusClass;
        AndroidJavaObject mAIAudio;
        public AIAudioStatus(){
            // javaAIAudioStatusClass = new AndroidJavaClass("com.bowhead.sheldon.aiaudio.AIAudio");
            // javaAIAudioStatus = javaAIAudioStatusClass.CallStatic<AndroidJavaObject>("getInstance");

            mAIAudio = new AndroidJavaObject("com.bowhead.sheldon.aiaudio.AIAudio");

        }

        public void registerOnLineListener(OnLineListenerNotify listener){
            OnLineListener mOnLineListener = new OnLineListener();
            mOnLineListener.setListener(listener);
            mAIAudio.Call("registerOnLineListener",mOnLineListener);
        }


        public void registerConnectionListener(ConnectionListenerNotify listener){
            ConnectionListener mConnectionListener = new ConnectionListener();
            mConnectionListener.setListener(listener);
            mAIAudio.Call("registerConnectionListener",mConnectionListener);
        }


        public void registerActiveListener(ActiveListenerNotify listener){
            ActiveListener mActiveListener =  new ActiveListener();
            mActiveListener.setListener(listener);
            mAIAudio.Call("registerActiveListener",mActiveListener);
        }

        public void registerContext3rdListener(Context3rdListenerNotify listener){
            Context3rdListener mContext3rdListener = new Context3rdListener();
            mContext3rdListener.setListener(listener);
            mAIAudio.Call("registerContext3rdListener",mContext3rdListener);
        }

        public void registerPushMsgListener(PushMsgListenerNotify listener){
            PushMsgListener mPushMsgListener = new PushMsgListener();
            mPushMsgListener.setListener(listener);
            mAIAudio.Call("registerPushMsgListener",mPushMsgListener);
        }

        public void registerUserBindListener(UserBindListenerNotify listener){
            UserBindListener mUserBindListener = new UserBindListener();
            mUserBindListener.setListener(listener);
            mAIAudio.Call("registerUserBindListener",mUserBindListener);
        }

        public void registerQRCodeListener(QRCodeListenerNotify listener){
            QRCodeListener mQRCodeListener = new QRCodeListener();
            mQRCodeListener.setListener(listener);
            mAIAudio.Call("registerQRCodeListener",mQRCodeListener);
        }

        public void registerAudioListener(AudioListenerNotify listener){
            AIAudioStatusListener.AudioListener mAudioListener = new AIAudioStatusListener.AudioListener();
            mAudioListener.setListener(listener);
            mAIAudio.Call("registerAudioListener",mAudioListener);
        }

        public void registerTTSListener(TTSListenerNotify listener){
            Debug.Log("<><>AIAudioStatus registerTTSListener<><>");
            TTSListener mTTSListener = new TTSListener();
            mTTSListener.setListener(listener);
            mAIAudio.Call("registerTTSListener",mTTSListener);
        }



    }
}