using System.Collections;
using System.Collections.Generic;
using Gululu.LocalData.RequstBody;

namespace Gululu.LocalData.Agents
{
    public class LocalAddFriendsInfo : ILocalAddFriendsInfo
    {

        [Inject]
        public IJsonUtils mJsonUtils{get;set;}

        [Inject]
        public LocalAddFriendsInfoUtils mLocalAddFriendsInfoUtils{get;set;}

        public void addFriend(string current_child_sn,string CupHWSN,long time_stamp){

            if(current_child_sn == null || current_child_sn ==""){
                return;
            }

            FriendInfoItem mFriendInfoItem =  new FriendInfoItem(CupHWSN,time_stamp);
            
            List<FriendInfoItem> mFriendInfoList = mLocalAddFriendsInfoUtils.getLocalFriendList(getSaveKey(current_child_sn));

            mFriendInfoList.Add(mFriendInfoItem);

            mLocalAddFriendsInfoUtils.saveFriendList(getSaveKey(current_child_sn),mFriendInfoList);
        }

       

        public List<FriendInfoItem> getAllAddedFriendsInfo(string current_child_sn){

            List<FriendInfoItem> mFriendInfoList = mLocalAddFriendsInfoUtils.getLocalFriendList(getSaveKey(current_child_sn));

            return mFriendInfoList;
        }

        public void deleteAddedFriendsInfo(string current_child_sn){
            mLocalAddFriendsInfoUtils.deleteLocalAddedFreindsInfo(getSaveKey(current_child_sn));
        }

        private string getSaveKey(string child_sn){
            return child_sn+"_CHILD_FRIENDS_INFO";
        }
    }
}