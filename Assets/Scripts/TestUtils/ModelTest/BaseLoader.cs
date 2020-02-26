using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ModelTest
{
    /// <summary>
    /// 模型加载基类
    /// </summary>
    public abstract class BaseLoader : MonoBehaviour
    {
        /************************************************属性与变量命名************************************************/
        protected static AssetBundle manifestAssetBundle;        
        protected static Dictionary<string, AssetBundle> assetBundles = new Dictionary<string, AssetBundle>();
        protected StringBuilder strContent = new StringBuilder();
        /************************************************Unity方法与事件***********************************************/
        protected virtual void Start()
        {
        }
        /************************************************自 定 义 方 法************************************************/
        protected void LoadManifest()
        {
            if (manifestAssetBundle == null)
            {
                manifestAssetBundle = AssetBundle.LoadFromFile(string.Format("{0}/Model/Android", Application.persistentDataPath));
            }
        }
        protected AssetBundle GetAssetBundle(string assetBundleName)
        {
            if (assetBundles.ContainsKey(assetBundleName) && assetBundles[assetBundleName] == null)
                assetBundles.Remove(assetBundleName);

            if (!assetBundles.ContainsKey(assetBundleName))
            {
                AssetBundle assetBundle = AssetBundle.LoadFromFile(string.Format("{0}/Model/{1}", Application.persistentDataPath, assetBundleName));
                assetBundles.Add(assetBundleName, assetBundle);

                if (manifestAssetBundle != null)
                {
                    AssetBundleManifest manifest = manifestAssetBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
                    if (manifest != null)
                    {
                        string[] dependencies = manifest.GetAllDependencies(assetBundleName);
                        foreach (string dependency in dependencies)
                        {
                            if (!assetBundles.ContainsKey(dependency))
                            {
                                AssetBundle dependencyAssetBundle = AssetBundle.LoadFromFile(string.Format("{0}/Model/{1}", Application.persistentDataPath, dependency));
                                assetBundles.Add(dependency, dependencyAssetBundle);
                            }
                        }
                    }
                }
            }
            this.PrintAssetBundles();
            return assetBundles[assetBundleName];
        }
        protected void PrintAssetBundles()
        {
            this.strContent.Remove(0, this.strContent.Length);
            if (assetBundles != null && assetBundles.Count > 0)
            {
                Dictionary<string, AssetBundle> buffer = assetBundles.OrderBy(p => p.Key).ToDictionary(p => p.Key, q => q.Value);
                foreach (var assetBundle in buffer)
                {
                    this.strContent.AppendFormat("AssetBundle: {0}\n", assetBundle.Key);
                }
            }
            Debug.LogFormat("<><BaseLoader.PrintAssetBundles>\n{0}", this.strContent.ToString());
        }
    }
}
