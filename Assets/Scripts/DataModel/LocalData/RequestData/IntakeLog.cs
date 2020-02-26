using System.Collections.Generic;

namespace Gululu.LocalData.RequstBody
{
    public class IntakeLogs
    {
        public List <LogItem > logs ;
    }


    public class LogItem
    {
        public LogItem(string TY,int VL,long TS ){
            this.TY = TY;
            this.VL = VL;
            this.TS = TS;
        }
        public string TY ;

        public int VL ;

        public long TS ;
    }
}