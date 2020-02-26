using Gululu.LocalData.DataManager;

namespace Cup.Utils.ble
{
    public interface IP2PManagerWrap
    {
        IAddFriendsInfoManager mAddFriendsInfoManager{get;set;}
        void startP2P(P2PSendInfo sendInfo,P2PSuccess success,P2PFail fail);

        void closeP2P();
    }
}