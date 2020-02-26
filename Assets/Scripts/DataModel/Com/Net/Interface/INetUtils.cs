using System.Collections.Generic;

namespace Gululu
{
    public interface INetUtils
    {
        bool isNetworkEnable();

        Dictionary<string,string> transforDictionarHead(string data);
    }
}