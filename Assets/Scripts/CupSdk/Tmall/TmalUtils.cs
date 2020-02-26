using Cup.Utils.android;
using UnityEngine;

namespace CupSdk.Tmall
{
    public class TmalUtils
    {
        public static AndroidJavaClass aiAudioFactory;
        public static AIAudioListener callBack;
        public static void startTmall(IAIAudioCallbackListener listener){
            if(aiAudioFactory == null){
                callBack = new AIAudioListener();
                callBack.setListener(listener);
                aiAudioFactory = new AndroidJavaClass("com.bowhead.sheldon.aiaudio.AIAudioFactory");

		        AndroidJavaObject AIAudio = aiAudioFactory.CallStatic<AndroidJavaObject>("getTmall");

                AndroidJavaObject context = AndroidContextHolder.GetAndroidContext();

		        AIAudio.Call("startService",context, callBack);
            }else{
                callBack.setListener(listener);
            }

        }
    }
}