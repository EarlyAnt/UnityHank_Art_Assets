using Cup.Utils.android;
using UnityEngine;

namespace Gululu.LocalData.Agents
{
    public class NativeDataManager : INativeDataManager
    {
        public void saveToken(string token){
            GuLog.Debug("save token : "+token);
            AndroidJavaObject mainActiviry = AndroidContextHolder.GetAndroidContext();

            mainActiviry.Call<bool>("saveToken",token);
        }
    }
}