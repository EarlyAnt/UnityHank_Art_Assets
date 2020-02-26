using System.Collections.Generic;

namespace Hank.Api
{
    public class PetAccessoryOrder
    {
        public List<PetAccessory> items { get; set; }
        public string remark { get; set; }
    }

    public class PetAccessory
    {
        public string sn { get; set; }
        public int count { get; set; }
    }

    public class OrderReceipt
    {
        public string status { get; set; }
        public string code_url { get; set; }
        public string order_sn { get; set; }
    }
}
