using UnityEngine;
using System.Collections;
#if UNITY_IPHONE
using UnityEngine.iOS;
#endif

namespace Gululu.Util
{
    public class ResourceLoadBase : MonoBehaviour
    {

        protected bool m_LoadOk = false;

        public bool IsLoadedOK()
        {
            return m_LoadOk;
        }

        static bool hotFix = false;

        public static string GetResourceBasePath()
        {
            if(hotFix)
            {
                return GetPersistentAssetsPath();
            }
            else
            {
                return GetSteamingAssetsPath();
            }
        }

        public static string GetSteamingAssetsPath()
        {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
            { return "file:///" + Application.streamingAssetsPath + "/"; }
#elif UNITY_IPHONE
		{ return "file://" +  Application.streamingAssetsPath + "/"; }
#elif UNITY_ANDROID
        { return  Application.streamingAssetsPath + "/"; }
#else
       return string.Empty;
#endif

        }

        public static string GetPersistentAssetsPath()
        {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
            { return "file:///" + Application.persistentDataPath + "/"; }
#else
             { return "file://" + Application.persistentDataPath + "/"; } 
#endif

        }


    }
}

