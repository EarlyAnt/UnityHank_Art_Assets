using UnityEngine;

namespace Cup.Utils.ble
{
    public class P2PHelpUtils
    {
        public static P2PInfo transforP2Pinfo(AndroidJavaObject nativeP2p){
            P2PInfo info = new P2PInfo();
            info.child_sn = nativeP2p.Call<string>("getChild_sn");
            info.cup_sn = nativeP2p.Call<string>("getCup_sn");
            info.pet_type = nativeP2p.Call<string>("getPet_type");
            info.role = nativeP2p.Call<string>("getRole");

            return info;
        }
    }
}