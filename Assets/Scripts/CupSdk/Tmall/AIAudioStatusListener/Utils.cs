using CupSdk.Tmall.AIAudioStatusListener.Result;
using UnityEngine;

namespace CupSdk.Tmall.AIAudioStatusListener.Utils
{
    public class ActiveListenerUtils
    {
        public static TmallAudioListenerResult Notofy2Audio(AndroidJavaObject status){
            TmallAudioListenerResult audio = new TmallAudioListenerResult();
            audio.audioID = status.Call<int>("getAudioID");
            audio.status = status.Call<int>("getStatus");
            return audio;
        }
    }

    public class ConnectionListenerUtils{
        public static ConnectionListenerResult Notofy2Result(AndroidJavaObject status){
            ConnectionListenerResult result = new ConnectionListenerResult();
            result.status = status.Call<int>("getStatus");
            return result;
        }
    }

    public class OnLineListenerUtils{

        public static OnLineListenerResult Notofy2Result(AndroidJavaObject status){
            OnLineListenerResult result = new OnLineListenerResult();
            result.status = status.Call<int>("getStatus");
            return result;
        }
    }

    public class UserBindListenerUtils{
        public static UserBindListenerResult Notofy2Result(AndroidJavaObject status){
            UserBindListenerResult result = new UserBindListenerResult();
            result.mStatus = status.Call<int>("getStatus");
            result.mNickName = status.Call<string>("getNickName");
            result.mUserID = status.Call<string>("getUserID");
            result.mAvatar = status.Call<string>("getAvatar");
            result.mLocation = status.Call<string>("getLocation");
            result.mUserType = status.Call<int>("getUserType");
            return result;
        }
    }

    public class PushMsgListenerUtils{
        public static AIAudioData Notofy2Result(AndroidJavaObject status){
            AIAudioData result = new AIAudioData();
            result.code = status.Call<int>("getCode");
            result.type = status.Call<int>("getType");
            result.question = status.Call<string>("getQuestion");
            result.domain = status.Call<string>("getDomain");
            result.intent = status.Call<string>("getIntent");
            result.spokenText = status.Call<string>("getSpokenText");
            result.writtenText = status.Call<string>("getWrittenText");
            result.emotion = status.Call<int>("getEmotion");
            result.extraData = status.Call<string>("getExtraData");
            return result;
        }
    }

    public class TTSListenerUtils{
        public static TTS Notofy2Result(AndroidJavaObject status){
            TTS result = new TTS();
            result.status = status.Call<int>("getStatus");
            return result;
        }
    }
}