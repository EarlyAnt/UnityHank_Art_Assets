using CupSdk.Tmall.AIAudioStatusListener.Result;
using CupSdk.Tmall.AIAudioStatusListener.Utils;
using UnityEngine;

namespace CupSdk.Tmall.AIAudioStatusListener
{
    public class UserBindListener : AndroidJavaProxy
    {
        public UserBindListener() : base("com.bowhead.sheldon.aiaudio.AIAudio$UserBindListener"){}

        public void onNotify(AndroidJavaObject status){
            UserBindListenerResult result = UserBindListenerUtils.Notofy2Result(status);
            Loom.QueueOnMainThread(()=>{
                mListener(result);
            });
        }
 
        private UserBindListenerNotify mListener;

        public void setListener(UserBindListenerNotify listener){
            mListener = listener;
        }
    }


    public delegate void UserBindListenerNotify(UserBindListenerResult status);
}