using CupSdk.BaseSdk.Touch;
using UnityEngine;

namespace Cup.Utils.Touch
{
    public class CupKeyEventHookStaticManager
    {
        private static CupKeyEventHookManager mCupKeyEventHookManager = new CupKeyEventHookManager();

        private static event PowerKeyPress PowerKeyPressEvent = delegate(){};
        private static event PowerKeyLongPress PowerKeyLongPressEvent = delegate(){};
        private static event PowerKeyLongPressUp PowerKeyLongPressUpEvent = delegate(){};

        private static bool isStart = false;

        private static void start(){
            Debug.Log("<><><CupKeyEventHookManager.start><><>");
            if(!isStart){
                mCupKeyEventHookManager.addCallBcak(staticPowerKeyPress,staticPowerKeyLongPress,staticPowerKeyLongPressUp);
                isStart = true;
            }
            
        }

        public static void addPowerKeyPressCallBack(PowerKeyPress powerKeyPress){
            start();
            PowerKeyPressEvent += powerKeyPress;
        }

        public static void addPowerKeyLongPressCallBack(PowerKeyLongPress powerKeyLongPress){
            start();
            PowerKeyLongPressEvent += powerKeyLongPress;
        }

        public static void addPowerKeyLongPressUpCallBack(PowerKeyLongPressUp powerKeyLongPressUp){
            start();
            PowerKeyLongPressUpEvent += powerKeyLongPressUp;
        }

        public static void removePowerKeyPressCallBack(PowerKeyPress powerKeyPress){
            PowerKeyPressEvent -= powerKeyPress;
        }

        public static void removePowerKeyLongPressCallBack(PowerKeyLongPress powerKeyLongPress){
            PowerKeyLongPressEvent -= powerKeyLongPress;
        }

        public static void removePowerKeyLongPressUpCallBack(PowerKeyLongPressUp powerKeyLongPressUp){
            PowerKeyLongPressUpEvent -= powerKeyLongPressUp;
        }

        

        
        private static void staticPowerKeyPress(){
            Debug.Log("<><>staticPowerKeyPress<><>");
            Loom.QueueOnMainThread(() =>
            {
                PowerKeyPressEvent();
            });
        }

        private static void staticPowerKeyLongPress(){
            Debug.Log("<><>staticPowerKeyLongPress<><>");
            Loom.QueueOnMainThread(() =>
            {
                PowerKeyLongPressEvent();
            });
        }

        private static void staticPowerKeyLongPressUp(){
            Debug.Log("<><>staticPowerKeyLongPressUp<><>");
            Loom.QueueOnMainThread(() =>
            {
                PowerKeyLongPressUpEvent();
            });
        }
    }
}