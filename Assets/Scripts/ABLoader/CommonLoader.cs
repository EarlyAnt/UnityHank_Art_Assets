using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chapter02
{
    public class CommonLoader : BaseLoader
    {
        public override void LoadNextAB()
        {
            AssetBundle assetBundle = AssetBundle.LoadFromFile(string.Format("{0}/Model/role/{1}", Application.persistentDataPath, this.assetBundleName));
            Debug.LogFormat("<><CommonLoader.LoadAB>asset bundle name: {0}", this.assetBundleName);
            base.LoadNextAB();
        }
    }
}
