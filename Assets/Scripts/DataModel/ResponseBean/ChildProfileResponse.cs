using System.Collections.Generic;

namespace Gululu
{
    public class UrlInfo{
        public string url;
        public int size;
        public string filename;
        public string crc;

        public int file_size;
    }
    public class Profile{
        public string updated_time;
        public List<UrlInfo> files;
    }
    public class ChildProfileResponse
    {
        public string status_code;

        public string status;

        public string x_child_sn;

        public List<string> files;

        public Profile profile;
    }
}