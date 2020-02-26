using System.Collections.Generic;

namespace Gululu
{
    public class ClocksItem
    {
        
        public int id { get; set; }
        
        public string x_child_sn { get; set; }
        
        public string clock_type { get; set; }
        
        public int clock_hour { get; set; }
        
        public int clock_min { get; set; }
        
        public string clock_weekdays { get; set; }
        
        public int clock_enable { get; set; }
    }

    public class ClocksInfo
    {
        public List <ClocksItem > clocks { get; set; }

        public ClocksInfo() { this.clocks = new List<ClocksItem>(); }
    }
}