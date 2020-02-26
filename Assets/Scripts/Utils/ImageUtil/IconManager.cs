using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gululu;
using System.IO;
using UnityEngine.UI;


namespace Gululu.Util
{
    public class IconManager : IIconManager
    {
        GameObject objIconRoot;

        public void OnlyLoadSelf()
        {
            string strNewObjectHierachy = "UI/Sprites" + "/" + "SpritesList";
            objIconRoot = Resources.Load(strNewObjectHierachy) as GameObject;
            objIconRoot.SetActive(false);
        }

        // strName :  local image name
        public Sprite GetIconByName(string strName)
        {
            if(!string.IsNullOrEmpty(strName))
            {
                Transform child = objIconRoot.transform.Find(strName);
                if (child != null)
                {
                    return child.GetComponent<SpriteRenderer>().sprite;
                }
            }
            return null;
        }

    }
}
