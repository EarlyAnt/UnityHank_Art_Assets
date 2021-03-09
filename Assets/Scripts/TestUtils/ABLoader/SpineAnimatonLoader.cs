using Common;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ABLoader
{
    public class SpineAnimatonLoader : BaseLoader
    {
        [SerializeField]
        private string objectName;
        [SerializeField]
        private bool setParent = false;
        private const string MATERIAL_NAME = "SkeletonGraphicDefault";

        [ContextMenu("0-创建物体")]
        public override void LoadAB()
        {
            this.assetBundle = this.GetAssetBundle(this.assetBundleName);
            Object spineObject = this.assetBundle.LoadAsset(this.objectName);
            Object materialObject = this.assetBundle.LoadAsset(MATERIAL_NAME);
            Material material = materialObject as Material;
            Shader shader = Shader.Find(material.shader.name);
            material.shader = shader;
            SkeletonGraphic spine = (spineObject as GameObject).GetComponent<SkeletonGraphic>();
            spine.material = material;
            this.gameObject = GameObject.Instantiate(spineObject) as GameObject;
            this.gameObject.name = this.objectName;
            if (this.setParent)
            {
                this.gameObject.transform.SetParent(this.transform, false);
                this.gameObject.transform.localPosition = Vector3.zero;
                this.gameObject.transform.localEulerAngles = Vector3.zero;
                this.gameObject.transform.localScale = Vector3.one;
            }
            else
            {
                this.gameObject.transform.position = this.transform.position;
                this.gameObject.transform.localEulerAngles = Vector3.zero;
                this.gameObject.transform.localScale = Vector3.one;
            }
            this.gameObject.AddComponent<SpineTest>();
            Debug.LogFormat("<><SpineAnimatonLoader.LoadAB>assetbundle: {0}, object: {1}", this.assetBundleName, this.objectName);
            base.LoadAB();
        }
    }
}
