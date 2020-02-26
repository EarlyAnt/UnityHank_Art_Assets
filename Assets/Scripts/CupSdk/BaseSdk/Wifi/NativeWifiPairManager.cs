using System;
using Cup.Utils.android;
//using Hank.CupPair;
using UnityEngine;

namespace CupSdk.BaseSdk.Wifi
{
    //public class NativeWifiPairManager : IPairWifiManager
    //{
    //    AndroidJavaObject mWifiPairManager;
    //    public void initAndOpenWifi()
    //    {
    //        if (mWifiPairManager == null)
    //        {
    //            mWifiPairManager = new AndroidJavaObject("com.bowhead.hank.manager.PairWifiManager", AndroidContextHolder.GetAndroidContext());
    //        }

    //        mWifiPairManager.Call("initAndOpenWifi");
    //    }

    //    public void close()
    //    {
    //        mWifiPairManager.Call("close");
    //    }

    //    public void connectWifi(string SSID, string pwd, Action<WIFIState> connectStateCallback)
    //    {
    //        NativWifiPairManagerCallback callback = new NativWifiPairManagerCallback();
    //        callback.setCallback((state) =>
    //        {
    //            WIFIState wifiState = transInt2WIFIState(state);
    //            connectStateCallback(wifiState);
    //        });

    //        mWifiPairManager.Call("connectWifi", SSID, pwd, callback);
    //    }


    //    private WIFIState transInt2WIFIState(int state)
    //    {
    //        GuLog.Info("transInt2WIFIState: "+state);
    //        WIFIState mWifiState = WIFIState.WIFI_CONNECT_ERROR;
    //        switch (state)
    //        {
    //            case 1:
    //                mWifiState = WIFIState.WIFI_START_SCAN_ERROR;
    //                break;
    //            case 2:
    //                mWifiState = WIFIState.WIFI_PWD_INVALIDATE;
    //                break;
    //            case 3:
    //                mWifiState = WIFIState.WIFI_CONNECT_ERROR;
    //                break;
    //            case 4:
    //                mWifiState = WIFIState.WIFI_CONNECTING;
    //                break;
    //            case 5:
    //                mWifiState = WIFIState.WIFI_SCANNING;
    //                break;
    //            case 6:
    //                mWifiState = WIFIState.WIFI_SCAN_FINISH;
    //                break;
    //            case 7:
    //                mWifiState = WIFIState.WIFI_CONNECTED;
    //                break;
    //            case 8:
    //                mWifiState = WIFIState.WIFI_DISCONNECT;
    //                break;
    //            case 9:
    //                mWifiState = WIFIState.WIFI_AUTHING;
    //                break;
    //            case 10:
    //                mWifiState = WIFIState.WIFI_AUTH_ERROR;
    //                break;
    //            case 11:
    //                mWifiState = WIFIState.WIFI_CONNECT_OTHER;
    //                break;
    //            case 12:
    //                mWifiState = WIFIState.WIFI_CONNECT_OTHER_AUTHEN_ERROR;
    //                break;
    //            case 13:
    //                mWifiState = WIFIState.WIFI_SCAN_INIT;
    //                break;
    //        }

    //        return mWifiState;
    //    }
    //}
}