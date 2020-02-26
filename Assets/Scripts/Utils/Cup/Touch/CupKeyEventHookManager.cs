using Cup.Utils.android;
using CupSdk.BaseSdk.Touch;
using UnityEngine;

namespace Cup.Utils.Touch
{
    public class CupKeyEventHookManager
    {
        private CupKeyEventHookCallback callBack;
        public CupKeyEventHookManager(){
            AndroidJavaObject  context = AndroidContextHolder.GetAndroidContext();
            callBack = new CupKeyEventHookCallback();
            context.Call("setCupKeyEventHookCallback",callBack);
        }

        public void addCallBcak(PowerKeyPress powerKeyPress, PowerKeyLongPress powerKeyLongPress,PowerKeyLongPressUp powerKeyLongPressUp){
            callBack.addListener(powerKeyPress,powerKeyLongPress,powerKeyLongPressUp);
        }
    }
}