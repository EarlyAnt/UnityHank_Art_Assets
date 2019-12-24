using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chapter02
{
    public abstract class BaseLoader : MonoBehaviour
    {
        [SerializeField]
        protected string assetBundleName;
        [SerializeField]
        private bool autoLoad;
        [SerializeField]
        private BaseLoader next;
        protected AssetBundle assetBundle;
        protected GameObject gameObject;
        private static Dictionary<string, AssetBundle> assetBundles = new Dictionary<string, AssetBundle>();

        protected virtual void Start()
        {
            if (this.autoLoad)
            {
                this.LoadNextAB();
            }
        }

        public virtual void LoadNextAB()
        {
            if (this.next != null)
                this.next.LoadNextAB();
        }

        protected AssetBundle GetAssetBundle(string assetBundleName)
        {
            if (assetBundles.ContainsKey(assetBundleName) && assetBundles[assetBundleName] == null)
                assetBundles.Remove(assetBundleName);

            if (!assetBundles.ContainsKey(assetBundleName))
            {
                AssetBundle assetBundle = AssetBundle.LoadFromFile(string.Format("{0}/Model/role/{1}", Application.persistentDataPath, this.assetBundleName));
                assetBundles.Add(assetBundleName, assetBundle);
            }
            return assetBundles[assetBundleName];
        }

        [ContextMenu("1-卸载AB包中未使用的资源")]
        protected void UnloadAssetBundle1()
        {
            if (this.assetBundle != null)
                this.assetBundle.Unload(false);
        }

        [ContextMenu("2-卸载AB包中的所有资源")]
        protected void UnloadAssetBundle2()
        {
            if (this.assetBundle != null)
                this.assetBundle.Unload(true);
        }

        [ContextMenu("3-销毁创建的物体")]
        protected void DestroyObject()
        {
            if (this.gameObject != null)
                GameObject.Destroy(this.gameObject);
        }
    }
}