using System.Collections.Generic;

namespace Hank.Api
{
    public class AllMissionInfo
    {
        public List<MissionInfo> Items { get; set; }
        public AllMissionInfo()
        {
            this.Items = new List<MissionInfo>();
        }
    }

    public class MissionInfo
    {
        public string StarID { get; set; }
        public string MissionID { get; set; }
    }

    public class SameStarFriendsInfo
    {
        public List<PalInfo> Items { get; set; }
    }

    public class PalInfo
    {
        public string ChildSN { get; set; }
        public string NickName { get; set; }
        public string AvatarID { get; set; }
        public string MissionID { get; set; }
    }
}