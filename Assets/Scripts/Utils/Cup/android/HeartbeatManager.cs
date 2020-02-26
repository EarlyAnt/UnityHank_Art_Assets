using System;
using UnityEngine;
using UnityHank.Assets.Script.Utils.Cup.android;

namespace Cup.Utils.android
{
    public class HeartbeatManager : IHeartbeatManager
    {
        HeartbeatListener mHeartbeatListener;
        AndroidJavaClass nativeHeartBeatManager
#if (UNITY_ANDROID) && (!UNITY_EDITOR)
            = new AndroidJavaClass("com.bowhead.hank.HeartBeatManager");
#else
            = null;
#endif
        public void setHeartbeatListener(HeartBeatReveive results)
        {
#if (UNITY_ANDROID) && (!UNITY_EDITOR)
            if (mHeartbeatListener == null)
            {
                mHeartbeatListener = new HeartbeatListener();
                mHeartbeatListener.setListener(results);
                nativeHeartBeatManager.CallStatic("setHeartBeatStartListener", mHeartbeatListener);
            }
#endif
        }

        public void notifyHeartBeatDone()
        {
#if (UNITY_ANDROID) && (!UNITY_EDITOR)
            nativeHeartBeatManager.CallStatic("onHeartBeatDone");
#endif
        }
    }
}