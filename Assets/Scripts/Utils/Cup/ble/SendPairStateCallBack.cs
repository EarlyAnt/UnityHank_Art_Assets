using UnityEngine;

namespace Cup.Utils.ble
{
    public class SendPairStateCallBack : AndroidJavaProxy
    {
        public SendPairStateCallBack() : base("com.bowhead.hank.ble.BluetoothPairManager$SendPairStateCallBack"){}

        public void onStatus(int status){
            Loom.QueueOnMainThread(() =>
            {
                if(mState != null)
                    mState(status);
            });
        }

        private SendPairState mState;
         public void setListener(SendPairState state){
            mState = state;
        }
    }

    public delegate void SendPairState(int status);
}