using System;
using UnityEngine;

namespace CupSdk.BaseSdk.Wifi
{
    public class NativWifiPairManagerCallback : AndroidJavaProxy
    {
        public NativWifiPairManagerCallback() : base("com.bowhead.hank.manager.PairWifiManager$Callback"){}

        public void onStatus(int state){
            if(mStateAction != null){
                mStateAction(state);
            }
        }

        Action<int> mStateAction;
        public void setCallback(Action<int> stateAction){
            mStateAction = stateAction;
        }
    }
}