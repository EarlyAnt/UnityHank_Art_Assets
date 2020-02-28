using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ModelTest
{
    /// <summary>
    /// 小宠物加载
    /// </summary>
    public class PetLoader : BaseLoader
    {
        /************************************************属性与变量命名************************************************/
        #region 小宠物相关变量
        [SerializeField]
        private Transform petRoot;
        [SerializeField]
        private List<PetInfo> petInfos;
        private PetInfo petInfo;
        private GameObject petObject;        
        public PetPlayer PetPlayer
        {
            get { return this.petObject != null ? this.petObject.GetComponent<PetPlayer>() : null; }
        }
        #endregion
        #region 配饰相关变量
        [SerializeField]
        private DressInfo accessoryButtons;
        [SerializeField]
        private DressInfo suitButtons;
        private Dictionary<AccessoryButton, GameObject> accessories = new Dictionary<AccessoryButton, GameObject>();
        #endregion
        /************************************************Unity方法与事件***********************************************/
        private void Awake()
        {
            this.LoadManifest();
        }
        protected override void Start()
        {
            this.LoadPet(this.petInfos.Find(t => t.Default));
        }
        /************************************************自 定 义 方 法************************************************/
        public void LoadPet(string petName)
        {
            this.LoadPet(this.petInfos.Find(t => t.PetName == petName));
        }
        public void PreviousPet()
        {
            if (this.petInfo == null)
            {
                this.LoadPet(this.petInfos[0]);
                Debug.LogFormat("<><PetLoader.PreviousPet>Pet: {0}", this.petInfos[0].PetName);
            }
            else
            {
                int index = this.petInfos.IndexOf(this.petInfo);
                if (index > 0)
                {
                    index -= 1;
                    this.LoadPet(this.petInfos[index]);
                    Debug.LogFormat("<><PetLoader.PreviousPet>Pet: {0}", this.petInfos[index].PetName);
                }
            }
        }
        public void NextPet()
        {
            if (this.petInfo == null)
            {
                this.LoadPet(this.petInfos[0]);
                Debug.LogFormat("<><PetLoader.NextPet>Pet: {0}", this.petInfos[0].PetName);
            }
            else
            {
                int index = this.petInfos.IndexOf(this.petInfo);
                if (index + 1 < this.petInfos.Count)
                {
                    index += 1;
                    this.LoadPet(this.petInfos[index]);
                    Debug.LogFormat("<><PetLoader.NextPet>Pet: {0}", this.petInfos[index].PetName);
                }
            }
        }
        private void LoadPet(PetInfo petInfo)
        {
            if (petInfo == null)
            {
                Debug.LogError("<><PetLoader.LoadPet>Parameter 'petInfo' is null");
                return;
            }
            this.petInfo = petInfo;
            this.DestroyModel();

            if (petInfo.IsLocal)
            {
                UnityEngine.Object prefab = Resources.Load(string.Format("Role/{0}", petInfo.Prefab));
                this.petObject = GameObject.Instantiate(prefab) as GameObject;
                petObject.transform.SetParent(this.petRoot, false);
                PetPlayer petPlayer = petObject.AddComponent<PetPlayer>();
                petPlayer.SetPetName(petInfo.AnimationNamePrefix);
                Debug.LogFormat("<><PetLoader.LoadPet>pet name: {0}", petInfo.PetName);
            }
            else
            {
                AssetBundle assetBundle = this.GetAssetBundle(petInfo.AB);
                UnityEngine.Object obj = assetBundle.LoadAsset(petInfo.Prefab);
                this.petObject = GameObject.Instantiate(obj) as GameObject;
                this.petObject.name = string.Format("{0}(Clone)", petInfo.PetName);
                this.petObject.transform.SetParent(this.petRoot, false);
                this.petObject.AddComponent<FixShader>();
                PetPlayer petPlayer = petObject.AddComponent<PetPlayer>();
                petPlayer.SetPetName(petInfo.AnimationNamePrefix);
                Debug.LogFormat("<><PetLoader.LoadPet>asset bundle name: {0}, pet name: {1}", petInfo.AB, petInfo.PetName);
            }
        }
        private void DestroyModel()
        {
            GameObject.Destroy(this.petObject);
            if (this.petInfo != null)
            {
                if (!this.petInfo.IsLocal && assetBundles.ContainsKey(this.petInfo.AB))
                {
                    if (assetBundles[this.petInfo.AB] != null)
                        assetBundles[this.petInfo.AB].Unload(true);
                    assetBundles.Remove(this.petInfo.AB);
                    Resources.UnloadUnusedAssets();
                    Debug.LogFormat("<><PetLoader.DestroyModel>asset bundle name: {0}, pet name: {1}", petInfo.AB, petInfo.PetName);
                }
            }
        }

        public void PreviousAccessory(Operations operation)
        {
            switch (operation)
            {
                case Operations.Accessory:
                    this.accessoryButtons.PreviousAccessory();
                    Debug.LogFormat("<><PetLoader.PreviousAccessory>Accessory: {0}", this.accessoryButtons.CurrentAccessory.Prefab);
                    break;
                case Operations.Suit:
                    this.suitButtons.PreviousAccessory();
                    Debug.LogFormat("<><PetLoader.PreviousAccessory>Suit: {0}", this.suitButtons.CurrentAccessory.Prefab);
                    break;
            }
        }
        public void NextAccessory(Operations operation)
        {
            switch (operation)
            {
                case Operations.Accessory:
                    this.accessoryButtons.NextAccessory();
                    Debug.LogFormat("<><PetLoader.NextAccessory>Accessory: {0}", this.accessoryButtons.CurrentAccessory.Prefab);
                    break;
                case Operations.Suit:
                    this.suitButtons.NextAccessory();
                    Debug.LogFormat("<><PetLoader.NextAccessory>Suit: {0}", this.suitButtons.CurrentAccessory.Prefab);
                    break;
            }
        }
        public void SetupAccessory(Operations operation)
        {
            switch (operation)
            {
                case Operations.Accessory:
                    this.SetupAccessory(this.accessoryButtons.CurrentAccessory);
                    break;
                case Operations.Suit:
                    this.SetupAccessory(this.suitButtons.CurrentAccessory);
                    break;
            }
        }
        public void SetupAccessory(AccessoryButton accessoryButton)
        {
            if (this.petObject == null)
            {
                Debug.LogError("<><PetLoader.SetupPetAccessory>Parameter 'petRoot' is null");
                return;
            }
            else if (accessoryButton == null)
            {
                Debug.LogError("<><PetLoader.SetupPetAccessory>Parameter 'accessoryButton' is null");
                return;
            }

            if (accessoryButton.CanWear)
            {//只有配饰和套装能穿在身上
                if (!this.accessories.ContainsKey(accessoryButton))
                {
                    foreach (KeyValuePair<AccessoryButton, GameObject> kvp in this.accessories.ToArray())
                    {//脱掉同位置的配饰
                        if (kvp.Key.Region == accessoryButton.Region)
                            this.TakeOff(kvp.Key);
                    }

                    if (accessoryButton.Region == Regions.Dummy_head || accessoryButton.Region == Regions.Dummy_wing)
                    {//穿帽子或翅膀，脱掉套装
                        foreach (KeyValuePair<AccessoryButton, GameObject> kvp in this.accessories.ToArray())
                        {
                            if (kvp.Key.Region == Regions.Dummy_taozhuang)
                                this.TakeOff(kvp.Key);
                        }
                    }
                    else if (accessoryButton.Region == Regions.Dummy_taozhuang)
                    {//穿套装，脱掉帽子和翅膀
                        foreach (KeyValuePair<AccessoryButton, GameObject> kvp in this.accessories.ToArray())
                        {
                            if (kvp.Key.Region == Regions.Dummy_head || kvp.Key.Region == Regions.Dummy_wing)
                                this.TakeOff(kvp.Key);
                        }
                    }

                    this.PutOn(accessoryButton);
                }
                else
                {
                    this.TakeOff(accessoryButton);
                }
            }
        }
        private void PlayAccessoryAnimation(Animator animator)
        {
            if (animator != null && animator.runtimeAnimatorController != null &&
                animator.runtimeAnimatorController.animationClips != null)
            {
                string prefix = "";
                switch (this.petInfo.PetName.ToUpper())
                {
                    case Roles.Purpie:
                        prefix = "pur";
                        break;
                    case Roles.Donny:
                        prefix = "dony";
                        break;
                    case Roles.Ninji:
                        prefix = "nin";
                        break;
                    case Roles.Sansa:
                        prefix = "san";
                        break;
                    case Roles.Yoyo:
                        prefix = "yoyo";
                        break;
                    case Roles.Nuo:
                        prefix = "nuo";
                        break;
                }


                for (int i = 0; i < animator.runtimeAnimatorController.animationClips.Length; i++)
                {
                    if (animator.runtimeAnimatorController.animationClips[i].name.Trim().StartsWith(prefix))
                    {
                        animator.Play(animator.runtimeAnimatorController.animationClips[i].name);
                        return;
                    }
                }
            }
        }
        private void PutOn(AccessoryButton accessoryButton)
        {
            this.TakeOff(accessoryButton);

            Transform bodyPart = UnityHelper.FindDeepChild(this.petObject.transform, accessoryButton.Region.ToString("G"));
            if (bodyPart == null) bodyPart = this.petObject.transform;
            AssetBundle assetBundle = this.GetAssetBundle(accessoryButton.AB);
            UnityEngine.Object obj = assetBundle.LoadAsset(accessoryButton.Prefab);
            GameObject accessoryObject = GameObject.Instantiate(obj) as GameObject;
            accessoryObject.name = string.Format("{0}(Clone)", accessoryButton.name);
            accessoryObject.transform.SetParent(bodyPart);
            accessoryObject.transform.localPosition = Vector3.zero;
            accessoryObject.transform.localRotation = Quaternion.identity;
            accessoryObject.transform.localScale = Vector3.one;
            accessoryObject.AddComponent<FixShader>();
            this.PlayAccessoryAnimation(accessoryObject.GetComponent<Animator>());
            this.accessories.Add(accessoryButton, accessoryObject);
            Debug.LogFormat("<><PetLoader.PutOn>{0}", accessoryButton.Prefab);
        }
        private void TakeOff(AccessoryButton accessoryButton)
        {
            if (this.accessories.ContainsKey(accessoryButton))
            {
                if (this.accessories[accessoryButton] != null)
                    GameObject.Destroy(this.accessories[accessoryButton]);
                this.accessories.Remove(accessoryButton);
            }

            if (assetBundles.ContainsKey(accessoryButton.AB) && assetBundles[accessoryButton.AB] != null)
                assetBundles[accessoryButton.AB].Unload(true);

            Debug.LogFormat("<><PetLoader.TakeOff>{0}", accessoryButton.Prefab);
        }
        [ContextMenu("搜集配饰")]
        private void CollectAccessories()
        {
            this.accessoryButtons.CollectButton();
            this.suitButtons.CollectButton();
        }
    }

    [Serializable]
    public class PetInfo
    {
        public string PetName;
        public string AnimationNamePrefix;
        public string Prefab;
        public bool IsLocal;
        public string AB;
        public bool Default;
    }
}
