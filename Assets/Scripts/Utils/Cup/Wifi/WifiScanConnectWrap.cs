using System;
using System.Collections.Generic;
using CupSdk.BaseSdk.Wifi;
using UnityEngine;

namespace Cup.Utils.Wifi
{
    public class WifiScanConnectWrap
    {
        private NativeWifiManager nativeWifiManager;

        private ScanCallback mScanCallback;

        private Dictionary<string, AndroidJavaObject> scanWifiResults = new Dictionary<string, AndroidJavaObject>();
        public WifiScanConnectWrap(NativeWifiManager wifiManager){
            nativeWifiManager = wifiManager;
            mScanCallback = new ScanCallback(); 
            
        }
        public void startScan(Action<List<string>> SSIDs,Action<bool> failed){
            mScanCallback.addListener((r)=>{
                processScanResult(r,SSIDs);
            },failed);

            nativeWifiManager.startScan(mScanCallback);
        }

        private void processScanResult(AndroidJavaObject scanResults,Action<List<string>> SSIDs){
            int count = scanResults.Call<int>("size");
            List<string> cupSsidList = new List<string>();
            for (int i = 0; i < count; i++)
            {
                AndroidJavaObject scanResult = scanResults.Call<AndroidJavaObject>("get", i);

                if (!string.IsNullOrEmpty(scanResult.Get<string>("SSID")))
                {
                    string ssid = scanResult.Get<string>("SSID");
                    string realSSID = ssid.Replace("\"", "");
                    cupSsidList.Add(realSSID);
                    if (!scanWifiResults.ContainsKey(realSSID))
                    {
                        scanWifiResults.Add(realSSID, scanResult);
                    }else{
                                    //TODO do update scanWifiResult
                    }
                }
            }

            SSIDs(cupSsidList);
        }


        public bool connectWifi(string ssid, string passwd){
            if(!scanWifiResults.ContainsKey(ssid)){
                return false;
            }
            AndroidJavaObject result = scanWifiResults[ssid];

            return nativeWifiManager.connectWifi(result,passwd);
        }

        public AndroidJavaObject getWifiScanResultByName(string ssid){
            if(!scanWifiResults.ContainsKey(ssid)){
                return null;
            }
            AndroidJavaObject result = scanWifiResults[ssid];

            return result;
        }

        public bool isValidateForEmptyPwd(string ssid){
            if(!scanWifiResults.ContainsKey(ssid)){
                return false;
            }
            AndroidJavaObject result = scanWifiResults[ssid];

            int type = nativeWifiManager.getSecurity(result);

            if(type == 0){
                return true;
            }

            return false;
        }

        public bool disconnectWifi(){
            return nativeWifiManager.disconnect();
        }
    }
}