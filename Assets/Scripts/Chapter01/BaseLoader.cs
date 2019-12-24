using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chapter01
{
    public abstract class BaseLoader : MonoBehaviour
    {
        [SerializeField]
        protected string assetBundleName;
        [SerializeField]
        private bool autoLoad;
        [SerializeField]
        private BaseLoader next;
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
            if (!assetBundles.ContainsKey(assetBundleName))
            {
                AssetBundle assetBundle = AssetBundle.LoadFromFile(string.Format("{0}/Model/role/{1}", Application.persistentDataPath, this.assetBundleName));
                assetBundles.Add(assetBundleName, assetBundle);
            }
            return assetBundles[assetBundleName];
        }
    }
}