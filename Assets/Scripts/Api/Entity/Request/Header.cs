using System.Collections.Generic;

namespace Hank.Api
{
    public class Header
    {
        public List<HeaderData> headers{get;set;}
    }

    public class HeaderData{
        public string key{get;set;}
        public string value{get;set;}
    }
}