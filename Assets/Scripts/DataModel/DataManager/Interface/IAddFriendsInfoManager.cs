using System;
using Gululu.LocalData.Agents;
using Hank.Api;

namespace Gululu.LocalData.DataManager
{
    public interface IAddFriendsInfoManager
    {
        ILocalChildInfoAgent mLocalChildInfoAgent { get; set; }

        ILocalAddFriendsInfo mLocalAddFriendsInfo { get; set; }

        INetUtils mNetUtils{ get; set; }

        IUploadChildFriendsService mUploadChildFriendsService{ get; set; }
        
        void addFriend(string CupHWSN, long time_stamp);


        void uploadBackupData();

        void upload(Action<UploadFriendsInfoData> callBack, Action<ResponseErroInfo> errCallBack);
    }
}
