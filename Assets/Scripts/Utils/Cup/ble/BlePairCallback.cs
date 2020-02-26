using UnityEngine;

namespace Cup.Utils.ble
{
    public class BlePairCallback : AndroidJavaProxy
    {
        public BlePairCallback() : base("com.bowhead.hank.ble.BluetoothPairManager$BlePairCallback"){}


        public void onSuccess(string SSID, string password, string childSN, string PairAction){
            Loom.QueueOnMainThread(() =>
            {
                mSuccess(SSID, password,childSN,PairAction);
            });
        }

        public void onError(int errCode){
            Loom.QueueOnMainThread(() =>
            {
                mError(errCode);
            });
        }

        private BlePairCallbackSuccess mSuccess;
        private BlePairCallbackError mError;
        public void setListener(BlePairCallbackSuccess success, BlePairCallbackError error){
            mSuccess = success;
            mError = error;
        }
    }

    public delegate void BlePairCallbackSuccess(string SSID, string password,string childSN, string PairAction);

    public delegate void BlePairCallbackError(int errCode);
}