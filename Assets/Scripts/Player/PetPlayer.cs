using Cup.Utils.Wifi;
using Gululu.Config;
using Hank.PetDress;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hank
{
    public class PetPlayer : BasePlayer
    {
        bool putOnAccessories = true;

        public virtual void RefreshRole(bool putOnAccessories = false)
        {
            this.RefreshRole(this.LocalPetInfoAgent.getCurrentPet(), putOnAccessories);
        }
        public virtual void RefreshRole(string petName, bool putOnAccessories = false)
        {
            this.putOnAccessories = putOnAccessories;
            RoleInfo roleInfo = RoleConfig.GetRoleInfo(petName);
            if (roleInfo == null)
            {
                Debug.LogErrorFormat("<><PetPlayer.LoadPrefab>Paramater 'roleInfo' is null, petType is {0}", this.PetName);
                return;
            }
            this.PetName = petName;
            this.LoadPrefab(roleInfo);

            if (this.putOnAccessories && this.objModel != null)
            {
                this.TakeOffAllAccessories();
                this.SetupPetAccessories();
            }
        }
        private void LoadPrefab(RoleInfo roleInfo)
        {
            if (this.modelPath != roleInfo.ModelPath || objModel == null)
            {
                DestoryModel();
                this.modelPath = roleInfo.ModelPath;
                if (roleInfo.IsLocal)
                {
                    this.objModel = PrefabRoot.GetGameObject("Role", this.modelPath);
                    if (this.objModel != null)
                    {
                        this.PetName = roleInfo.Name;
                        this.objModel.transform.SetParent(this.roleRoot, false);
                        this.objModel.tag = this.GetTag();
                        animator = objModel.GetComponent<Animator>();
                        this.LocalPetAccessoryAgent.LoadData();
                    }
                }
                else
                {
                    this.AssetBundleUtils.LoadAssetAsync(roleInfo.AB, roleInfo.ModelPath, (gameObject) =>
                    {
                        if (gameObject != null)
                        {
                            this.PetName = roleInfo.Name;
                            gameObject.SetActive(true);
                            gameObject.AddComponent<FixShader>();
                            this.objModel = gameObject;
                            this.objModel.transform.SetParent(this.roleRoot, false);
                            this.objModel.tag = this.GetTag();
                            animator = objModel.GetComponent<Animator>();
                            this.LocalPetAccessoryAgent.LoadData();
                        }
                        else
                        {
                            this.StopAllCoroutines();
                            this.StartCoroutine(this.CircleInspect(roleInfo));
                        }
                    },
                    (errorText) =>
                    {
                        this.StopAllCoroutines();
                        this.StartCoroutine(this.CircleInspect(roleInfo));
                        Debug.LogErrorFormat("<><PetPlayer.LoadPrefab>Error: {0}", errorText);
                    });
                }
            }
        }
        private IEnumerator CircleInspect(RoleInfo roleInfo)
        {
            if (this.objModel == null)
            {
                List<string> unlockedPets = this.LocalUnlockedPetsAgent.GetUnlockedPets();
                if (unlockedPets != null && unlockedPets.Count > 0)
                {
                    if (unlockedPets.Contains(Roles.Purpie))
                        this.RefreshRole(Roles.Purpie, this.putOnAccessories);
                    else if (unlockedPets.Contains(Roles.Ninji))
                        this.RefreshRole(Roles.Ninji, this.putOnAccessories);
                    else if (unlockedPets.Contains(Roles.Sansa))
                        this.RefreshRole(Roles.Sansa, this.putOnAccessories);
                }
            }

            string assetBundlePath = string.Format("Model/{0}.ab", roleInfo.AB);
            while (!this.ResourceUtils.FileExisted(assetBundlePath))
            {
                if (NativeWifiManager.Instance.IsConnected())
                    yield return this.StartCoroutine(this.ResourceUtils.LoadAssetBundle(assetBundlePath, (assetBundle) =>
                                                     this.ResourceUtils.UnloadAssetBundle(assetBundlePath)));
                yield return new WaitForSeconds(2f);
            }
            yield return new WaitForSeconds(2f);
            this.modelPath = "";
            this.LoadPrefab(roleInfo);
        }

        public override void Show()
        {
            base.Show();

            if (this.objModel != null && !this.destroying)
                this.SetAccessoryAnimation();
        }

        public void SetupPetAccessory(Accessory accessory)
        {
            if (this.objModel == null)
            {
                Debug.LogError("<><PetPlayer.SetupPetAccessory>Parameter 'petRoot' is null");
                return;
            }

            if (accessory != null)
            {
                if (accessory.CanWear)
                {//只有配饰和套装能穿在身上
                    UnityHelper.DeleteAllChildren(this.objModel.transform, accessory.Region);//删除此部位的配饰
                    Transform bodyPart = UnityHelper.FindDeepChild(this.objModel.transform, accessory.Region);
                    if (bodyPart == null) bodyPart = this.objModel.transform;

                    this.AssetBundleUtils.LoadAssetAsync(accessory.AB, accessory.Prefab, (gameObject) =>
                    {
                        if (gameObject != null)
                        {
                            gameObject.transform.SetParent(bodyPart);
                            gameObject.transform.localPosition = Vector3.zero;
                            gameObject.transform.localRotation = Quaternion.identity;
                            gameObject.transform.localScale = Vector3.one;
                            gameObject.AddComponent<FixShader>();
                            gameObject.SetActive(true);
                            this.PlayAccessoryAnimation(gameObject.GetComponent<Animator>());
                            Debug.LogFormat("<><PetPlayer.SetupPetAccessory>AB: {0}, Prefab: {1}, GameObject: {2}", accessory.AB, accessory.Prefab, gameObject.name);
                        }
                    },
                    (errorText) =>
                    {
                        Debug.LogErrorFormat("<><PetPlayer.SetupPetAccessory>Error: {0}", errorText);
                    });
                }
            }
            else Debug.LogErrorFormat("<><PetPlayer.SetupPetAccessory>Can't find the accessory [{0}]", accessory);
        }

        public void SetupPetAccessoriesByTypes(List<int> accessoryTypeList)
        {
            if (this.objModel == null || accessoryTypeList == null || accessoryTypeList.Count == 0)
            {
                Debug.LogError("<><PetPlayer.TakeOffPetAccessoryByTypes>objModel or accessoryTypeList is null");
                return;
            }

            List<string> petAccessories = this.LocalPetAccessoryAgent.GetPetAccessories(this.PetName);
            for (int i = 0, accessoryID = 0; i < petAccessories.Count; i++)
            {
                if (!int.TryParse(petAccessories[i], out accessoryID))
                {
                    Debug.LogErrorFormat("<><PetPlayer.SetupPetAccessories>AccessoryID [{0}] is invalid", petAccessories[i]);
                    continue;
                }

                Accessory accessory = this.AccessoryConfig.GetAccessory(accessoryID);
                if (accessory != null && accessoryTypeList.Contains(accessory.Type))
                    this.SetupPetAccessory(accessory);
            }
        }

        public void SetupPetAccessories()
        {
            if (this.objModel == null)
            {
                Debug.LogError("<><PetPlayer.SetupPetAccessories>objModel is null");
                return;
            }

            List<Accessory> petAccessories = this.LocalPetAccessoryAgent.GetAllPetAccessories(this.PetName);
            if (petAccessories != null && petAccessories.Count > 0)
            {
                foreach (var petAccessory in petAccessories)
                    this.SetupPetAccessory(petAccessory);
            }
        }

        public void TakeOffPetAccessory(Accessory accessory)
        {
            if (this.objModel == null)
            {
                Debug.LogError("<><PetPlayer.TakeOffPetAccessory>objModel is null");
                return;
            }

            if (accessory != null)
            {
                if (!string.IsNullOrEmpty(accessory.AB))
                    this.AssetBundleUtils.StopLoadAsset(accessory.AB);

                switch (accessory.AccessoryType)
                {
                    case Gululu.Config.AccessoryTypes.Head:
                        UnityHelper.DeleteAllChildren(this.objModel.transform, RolePlayer.BodyPart.HEAD);
                        break;
                    case Gululu.Config.AccessoryTypes.Back:
                        UnityHelper.DeleteAllChildren(this.objModel.transform, RolePlayer.BodyPart.WING);
                        break;
                    case Gululu.Config.AccessoryTypes.Suit:
                        UnityHelper.DeleteAllChildren(this.objModel.transform, RolePlayer.BodyPart.SUIT);
                        break;
                }
                Debug.LogFormat("<><PetPlayer.TakeOffPetAccessory>AB: {0}, Prefab: {1}", accessory.AB, accessory.Prefab);
            }
        }

        public void TakeOffPetAccessoryByTypes(List<int> accessoryTypeList)
        {
            if (this.objModel == null || accessoryTypeList == null || accessoryTypeList.Count == 0)
            {
                Debug.LogError("<><PetPlayer.TakeOffPetAccessoryByTypes>objModel or accessoryTypeList is null");
                return;
            }

            List<string> petAccessories = this.LocalPetAccessoryAgent.GetPetAccessories(this.PetName);
            for (int i = 0, accessoryID = 0; i < petAccessories.Count; i++)
            {
                if (!int.TryParse(petAccessories[i], out accessoryID))
                {
                    Debug.LogErrorFormat("<><PetPlayer.TakeOffPetAccessoryByTypes>AccessoryID [{0}] is invalid", petAccessories[i]);
                    continue;
                }

                Accessory accessory = this.AccessoryConfig.GetAccessory(accessoryID);
                if (accessory != null && accessoryTypeList.Contains(accessory.Type))
                    this.TakeOffPetAccessory(accessory);
            }
        }

        public void TakeOffAllAccessories()
        {
            if (this.objModel == null)
            {
                Debug.LogError("<><PetPlayer.TakeOffAllAccessories>Parameter 'petRoot' is null");
                return;
            }
            UnityHelper.DeleteAllChildren(this.objModel.transform, RolePlayer.BodyPart.HEAD);
            UnityHelper.DeleteAllChildren(this.objModel.transform, RolePlayer.BodyPart.WING);
            UnityHelper.DeleteAllChildren(this.objModel.transform, RolePlayer.BodyPart.SUIT);
            Debug.Log("<><PetPlayer.TakeOffAllAccessories>+ + + + + + + +");
        }

        public void SetAllAccessoriesVisible(bool visible)
        {
            UnityHelper.SetAllChildrenVisible(this.objModel.transform, RolePlayer.BodyPart.HEAD, visible);
            UnityHelper.SetAllChildrenVisible(this.objModel.transform, RolePlayer.BodyPart.WING, visible);
            UnityHelper.SetAllChildrenVisible(this.objModel.transform, RolePlayer.BodyPart.SUIT, visible);
        }

        protected void SetAccessoryAnimation()
        {
            if (this.objModel != null)
            {
                AccessoryAnimation[] accessoryAnimations = this.objModel.transform.GetComponentsInChildren<AccessoryAnimation>();
                if (accessoryAnimations != null)
                {
                    for (int i = 0; i < accessoryAnimations.Length; i++)
                        accessoryAnimations[i].SetAnimation(this.PetName);
                }
            }
        }

        private void PlayAccessoryAnimation(Animator animator)
        {
            if (animator != null && animator.runtimeAnimatorController != null &&
                animator.runtimeAnimatorController.animationClips != null)
            {
                string prefix = "";
                switch (this.PetName.ToUpper())
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
                        Debug.LogFormat("<><PetPlayer.PlayAccessoryAnimation>PetName: {0}, Prefab: {1}", this.PetName.ToUpper(), prefix);
                        return;
                    }
                }
            }
        }
    }
}

