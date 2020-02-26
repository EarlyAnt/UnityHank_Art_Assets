using Cup.Utils.android;
using UnityEngine;

namespace Gululu
{
    public class GululuNetworkHelper
    {
        [Inject]
        public ILanguageUtils languageUtils { get; set; }
        public string GetAgent(){
            return "GululuCupHank/" + CupBuild.getAppVersionName() + ";" + "Unity/" + SystemInfo.operatingSystem + ";" + SystemInfo.deviceName + "/" + SystemInfo.deviceModel;
        }

        public string GetUdid(){
            return SystemInfo.deviceUniqueIdentifier;
        }

        public string GetAcceptLang(){
            return languageUtils.getLanguageToAgentKey();
        }

    }
}