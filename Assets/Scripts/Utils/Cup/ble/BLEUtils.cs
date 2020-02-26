using System;
using Cup.Utils.android;
using UnityEngine;

namespace Cup.Utils.ble
{
    public class BLEUtils : IBLEUtils
    {
        public AndroidJavaObject BleManager;

        public void init(){
            if(BleManager == null){
                Debug.Log("BLEUtils init");
                BleManager  = new AndroidJavaObject("com.bowhead.hank.ble.BluetoothPairManager",AndroidContextHolder.GetAndroidContext());
            }
            
        }

        
        public void StartPairBLE(BlePairCallbackSuccess success, BlePairCallbackError error)
        {
            Debug.Log("<><><>StartBLE<><>");
            AndroidJavaObject context = AndroidContextHolder.GetAndroidContext();

            BlePairCallback mBlePairCallback = new BlePairCallback();
            mBlePairCallback.setListener(success,error);
            bool result = BleManager.Call<bool>("startPairServer",context,mBlePairCallback);

             Debug.Log("<><><>StartBLE result <><>"+result);
        }

        public void StopPairBLE()
        {
            Debug.Log("<><><>StopBLE<><>");
            AndroidJavaObject context = AndroidContextHolder.GetAndroidContext();

            BleManager.Call("closePairServer",context);

        }

        public bool ISBLEEnable()
        {
            return BleManager.Call<bool>("isBluetoothEnabled");
        }

        public void SetBleEnable(bool isEnable){
            AndroidJavaObject context = AndroidContextHolder.GetAndroidContext();

            BleManager.Call("setBluetoothEnabled",context,isEnable);
        }


        public void sendPairState(string pairStatus)
        {
            Debug.Log("<><><>setPairState<><>");
            AndroidJavaObject context = AndroidContextHolder.GetAndroidContext();

            SendPairStateCallBack callBack = new SendPairStateCallBack();
            callBack.setListener((int status)=>{

            });

            BleManager.Call("sendPairState",pairStatus,callBack);

        }

        public void sendPairState(PairMessage message){
            string strmessage = Enum.GetName(typeof(PairMessage),message);

            Debug.Log("<><>setPairState<><>"+strmessage);

            sendPairState(strmessage);
        }
    }

    public enum PairMessage{
        NONE = -1,
        REGISTER_SUCCESS = 1,
        PASSWORD_ERROR = 2,
        REGISTER_ERROR = 3,

        SSID_NOT_FOUND = 4,

        WIFI_SCAN_ERROR = 5,

        BLE_START_ERROR = 6,

        RECEIVED_PW_SSID = 7,

        CONNECTED_WIFI = 8,

        CONNECTED_SERVICE = 9,

        UPDATE_SUCCESS = 10,

        UPDATE_ERROR = 11,

        WIFI_CONNECT_ERROR = 12
    }
}