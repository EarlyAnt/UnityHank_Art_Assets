using Cup.Utils.android;
using UnityEngine;

namespace Cup.Utils.Touch
{
    public class TouchManager
    {
        public static void setLeftTouchListenerManager(AndroidJavaObject touchManager){
            AndroidJavaObject mainActivity = AndroidContextHolder.GetAndroidContext();
            mainActivity.Call("setLeftTouchListenerManager",touchManager);
        }

        public static void setRightTouchListenerManager(AndroidJavaObject touchManager){
            AndroidJavaObject mainActivity = AndroidContextHolder.GetAndroidContext();
            mainActivity.Call("setRightTouchListenerManager",touchManager);
        }
    }
}