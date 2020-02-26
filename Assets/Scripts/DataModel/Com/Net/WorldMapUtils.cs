using Cup.Utils.android;
using Gululu.LocalData.Agents;
using Hank;
using Hank.Api;
using System;
using UnityEngine;

namespace Gululu.net
{
    //世界地图页数据同步工具
    public class WorldMapUtils : IWorldMapUtils
    {
        [Inject]
        public IUrlProvider UrlProvider { get; set; }
        [Inject]
        public IJsonUtils JsonUtils { get; set; }
        [Inject]
        public ILocalChildInfoAgent LocalChildInfoAgent { get; set; }
        [Inject]
        public INativeOkHttpMethodWrapper NativeOkHttpMethodWrapper { get; set; }

        public void GetAllMissionInfo(Action<AllMissionInfo> callBack = null, Action<string> errCallBack = null)
        {
            string url = UrlProvider.GetAllMissionInfo(this.LocalChildInfoAgent.getChildSN(), CupBuild.getCupSn());
            Debug.LogFormat("<><WorldMapUtils.GetAllMissionInfo>ChildSN: {0}, CupSN: {1}, Url: {2}", this.LocalChildInfoAgent.getChildSN(), CupBuild.getCupSn(), url);

            NativeOkHttpMethodWrapper.get(url, "", (result) =>
            {
                Debug.LogFormat("<><WorldMapUtils.GetAllMissionInfo>Result: {0}", result);
                GetAllMissionInfoResponse response = this.JsonUtils.String2Json<GetAllMissionInfoResponse>(result);
                if (response != null && response.highest_mission_data != null)
                {
                    AllMissionInfo allMissionInfo = new AllMissionInfo();
                    string starID = "";
                    string missionID = "";
                    for (int i = 0; i < response.highest_mission_data.Count; i++)
                    {
                        starID = response.highest_mission_data[i].star_id;
                        missionID = response.highest_mission_data[i].mission_id;
                        if (string.IsNullOrEmpty(starID) || string.IsNullOrEmpty(missionID) && int.Parse(starID) != PlayerData.defaultStar)
                        {
                            Debug.LogAssertionFormat("<><WorldMapUtils.GetAllMissionInfo>Response is invalid, starID: {0}, missionID: {1}", starID, missionID);
                            continue;
                        }
                        else if (string.IsNullOrEmpty(missionID) && int.Parse(starID) == PlayerData.defaultStar)
                        {
                            missionID = PlayerData.startMission.ToString();
                        }

                        allMissionInfo.Items.Add(new MissionInfo() { StarID = starID, MissionID = missionID });
                    }
                    Debug.LogFormat("<><WorldMapUtils.GetAllMissionInfo>Response data is valid: {0}", result);
                    if (callBack != null) callBack(allMissionInfo);
                }
            },
            (errorInfo) =>
            {
                Debug.LogErrorFormat("<><WorldMapUtils.GetAllMissionInfo>Error: {0}", errorInfo.ErrorInfo);
                if (errCallBack != null) errCallBack(errorInfo.ErrorInfo);
            });
        }

        public void GetSameStarFriendsInfo(Action<SameStarFriendsInfo> callBack = null, Action<string> errCallBack = null)
        {
            string url = UrlProvider.GetSameStarFriendsInfo(this.LocalChildInfoAgent.getChildSN(), CupBuild.getCupSn());

            NativeOkHttpMethodWrapper.get(url, "", (result) =>
            {
                SameStarFriendsInfo sameStarFriendsInfo = this.JsonUtils.String2Json<SameStarFriendsInfo>(result);
                if (callBack != null) callBack(sameStarFriendsInfo);
            },
            (errorInfo) =>
            {
                if (errCallBack != null) errCallBack(errorInfo.ErrorInfo);
                Debug.LogErrorFormat("<><WorldMapUtils.GetSameStarFriendsInfo>Error: {0}", errorInfo.ErrorInfo);
            });
        }
    }
}