using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chapter02
{
    public class PetLoader : BaseLoader
    {
        [SerializeField]
        private string objectName;

        [ContextMenu("0-创建物体")]
        public override void LoadNextAB()
        {
            this.assetBundle = this.GetAssetBundle(this.assetBundleName);
            Object obj = this.assetBundle.LoadAsset(this.objectName);
            this.gameObject = GameObject.Instantiate(obj) as GameObject;
            this.gameObject.name = this.objectName;
            this.gameObject.transform.position = this.transform.position;
            this.gameObject.AddComponent<FixShader>();
            Debug.LogFormat("<><PetLoader.LoadAB>asset bundle name: {0}, pet name: {1}", this.assetBundleName, this.objectName);
            base.LoadNextAB();
        }
    }
}
