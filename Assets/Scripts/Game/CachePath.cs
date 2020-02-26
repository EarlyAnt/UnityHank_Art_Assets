using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Hank
{
    public class CachePath : ICachePath
    {

        string GetCachePath(string subDirctory)
        {
            string filePath = Application.persistentDataPath + subDirctory;
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            return filePath;
        }

        public string GetWWWPersistentAssetsPath(string path)
        {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
            { return "file:///" + path; }
#else
            { return "file://" + path; } 
#endif
        }


        public string GetSpriteFullName(string fileName)
        {
            return GetCachePath("/Sprite/") + fileName;
        }

        public string GetAudioFullName(string fileName)
        {
            return GetCachePath("/Audio/") + fileName;
        }

        public void DeleteCacheFile(string fullName)
        {
            if (File.Exists(fullName))
            {
                File.Delete(fullName);
            }
        }
        public void WriteCacheFile(string fullName, byte[] bytes)
        {
            File.WriteAllBytes(fullName, bytes);
        }
        public bool IsExist(string fullName)
        {
            return File.Exists(fullName);
        }

    }
}
