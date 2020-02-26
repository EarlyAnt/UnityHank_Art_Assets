using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hank
{
    public interface ICachePath
    {
        string GetSpriteFullName(string fileName);
        string GetAudioFullName(string fileName);

        string GetWWWPersistentAssetsPath(string path);

        void DeleteCacheFile(string fullName);
        void WriteCacheFile(string fullName, byte[] bytes);
        bool IsExist(string fullName);
    }
}
