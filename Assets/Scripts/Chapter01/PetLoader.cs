using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chapter01
{
    public class PetLoader : BaseLoader
    {
        [SerializeField]
        private string objectName;

        public override void LoadNextAB()
        {
            AssetBundle assetBundle = this.GetAssetBundle(this.assetBundleName);
            Object obj = assetBundle.LoadAsset(this.objectName);
            GameObject gameObject = GameObject.Instantiate(obj) as GameObject;
            gameObject.name = this.objectName;
            gameObject.transform.position = this.transform.position;
            gameObject.AddComponent<FixShader>();
            Debug.LogFormat("<><PetLoader.LoadAB>asset bundle name: {0}, pet name: {1}", this.assetBundleName, this.objectName);
            base.LoadNextAB();
        }
    }
}
