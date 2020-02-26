using System.Collections.Generic;

namespace Hank.Api
{
    public class LocalPetsInfo
    {
        public string current_pet { get; set; }
        public List<string> unlocked_pets { get; set; }
    }

    public class RemotePetsInfo
    {
        public string status { get; set; }
        public List<string> pets { get; set; }
    }
}