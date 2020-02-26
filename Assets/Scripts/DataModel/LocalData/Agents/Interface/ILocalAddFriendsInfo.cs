using System.Collections.Generic;
using Gululu.LocalData.RequstBody;

namespace Gululu.LocalData.Agents
{
    public interface ILocalAddFriendsInfo
    {
        IJsonUtils mJsonUtils { get; set; }
        void addFriend(string current_child_sn, string CupHWSN, long time_stamp);

        List<FriendInfoItem> getAllAddedFriendsInfo(string current_child_sn);

        void deleteAddedFriendsInfo(string current_child_sn);
    }
}