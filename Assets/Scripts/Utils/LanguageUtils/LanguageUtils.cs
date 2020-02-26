using Hank;
using UnityEngine;

namespace Gululu
{

    public class LanguageUtils : ILanguageUtils
    {
        [Inject]
        public IGameDataHelper gameDataHelper { get; set; }

        GululuLanguage gululuLanguage = GululuLanguage.ZH_CN;

        private readonly string AssetsLanguageRootPath = "Language/";

        readonly string languageKey = "LanguageKey";
        public void ReadLanguage()
        {
            gululuLanguage = (GululuLanguage)gameDataHelper.GetObject<int>(languageKey,2);
        }

        public string GetLanguagePath(GululuLanguage lang) {
            return AssetsLanguageRootPath +LangConvertFileName(lang);
        }

        public string LangConvertFileName(GululuLanguage language){
            string langPath = "lang_en/";
            
            switch(language){
                case GululuLanguage.ENGLISH:
                    langPath = "lang_en/";
                    break;
                case GululuLanguage.ZH_CN:
                    langPath = "lang_zh_CN/";
                    break;
                case GululuLanguage.ZH_TW:
                    langPath = "lang_zh_TW/";
                    break;
//                 case GululuLanguage.ZH_HK:
//                     langPath = "lang_zh_HK/";
//                     break;
                default:
                    langPath = "lang_en/";
                    break;
            }

            return langPath+"lang";
        }

        public string GetLangeuageKey(GululuLanguage language){
            string key = "en";
            
            switch(language){
                case GululuLanguage.ENGLISH:
                    key = "en";
                    break;
                case GululuLanguage.ZH_CN:
                    key = "zh_cn";
                    break;
                case GululuLanguage.ZH_TW:
                    key = "zh_tw";
                    break;
//                 case GululuLanguage.ZH_HK:
//                     key = "zh_hk";
//                     break;
                default:
                    key = "en";
                    break;
            }

            return key;
        }

        public void setLanguageByAgentKey(string key)
        {
            switch(key)
            {
                case "zh-Hans":
                    {
                        gululuLanguage = GululuLanguage.ZH_CN;
                    }
                    break;
                case "zh-Hant":
                    {
                        gululuLanguage = GululuLanguage.ZH_TW;
                    }
                    break;
                default:
                    {
                        gululuLanguage = GululuLanguage.ENGLISH;
                    }
                    break;
            }
            gameDataHelper.SaveObject<int>(languageKey, (int)gululuLanguage);
        }

        public string getLanguageToAgentKey()
        {
//             GululuLanguage gululuLanguage = getCurrentLanguage();
            string key = "en";
            switch (gululuLanguage)
            {
                case GululuLanguage.ENGLISH:
                    key = "en";
                    break;
                case GululuLanguage.ZH_CN:
                    key = "zh-Hans";
                    break;
                case GululuLanguage.ZH_TW:
                    key = "zh-Hant";
                    break;
                default:
                    key = "en";
                    break;
            }
            return key;
        }

        public bool checkIsHans()
        {
//             GululuLanguage gululuLanguage = getCurrentLanguage();
            if(gululuLanguage == GululuLanguage.ZH_CN)
            {
                return true;
            }else{
                return false;
            }
        }

        public bool checkIsEN()
        {
//             GululuLanguage gululuLanguage = getCurrentLanguage();
            if (gululuLanguage == GululuLanguage.ENGLISH)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public GululuLanguage getCurrentLanguage(){
#if UNITY_EDITOR
            return GululuLanguage.ZH_CN;
#else
            return gululuLanguage;
//             GululuLanguage language;
// 
//             switch (Application.systemLanguage){
//                 case SystemLanguage.ChineseSimplified:
//                     language = GululuLanguage.ZH_CN;
//                     break;
//                 case SystemLanguage.ChineseTraditional:
//                     language = GululuLanguage.ZH_TW;
//                     break;
//                 default:
//                     language = GululuLanguage.ENGLISH;
//                     break;
//             }
// 
//             return language;
#endif
        }

    }
}