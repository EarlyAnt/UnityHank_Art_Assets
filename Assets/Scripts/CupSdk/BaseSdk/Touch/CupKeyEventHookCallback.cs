using UnityEngine;

namespace CupSdk.BaseSdk.Touch
{
    public class CupKeyEventHookCallback : AndroidJavaProxy
    {
        public CupKeyEventHookCallback() : base("com.bowhead.sheldon.cupkey.CupKeyEventHook$Callback"){}

        public void onPowerKeyPress(){
            if(mPowerKeyPress != null)
                mPowerKeyPress();
        }

        public void onPowerKeyLongPress(){
            if(mPowerKeyLongPress != null)
                mPowerKeyLongPress();
        }

        public void onPowerKeyLongPressUp(){
            if(mPowerKeyLongPressUp != null){
                mPowerKeyLongPressUp();
            }
        }

        private PowerKeyPress mPowerKeyPress;
        private PowerKeyLongPress mPowerKeyLongPress;

        private PowerKeyLongPressUp mPowerKeyLongPressUp;
        public void addListener(PowerKeyPress powerKeyPress, PowerKeyLongPress powerKeyLongPress, PowerKeyLongPressUp powerKeyLongPressUp){
            mPowerKeyPress = powerKeyPress;
            mPowerKeyLongPress = powerKeyLongPress;
            mPowerKeyLongPressUp = powerKeyLongPressUp;
        }
    }

    public delegate void PowerKeyPress();

    public delegate void PowerKeyLongPress();

    public delegate void PowerKeyLongPressUp();
    
}