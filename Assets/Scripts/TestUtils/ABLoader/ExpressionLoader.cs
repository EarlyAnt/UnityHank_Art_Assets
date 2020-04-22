using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ABLoader
{
    public class ExpressionLoader : BaseLoader
    {
        [SerializeField]
        private string objectName;
        [SerializeField]
        private bool SetParent = false;

        [ContextMenu("0-创建物体")]
        public override void LoadAB()
        {
            this.assetBundle = this.GetAssetBundle(this.assetBundleName);
            Object obj = this.assetBundle.LoadAsset(this.objectName);
            this.gameObject = GameObject.Instantiate(obj) as GameObject;
            this.gameObject.name = this.objectName;
            if (this.SetParent)
            {
                this.gameObject.transform.SetParent(this.transform, false);
                this.gameObject.transform.localEulerAngles = Vector3.zero;
            }
            else
            {
                this.gameObject.transform.position = this.transform.position;
                this.gameObject.transform.rotation = Quaternion.identity;
            }
            this.gameObject.AddComponent<AnimationMemoryManager>();
            Debug.LogFormat("<><ExpressionLoader.LoadAB>assetbundle: {0}, object: {1}", this.assetBundleName, this.objectName);
            base.LoadAB();
        }
    }
}
