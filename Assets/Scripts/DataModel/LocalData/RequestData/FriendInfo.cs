using System.Collections.Generic;

namespace Gululu.LocalData.RequstBody
{
    public class FriendInfo{
        public List<FriendInfoItem> logs;
    }

    public class FriendInfoItem{

        public FriendInfoItem(string CN, long TS, string TY = "TYIT"){
            this.CN = CN;
            this.TS = TS;
            this.TY = TY;
        }
        public string CN;
        public long TS;
        public string TY= "TYIT";
    }

}