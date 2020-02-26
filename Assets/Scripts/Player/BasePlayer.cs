using Gululu.Config;
using Gululu.LocalData.Agents;
using Gululu.Util;
using Hank.PetDress;
using strange.extensions.mediation.impl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hank
{
    public class BasePlayer : View
    {
        [Inject]
        public IRoleConfig RoleConfig { get; set; }
        [Inject]
        public IActionConfig ActionConifg { get; set; }
        [Inject]
        public IAccessoryConfig AccessoryConfig { get; set; }
        [Inject]
        public IPrefabRoot PrefabRoot { get; set; }
        [Inject]
        public IResourceUtils ResourceUtils { get;set; }
        [Inject]
        public IAssetBundleUtils AssetBundleUtils { get; set; }
        [Inject]
        public IPlayerDataManager PlayerDataManager { get; set; }
        [Inject]
        public ILocalPetInfoAgent LocalPetInfoAgent { get; set; }
        [Inject]
        public ILocalUnlockedPetsAgent LocalUnlockedPetsAgent { get; set; }
        [Inject]
        public IDrinkWaterUIPresentManager UIState { get; set; }
        [Inject]
        public ILocalPetAccessoryAgent LocalPetAccessoryAgent { get; set; }
        [SerializeField]
        protected Transform roleRoot;
        [SerializeField]
        protected bool createDefaultRole = true;
        protected GameObject objModel;
        protected Animator animator;
        protected string modelPath;
        public string PetName { get; protected set; }
        protected string assetBundleName;
        private string lastSoundType;
        protected bool destroying;
        private Coroutine coroutine;

        protected override void Start()
        {
            base.Start();
            Initialize();
        }
        protected override void OnDestroy()
        {
            this.destroying = true;
        }

        protected virtual void Initialize()
        {
        }

        protected virtual void LoadPrefab(string prefabPath)
        {
            if (this.modelPath != prefabPath || objModel == null)
            {
                DestoryModel();
                this.modelPath = prefabPath;
                this.objModel = PrefabRoot.GetGameObject("Role", this.modelPath);
                if (this.objModel != null)
                {
                    this.objModel.transform.SetParent(this.roleRoot, false);
                    this.objModel.tag = this.GetTag();
                    animator = objModel.GetComponent<Animator>();
                    this.LocalPetAccessoryAgent.LoadData();
                }
            }
        }
        public virtual void DestoryModel()
        {
            if (objModel != null)
            {
                animator = null;
                Destroy(objModel);
                objModel = null;
                this.modelPath = "";
            }
        }
        protected virtual string GetTag()
        {
            return "";
        }

        public virtual void Show()
        {
            if (this.objModel != null && !this.destroying)
            {
                this.objModel.SetActive(true);
            }
        }
        public virtual void Hide()
        {
            if (this.objModel != null)
                this.objModel.SetActive(false);
        }
        public virtual void ResetStatus()
        {
        }

        public void Play(string strAni)
        {
            if (string.IsNullOrEmpty(PetName) || string.IsNullOrEmpty(strAni))
                return;

            AniAndSound info = ActionConifg.GetAniAndSound(PetName, strAni);
            GuLog.Debug("<><BasePlayer>Play Pet:" + PetName + "  Ani:" + strAni);
            if (info != null)
            {
                GuLog.Debug("<><BasePlayer>Play Action:" + info.AniName);
                if (animator)
                {
                    animator.Play(info.AniName);
                }
                Debug.LogFormat("<><BasePlayer.Play>Sound: {0}, HasModulePageOpened: {1}, InGuideView: {2}",
                                info.SoundType, TopViewHelper.Instance.HasModulePageOpened(), TopViewHelper.Instance.IsInGuideView());
                if (!string.IsNullOrEmpty(info.SoundType) && this.PlayerDataManager.LanguageInitialized() &&
                    (!TopViewHelper.Instance.HasModulePageOpened() || TopViewHelper.Instance.IsInGuideView()))
                {//设置过语言且没有打开任何功能模块，或正在引导才能播放此声音
                    Debug.LogFormat("<><BasePlayer.Play>Sound: {0} ==== Playing", info.SoundType);
                    SoundPlayer.GetInstance().PlayRoleSound(info.SoundType);
                }
            }
        }
        public bool IsActionOnPlaying(string strAni)
        {
            if (animator && gameObject.activeInHierarchy)
            {
                AniAndSound info = ActionConifg.GetAniAndSound(PetName, strAni);
                if (info != null)
                {
                    AnimatorStateInfo stateinfo = animator.GetCurrentAnimatorStateInfo(0);
                    return stateinfo.IsName(info.AniName);
                }
                else
                {
                    GuLog.Info("Not Action:" + strAni + "   in Pet model:" + PetName);
                }
            }
            return true;
        }

        public float GetCurrentAnimTime()
        {
            AnimatorStateInfo stateinfo = animator.GetCurrentAnimatorStateInfo(0);

            return stateinfo.length;
        }

        public void SetParameter(string param, bool value)
        {
            if (animator)
            {
                animator.SetBool(param, value);
            }
        }
        public void PlayActionSound(string strAni)
        {
            AniAndSound info = ActionConifg.GetAniAndSound(PetName, strAni);
            GuLog.Debug("<><BasePlayer>Play Pet:" + PetName + "  Ani:" + strAni);
            if (info != null)
            {
                Debug.LogFormat("<><BasePlayer.PlayActionSound>Sound: {0}, HasModulePageOpened: {1}, InGuideView: {2}",
                                info.SoundType, TopViewHelper.Instance.HasModulePageOpened(), TopViewHelper.Instance.IsInGuideView());
                if (!string.IsNullOrEmpty(info.SoundType) && (!TopViewHelper.Instance.HasModulePageOpened() || TopViewHelper.Instance.IsInGuideView()))
                {//没有打开任何功能模块，或正在引导才能播放此声音
                    Debug.LogFormat("<><BasePlayer.PlayActionSound>Sound: {0} ==== Playing", info.SoundType);
                    SoundPlayer.GetInstance().PlayRoleSound(info.SoundType);
                }
            }

        }

        public void CrossFade(string animation, float fadeTime = 0.2f, System.Action complete = null)
        {
            AniAndSound info = ActionConifg.GetAniAndSound(PetName, animation);
            GuLog.Debug("<><BasePlayer>CrossFade Pet:" + PetName + "  Ani:" + animation);
            if (info != null)
            {
                GuLog.Debug("<><BasePlayer>Play Action:" + info.AniName);

                if (animator)
                {
                    animator.CrossFade(info.AniName, fadeTime);
                    if (this.coroutine != null)
                        this.StopCoroutine(this.coroutine);
                    this.coroutine = StartCoroutine(WaitAnimationPlayComplete(animation, complete));
                }
                if (!string.IsNullOrEmpty(info.SoundType))
                {
                    this.lastSoundType = info.SoundType;
                    SoundPlayer.GetInstance().PlayRoleSound(info.SoundType);
                }
            }
        }
        IEnumerator WaitAnimationPlayComplete(string animName, System.Action complete)
        {
            yield return new WaitForSeconds(GetAnimationTime(animName));
            if (complete != null)
            {
                complete.Invoke();
            }
        }
        public void StopAnimation()
        {
            if (this.coroutine != null)
                this.StopCoroutine(this.coroutine);

            if (!string.IsNullOrEmpty(this.lastSoundType))
                SoundPlayer.GetInstance().StopRoleSound(this.lastSoundType);
        }
        public float GetAnimationTime(string animationName)
        {
            float length = 0;
            AniAndSound info = ActionConifg.GetAniAndSound(PetName, animationName);
            GuLog.Debug("<><BasePlayer>GetAnimationTime Pet:" + PetName + "  Ani:" + animationName);
            if (info != null)
            {
                AnimationClip[] clips = this.animator.runtimeAnimatorController.animationClips;
                foreach (AnimationClip clip in clips)
                {
                    if (clip.name.ToLower().Contains(info.AniName.ToLower()))
                    {
                        length = clip.length;
                        break;
                    }
                }
            }
            return length;
        }

        public void SwitchHungryState(bool isHungry)
        {
            this.animator.SetBool("Hunger", isHungry);
        }

        public void ShowParticleEffect()
        {
            if (this.objModel == null) return;
            ParticleSystem[] particleSystems = this.objModel.GetComponentsInChildren<ParticleSystem>();
            if (particleSystems != null && particleSystems.Length > 0)
            {
                for (int i = 0; i < particleSystems.Length; i++)
                    particleSystems[i].gameObject.SetActive(true);
            }
        }
        public void HideParticleEffect()
        {
            if (this.objModel == null) return;
            ParticleSystem[] particleSystems = this.objModel.GetComponentsInChildren<ParticleSystem>();
            if (particleSystems != null && particleSystems.Length > 0)
            {
                for (int i = 0; i < particleSystems.Length; i++)
                    particleSystems[i].gameObject.SetActive(false);
            }
        }
    }
}

