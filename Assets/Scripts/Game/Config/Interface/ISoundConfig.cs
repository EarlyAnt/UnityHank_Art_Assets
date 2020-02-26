using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gululu.Util;
using System.Security;
using Mono.Xml;
using System;

namespace Gululu.Config
{
    public class SoundClip
    {
        public string SoundPath;
        public string Storage;
        public float Rate;
    }

    public class SoundInfo
    {
        public string Type;
        public string Storage;
        public string Module;
        public List<SoundClip> sounds = new List<SoundClip>();
    }

    public interface ISoundConfig
    {
        void LoadSoundConfig();
        SoundClip GetSoundClip(string soundType);
        SoundClip GetLastSoundClip(string soundType);
        bool IsLoadedOK();
        List<SoundInfo> GetAllSoundInfos();
    }
}
