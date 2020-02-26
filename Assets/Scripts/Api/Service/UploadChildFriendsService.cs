using System;
using System.Collections.Generic;
using BestHTTP;
using Cup.Utils.android;
using Gululu;
using Gululu.LocalData.RequstBody;
using Gululu.net;

namespace Hank.Api
{
    public class UploadChildFriendsService : IUploadChildFriendsService
    {
         [Inject]
        public IJsonUtils mJsonUtils { get; set; }

        [Inject]
        public IGululuNetwork mGululuNetwork { get; set; }

        [Inject]
        public IUrlProvider mUrlProvider { get; set; }
        
         [Inject]
        public IUploadChildFriendsModel mUploadChildFriendsModel{ get; set; }

        [Inject]
        public INativeOkHttpMethodWrapper mNativeOkHttpMethodWrapper{ get; set; }

        public void uploadChildFriendsinfo(FriendInfo mFriendInfo,string current_child_sn,Action<UploadFriendsInfoData> successCallback,Action<ResponseErroInfo> errorCallBack){
            
            Dictionary<string, string> head = new Dictionary<string, string>();
            string body = mJsonUtils.Json2String(mFriendInfo);

            // mGululuNetwork.sendRequest(mUrlProvider.getUploadChildFriendsUrl(current_child_sn,CupBuild.getCupSn()),head,body,(result)=>{
                // UploadFriendsInfoData info = mJsonUtils.String2Json<UploadFriendsInfoData>(result);
                // mUploadChildFriendsModel.mUploadFriendsInfoData = info;
                // successCallback(info);
                
            // },(ecb)=>{
            //     errorCallBack(ecb);
            // },HTTPMethods.Post);

            mNativeOkHttpMethodWrapper.post(mUrlProvider.getUploadChildFriendsUrl(current_child_sn,CupBuild.getCupSn()),"",body,(result)=>{
                UploadFriendsInfoData info = mJsonUtils.String2Json<UploadFriendsInfoData>(result);
                mUploadChildFriendsModel.mUploadFriendsInfoData = info;
                successCallback(info);
            },(errorResult)=>{
                errorCallBack(errorResult);
            });
            
        }
    }
}