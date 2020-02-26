using UnityEngine;
using System;
using UnityEngine.UI;
using System.Collections.Generic;
using Gululu.Config;

namespace Gululu.Util
{   
    public interface IPrefabRoot 
    {
        II18NConfig I18NConfig { get; set; }

        void ReplaceFont(GameObject newGameObject);
        GameObject GetUniqueGameObject(string strResourcePre, string name);

        GameObject GetGameObject(string strResourcePre, string name);
        T GetObjectNoInstantiate<T>(string strResourcePre, string name) where T : UnityEngine.Object;


        void persentView<T>(ref GameObject viewGameObject, Transform transform) where T : MonoBehaviour;

        void hideView(ref GameObject viewGameObject);

        void Clear();
    }
}
