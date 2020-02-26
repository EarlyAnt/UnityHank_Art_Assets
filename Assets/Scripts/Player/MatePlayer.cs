using Gululu.Config;
using Hank.MainScene;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hank
{
    public class MatePlayer : BasePlayer
    {
        public enum Locations { TopLeft = 0, BottomRight = 1 }
        [Inject]
        public ChangeRoleSignal ChangeRoleSignal { get; set; }
        [SerializeField]
        private Locations location;
        [SerializeField]
        protected string MateName;
        [SerializeField, Range(0f, 60f)]
        protected float IntervalMin = 10.0f;
        [SerializeField, Range(0f, 60f)]
        protected float IntervalMax = 30.0f;
        private bool stopAnimation;
        public Locations Location
        {
            get { return this.location; }
        }

        protected override void Start()
        {
            base.Start();
        }
        protected override void OnDestroy()
        {
            this.ChangeRoleSignal.RemoveListener(this.RefreshRole);
            this.stopAnimation = true;
            this.StopAllCoroutines();
            base.OnDestroy();
        }
        protected override void Initialize()
        {
            if (this.createDefaultRole)
            {
                this.LocalPetAccessoryAgent.LoadData();
                this.RefreshRole();
            }
            this.ChangeRoleSignal.AddListener(this.RefreshRole);
        }
        public void RefreshRole()
        {
            DestoryModel();
            List<string> petAccessories = this.LocalPetAccessoryAgent.GetPetAccessories(this.PlayerDataManager.CurrentPet);
            for (int i = 0, accessoryID = 0; i < petAccessories.Count; i++)
            {
                if (!int.TryParse(petAccessories[i], out accessoryID))
                {
                    Debug.LogErrorFormat("<><MatePlayer.RefreshRole>AccessoryID [{0}] is invalid", petAccessories[i]);
                    continue;
                }

                Accessory accessory = this.AccessoryConfig.GetAccessory(accessoryID);
                if (accessory != null && this.IsNative(accessory.Region))
                {
                    this.RefreshRole(accessory);
                }
            }
        }
        public void RefreshRole(Accessory accessory)
        {
            this.LoadPrefab(accessory);
            this.StopAllCoroutines();
            this.StartCoroutine(this.RandomAnimation());
        }
        private void LoadPrefab(Accessory accessory)
        {
            if (this.modelPath != accessory.Prefab || this.objModel == null)
            {
                DestoryModel();
                this.modelPath = accessory.Prefab;

                this.AssetBundleUtils.LoadAssetAsync(accessory.AB, accessory.Prefab, (gameObject) =>
                {
                    this.DestroyRedundantModel();//删除同位置下多余的的小精灵(避免频繁切换时，同一个位置出现多个小精灵的情况)
                    this.assetBundleName = accessory.AB;
                    this.objModel = gameObject;
                    if (this.objModel != null)
                    {
                        this.objModel.transform.SetParent(this.roleRoot, false);
                        this.objModel.tag = this.GetTag();
                        this.objModel.AddComponent<FixShader>();
                        this.objModel.AddComponent<MatePlayerTag>().Location = this.Location;
                        this.objModel.SetActive(true);
                        this.animator = objModel.GetComponent<Animator>();
                        this.LocalPetAccessoryAgent.LoadData();
                    }
                },
                (errorText) =>
                {
                    Debug.LogErrorFormat("<><MatePlayer.LoadPrefab>Error: {0}", errorText);
                });
            }
        }
        public void DestoryModel(Accessory accessory)
        {
            if (accessory != null && !string.IsNullOrEmpty(accessory.AB))
                this.AssetBundleUtils.StopLoadAsset(accessory.AB);

            base.DestoryModel();
        }
        public bool IsNative(string accessoryRegion)
        {
            return (!string.IsNullOrEmpty(accessoryRegion) && accessoryRegion.ToUpper() == this.location.ToString("G").ToUpper());
        }
        protected override string GetTag()
        {
            return "Mate";
        }
        private IEnumerator RandomAnimation()
        {
            if (this.animator == null)
                yield break;

            float time = 0;
            while (!this.stopAnimation)
            {
                time = 0;
                float interval = Random.Range(this.IntervalMin, this.IntervalMax);
                while (time < interval && !this.stopAnimation)
                {
                    time += Time.deltaTime;
                    yield return null;
                }

                if (this.animator != null && this.animator.runtimeAnimatorController != null && this.animator.runtimeAnimatorController.animationClips != null)
                {
                    int state = Random.Range(1, this.animator.runtimeAnimatorController.animationClips.Length);
                    this.animator.SetInteger("State", state);
                    this.animator.SetTrigger("Jump");
                }
            }
        }
        private void DestroyRedundantModel()
        {
            MatePlayerTag[] matePlayerTags = this.roleRoot.GetComponentsInChildren<MatePlayerTag>();
            if (matePlayerTags != null && matePlayerTags.Length > 0)
            {
                foreach (var matePlayerTag in matePlayerTags)
                {
                    if (matePlayerTag.Location == this.Location)
                        GameObject.Destroy(matePlayerTag.gameObject);
                }
            }
        }
    }
}

