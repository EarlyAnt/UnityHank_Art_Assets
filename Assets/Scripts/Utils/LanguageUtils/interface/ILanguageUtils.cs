using UnityEngine;

namespace Gululu
{

    public interface ILanguageUtils
    {
        void ReadLanguage();

        string GetLanguagePath(GululuLanguage lang);

        string LangConvertFileName(GululuLanguage language);

        string GetLangeuageKey(GululuLanguage language);

        void setLanguageByAgentKey(string key);

        string getLanguageToAgentKey();

        bool checkIsHans();

        bool checkIsEN();

        GululuLanguage getCurrentLanguage();

    }
}