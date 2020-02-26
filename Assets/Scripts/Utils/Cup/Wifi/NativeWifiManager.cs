using System;
using Cup.Utils.android;
using CupSdk.BaseSdk.Wifi;
using UnityEngine;

namespace Cup.Utils.Wifi
{
    public class NativeWifiManager
    {
        private static NativeWifiManager instance = null;
        public static NativeWifiManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new NativeWifiManager();
                return instance;
            }
        }

        private WifiManager mWifiManager;
        private AndroidJavaObject context;
        public NativeWifiManager()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            context = AndroidContextHolder.GetAndroidContext();
            AndroidJavaObject systemWifiManager = context.Call<AndroidJavaObject>("getSystemService", "wifi");
            mWifiManager = new WifiManager(systemWifiManager);
#endif
        }

        public void registerReceiver()
        {
            mWifiManager.registerReceiver(context);
        }

        public void unregisterReceiver()
        {
            mWifiManager.unregisterReceiver(context);
            mWifiManager = null;
            context = null;
        }

        public bool startScan(ScanCallback callBack)
        {
            return mWifiManager.startScan(callBack);
        }

        public void registerWifiStateListener(WifiStateListener listener)
        {
            mWifiManager.registerWifiStateListener(listener);
        }

        public void registerConnectionListener(ConnectionListener listener)
        {
            mWifiManager.registerConnectionListener(listener);
        }

        public bool connectWifi(AndroidJavaObject result, string password)
        {
            return mWifiManager.connectWifi(result, password);
        }

        public bool setWifiEnabled(bool enable)
        {
            return mWifiManager.setWifiEnabled(enable);
        }

        public bool isWifiEnabled()
        {
            return mWifiManager.isWifiEnabled();
        }

        public int getWifiState()
        {
            return mWifiManager.getWifiState();
        }

        public AndroidJavaObject getConnectionInfo()
        {
            return mWifiManager.getConnectionInfo();
        }

        public bool disableNetwork(int netId)
        {
            return mWifiManager.disableNetwork(netId);
        }

        public bool disconnect()
        {
            return mWifiManager.disconnect();
        }

        public void acquireWifiLock()
        {
            mWifiManager.acquireWifiLock();
        }

        public void releaseWifiLock()
        {
            mWifiManager.releaseWifiLock();
        }

        public int getSecurity(AndroidJavaObject result)
        {
            return WifiManager.getSecurity(result);
        }

        public int getRssi()
        {
            return mWifiManager.getRssi();
        }

        public bool IsConnected()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            AndroidJavaObject wifiInfo = this.getConnectionInfo();
            if (wifiInfo != null)
            {
                string ssid = wifiInfo.Call<string>("getSSID");
                if (ssid != "<unknown ssid>")
                {
                    return true;
                }
                else return false;
            }
            return false;
#else
            return true;
#endif
        }
    }
}