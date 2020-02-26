using System;
using Gululu.LocalData.DataManager;
using Gululu.Util;

namespace Cup.Utils.ble
{


    public class P2PManagerWrap : IP2PManagerWrap
    {
        [Inject]
        public IAddFriendsInfoManager mAddFriendsInfoManager{get;set;}
        
        P2PManager mP2PManager;

        public void closeP2P()
        {
            if(mP2PManager != null){
                mP2PManager.closeP2P();
            }

            mP2PManager = null;
        }

        public void startP2P(P2PSendInfo sendInfo, P2PSuccess success, P2PFail fail)
        {
            string childsn = sendInfo.childsn;
            
            string cupSn = sendInfo.cupSn;
            
            string petType = sendInfo.petType;

            int scanTimeOut = sendInfo.scanTimeOut;

            int waitTimeOut = sendInfo.waitTimeOut;
            
            if(!ValidSendInfo(sendInfo)){
                fail();
                return;
            }

            mP2PManager = new P2PManager((info)=>{
                GuLog.Info("p2p info:"+"child_sn:"+info.child_sn+" cup_sn:"+info.cup_sn);
                mAddFriendsInfoManager.addFriend(info.cup_sn,DateUtil.GetTimeStamp());
                success(info);
            },fail);

            mP2PManager.startP2P(childsn,cupSn,petType,scanTimeOut,waitTimeOut);
        }

        public bool ValidSendInfo(P2PSendInfo sendInfo){
            if(String.IsNullOrEmpty(sendInfo.childsn)
             || String.IsNullOrEmpty(sendInfo.childsn) 
             || String.IsNullOrEmpty(sendInfo.childsn) 
             || sendInfo.scanTimeOut < 1500 
             || sendInfo.waitTimeOut < 1500){
                 return false;
            }

            return true;
        }
    }
}