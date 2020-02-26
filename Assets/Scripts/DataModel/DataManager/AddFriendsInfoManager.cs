using System;
using System.Collections.Generic;
using Gululu.LocalData.Agents;
using Gululu.LocalData.RequstBody;
using Gululu.net;
using Hank.Api;
using UnityEngine;

namespace Gululu.LocalData.DataManager
{
    public class AddFriendsInfoManager : IAddFriendsInfoManager
    {


        [Inject]
        public ILocalChildInfoAgent mLocalChildInfoAgent { get; set; }

        [Inject]
        public ILocalAddFriendsInfo mLocalAddFriendsInfo { get; set; }

        [Inject]
        public IUploadChildFriendsService mUploadChildFriendsService { get; set; }

        [Inject]
        public INetUtils mNetUtils { get; set; }

        public void addFriend(string CupHWSN, long time_stamp)
        {
            Debug.Log("addNewFriends: CupHWSN:" + CupHWSN + "  time_stamp: " + time_stamp);

            string currentChildSN = mLocalChildInfoAgent.getChildSN();
            mLocalAddFriendsInfo.addFriend(currentChildSN, CupHWSN, time_stamp);
            uploadBackupData();
        }


        public void uploadBackupData()
        {
            if (mNetUtils.isNetworkEnable())
            {
                upload((cb) => { }, (ecb) => { });
            }
        }

        public void upload(Action<UploadFriendsInfoData> callBack, Action<ResponseErroInfo> errCallBack)
        {
            string currentChildSN = mLocalChildInfoAgent.getChildSN();
            if (currentChildSN == string.Empty)
            {
                errCallBack(ResponseErroInfo.GetErrorInfo(0,"upload resource is empty"));
                return;
            }
            List<FriendInfoItem> info = mLocalAddFriendsInfo.getAllAddedFriendsInfo(currentChildSN);

            if (info != null && info.Count > 0)
            {
                FriendInfo mFriendInfo = new FriendInfo();
                mFriendInfo.logs = info;

                mUploadChildFriendsService.uploadChildFriendsinfo(mFriendInfo, currentChildSN, (cb) =>
                {
                    Debug.Log("uploadChildFriendsinfo: success");
                    mLocalAddFriendsInfo.deleteAddedFriendsInfo(currentChildSN);
                    callBack(cb);
                }, (ecb) =>
                {
                    Debug.Log("uploadChildFriendsinfo: error");
                    errCallBack(ecb);
                });
            }else{
                errCallBack(ResponseErroInfo.GetErrorInfo(0,"upload resource is empty"));
            }


        }
    }
}