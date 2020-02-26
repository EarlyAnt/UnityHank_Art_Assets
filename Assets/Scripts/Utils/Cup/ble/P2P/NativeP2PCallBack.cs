using UnityEngine;

namespace Cup.Utils.ble
{
    public class NativeP2PCallBack : AndroidJavaProxy
    {
        public NativeP2PCallBack() : base("com.bowhead.hank.ble.P2PManager$P2PCallBack"){}

        public void onSuccess(AndroidJavaObject p2pInfo){
            if(mP2PSuccess != null){
                P2PInfo info = P2PHelpUtils.transforP2Pinfo(p2pInfo);
                Loom.QueueOnMainThread(() =>
                {
                    mP2PSuccess(info);
                });
            }
        }

        public void onFail(){
            if(mP2PFail != null){
                Loom.QueueOnMainThread(() =>
                {
                    mP2PFail();
                });
            }
        }
        
        P2PSuccess mP2PSuccess;
        P2PFail mP2PFail;

        public void setListener(P2PSuccess mP2PSuccess, P2PFail mP2PFail){
            this.mP2PFail = mP2PFail;
            this.mP2PSuccess = mP2PSuccess;
        }
    
    }
    public delegate void P2PSuccess(P2PInfo info);

    public delegate void P2PFail();
}