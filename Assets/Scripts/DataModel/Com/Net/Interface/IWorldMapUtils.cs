using Hank.Api;
using System;
using System.Collections.Generic;

namespace Gululu.net
{
    public class GetAllMissionInfoResponse
    {
        public string status { get; set; }
        public List<MissionData> highest_mission_data { get; set; }
        public GetAllMissionInfoResponse()
        {
            this.highest_mission_data = new List<MissionData>();
        }
    }

    public class MissionData
    {
        public string star_id { get; set; }
        public string mission_id { get; set; }
    }

    public interface IWorldMapUtils
    {
        void GetAllMissionInfo(Action<AllMissionInfo> callBack = null, Action<string> errCallBack = null);
        void GetSameStarFriendsInfo(Action<SameStarFriendsInfo> callBack = null, Action<string> errCallBack = null);
    }
}