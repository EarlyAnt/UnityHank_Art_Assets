using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ModelTest
{
    /// <summary>
    /// 小宠物加载
    /// </summary>
    public class PetLoader : MonoBehaviour
    {
        /************************************************属性与变量命名************************************************/
        [SerializeField]
        private Transform petRoot;
        [SerializeField]
        private List<PetInfo> petInfos;
        private PetInfo petInfo;
        private AssetBundle assetBundle;
        private GameObject petObject;
        private AssetBundle manifestAssetBundle;
        private StringBuilder strContent = new StringBuilder();
        private Dictionary<string, AssetBundle> assetBundles = new Dictionary<string, AssetBundle>();
        /************************************************Unity方法与事件***********************************************/
        private void Start()
        {
            this.LoadPet(this.petInfos.Find(t => t.Default));
        }
        /************************************************自 定 义 方 法************************************************/
        public void LoadPet(string petName)
        {
            this.LoadManifest();
            this.LoadPet(this.petInfos.Find(t => t.PetName == petName));
        }
        private void LoadPet(PetInfo petInfo)
        {
            if (petInfo == null)
            {
                Debug.LogError("<><PetLoader.LoadPet>Parameter 'petInfo' is null");
                return;
            }
            this.DestroyModel();

            if (petInfo.IsLocal)
            {
                UnityEngine.Object prefab = Resources.Load(string.Format("Role/{0}", petInfo.Prefab));
                this.petObject = GameObject.Instantiate(prefab) as GameObject;
                petObject.transform.SetParent(this.petRoot, false);
                PetPlayer petPlayer = petObject.AddComponent<PetPlayer>();
                petPlayer.SetPetName(petInfo.PetName);
                Debug.LogFormat("<><PetLoader.LoadPet>pet name: {0}", petInfo.PetName);
            }
            else
            {
                this.assetBundle = this.GetAssetBundle(petInfo.AB);
                UnityEngine.Object obj = this.assetBundle.LoadAsset(petInfo.Prefab);
                this.petObject = GameObject.Instantiate(obj) as GameObject;
                this.petObject.name = string.Format("{0}(Clone)", petInfo.PetName);
                this.petObject.transform.SetParent(this.petRoot, false);
                this.petObject.AddComponent<FixShader>();
                Debug.LogFormat("<><PetLoader.LoadPet>asset bundle name: {0}, pet name: {1}", petInfo.AB, petInfo.PetName);
            }
        }
        private void DestroyModel()
        {
            GameObject.Destroy(this.petObject);
            if (this.petInfo != null)
            {
                if (!this.petInfo.IsLocal && this.assetBundles.ContainsKey(this.petInfo.AB))
                {
                    if (this.assetBundles[this.petInfo.AB] != null)
                        this.assetBundles[this.petInfo.AB].Unload(true);
                    this.assetBundles.Remove(this.petInfo.AB);
                    Resources.UnloadUnusedAssets();
                    Debug.LogFormat("<><PetLoader.DestroyModel>asset bundle name: {0}, pet name: {1}", petInfo.AB, petInfo.PetName);
                }
            }
        }
        private void LoadManifest()
        {
            if (this.manifestAssetBundle == null)
            {
                this.manifestAssetBundle = AssetBundle.LoadFromFile(string.Format("{0}/Model/Android", Application.persistentDataPath));
            }
        }
        private AssetBundle GetAssetBundle(string assetBundleName)
        {
            if (this.assetBundles.ContainsKey(assetBundleName) && this.assetBundles[assetBundleName] == null)
                this.assetBundles.Remove(assetBundleName);

            if (!this.assetBundles.ContainsKey(assetBundleName))
            {
                AssetBundle assetBundle = AssetBundle.LoadFromFile(string.Format("{0}/Model/{1}", Application.persistentDataPath, assetBundleName));
                this.assetBundles.Add(assetBundleName, assetBundle);

                if (manifestAssetBundle != null)
                {
                    AssetBundleManifest manifest = manifestAssetBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
                    if (manifest != null)
                    {
                        string[] dependencies = manifest.GetAllDependencies(assetBundleName);
                        foreach (string dependency in dependencies)
                        {
                            if (!this.assetBundles.ContainsKey(dependency))
                            {
                                AssetBundle dependencyAssetBundle = AssetBundle.LoadFromFile(string.Format("{0}/Model/{1}", Application.persistentDataPath, dependency));
                                this.assetBundles.Add(dependency, dependencyAssetBundle);
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
            Debug.LogFormat("<><PetLoader.PrintAssetBundles>\n{0}", this.strContent.ToString());
        }
    }

    [Serializable]
    public class PetInfo
    {
        public string PetName;
        public string Prefab;
        public bool IsLocal;
        public string AB;
        public bool Default;
    }
}
