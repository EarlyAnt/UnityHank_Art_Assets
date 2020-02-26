using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;
using System.Linq;
using UnityEngine.Assertions;

namespace Gululu
{
    public enum GululuLanguage{
        ENGLISH = 1, 
        ZH_CN, 
        ZH_TW,
    }
    public interface ILangManager
    {
        string GetLanguageString(string key);
    }
}