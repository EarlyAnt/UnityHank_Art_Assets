using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gululu;
using System.IO;
using UnityEngine.UI;


namespace Gululu.Util
{
    public interface IIconManager 
    {
        void OnlyLoadSelf();
        // strName :  local image name
        Sprite GetIconByName(string strName);
    }
}
