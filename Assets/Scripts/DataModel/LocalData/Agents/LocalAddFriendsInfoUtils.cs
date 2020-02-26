using System.Collections.Generic;
using Gululu.LocalData.RequstBody;

namespace Gululu.LocalData.Agents
{
    public class LocalAddFriendsInfoUtils
    {
        [Inject]
        public ILocalDataManager localDataRepository{get;set;}

        [Inject]
        public IJsonUtils mJsonUtils{get;set;}
        public List<FriendInfoItem> getLocalFriendList(string key){
            List<FriendInfoItem> mFriendInfoList;
            string localAddedFriendsInfo = localDataRepository.getObject<string>(key,"");

            if(localAddedFriendsInfo == ""){
                mFriendInfoList = new List<FriendInfoItem>();
            }else{
                mFriendInfoList = mJsonUtils.String2Json<List<FriendInfoItem>>(localAddedFriendsInfo);
            }

            return mFriendInfoList;
        }

        public void deleteLocalAddedFreindsInfo(string key){
            localDataRepository.removeObject(key);
        }

        public void saveFriendList(string key,List<FriendInfoItem> mFriendInfoList){
            string saveFreindsInfo = mJsonUtils.Json2String(mFriendInfoList);
            
            localDataRepository.saveObject<string>(key,saveFreindsInfo);
        }
    }
}