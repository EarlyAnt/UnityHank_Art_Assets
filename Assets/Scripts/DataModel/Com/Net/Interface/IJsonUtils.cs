using System.Collections.Generic;
using Gululu.LocalData.Agents;
using LitJson;

namespace Gululu
{
    public interface IJsonUtils
    {
        ILocalCupAgent mLocalCupAgent{get;set;}
        string DicToJsonStr(IDictionary<string, string> bodyContent);

        T String2Json<T>(string jsonStr);

        string Json2String(object json);

        JsonData JsonStr2JsonData(string josnStr);

        string getToken();
    }
}