using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gululu.Util;
using System.Security;
using Mono.Xml;
using System;

namespace Gululu.Config
{
    public class AniAndSound
    {
        public string AniName;
        public string SoundType;
    }
    public interface IActionConfig 
    {
        void LoadActionConfig();
        AniAndSound GetAniAndSound(string petType, string actionType);
        bool IsLoadedOK();

    }
}
