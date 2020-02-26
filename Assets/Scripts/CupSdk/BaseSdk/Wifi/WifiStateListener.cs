using System;
using UnityEngine;

namespace CupSdk.BaseSdk.Wifi
{
    public class WifiStateListener : AndroidJavaProxy
    {
        public WifiStateListener() : base("com.bowhead.sheldon.wifi.WifiManagerWrap$WifiStateListener"){}

        public void onWifiEnabled(){
            GuLog.Debug("WifiStateListener onWifiEnabled");
            if(mWiFiEnable != null){
                Loom.QueueOnMainThread(()=>{
                    mWiFiEnable();
                });
            }
                
        }

        public void onWifiDisabled(){
            GuLog.Debug("WifiStateListener onWifiDisabled");
            if(mWIFIDisable != null){
                Loom.QueueOnMainThread(()=>{
                    mWIFIDisable();
                });
            }
                
        }

        private Action mWiFiEnable;
        private Action mWIFIDisable;
        public void addCallback(Action wiFiEnable, Action wifiDisable){
            mWiFiEnable = wiFiEnable;
            mWIFIDisable = wifiDisable;
        }
    }
}