using System;
using Gululu;
using Gululu.LocalData.RequstBody;
using Gululu.net;
using Hank.Api;

namespace Hank.Api
{
    public interface IUploadChildFriendsService
    {
        IJsonUtils mJsonUtils { get; set; }

        IGululuNetwork mGululuNetwork { get; set; }

        IUrlProvider mUrlProvider { get; set; }

        IUploadChildFriendsModel mUploadChildFriendsModel { get; set; }
        void uploadChildFriendsinfo(FriendInfo mFriendInfo,string current_child_sn,Action<UploadFriendsInfoData> successCallback,Action<ResponseErroInfo> errorCallBack);
    }
}