using DG.Tweening;
using Gululu.Config;
using Gululu.Util;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Hank.PetDress
{
    /// <summary>
    /// 配饰动画控制
    /// </summary>
    public class AccessoryAnimation : MonoBehaviour
    {
        /************************************************属性与变量命名************************************************/
        [SerializeField]
        private Animator animator;
        [SerializeField]
        private AnimationInfo animationInfo;
        /************************************************Unity方法与事件***********************************************/
        private void Start()
        {
            if (this.animator == null)
                this.animator = this.gameObject.GetComponent<Animator>();
        }
        /************************************************自 定 义 方 法************************************************/
        public void SetAnimation(string petName)
        {
            if (string.IsNullOrEmpty(petName) || this.animator == null || this.animationInfo == null)
                return;

            switch (petName.ToUpper())
            {
                case "PURPIE":
                    this.animator.Play(this.animationInfo.Purpie);
                    break;
                case "DONNY":
                    this.animator.Play(this.animationInfo.Donny);
                    break;
                case "NINJI":
                    this.animator.Play(this.animationInfo.Ninji);
                    break;
                case "SANSA":
                    this.animator.Play(this.animationInfo.Sansa);
                    break;
                case "YOYO":
                    this.animator.Play(this.animationInfo.Yoyo);
                    break;
                case "NUO":
                    this.animator.Play(this.animationInfo.Nuo);
                    break;
            }
        }
        [ContextMenu("查找动画片段")]
        private void FindAnimations()
        {
            if (this.animator != null && this.animator.runtimeAnimatorController != null && this.animator.runtimeAnimatorController.animationClips != null)
            {
                foreach (var animation in this.animator.runtimeAnimatorController.animationClips)
                {
                    if (animation.name.StartsWith("pur_"))
                        this.animationInfo.Purpie = animation.name;
                    else if (animation.name.StartsWith("dony_"))
                        this.animationInfo.Donny = animation.name;
                    else if (animation.name.StartsWith("nin_"))
                        this.animationInfo.Ninji = animation.name;
                    else if (animation.name.StartsWith("san_"))
                        this.animationInfo.Sansa = animation.name;
                    else if (animation.name.StartsWith("yoyo_"))
                        this.animationInfo.Yoyo = animation.name;
                    else if (animation.name.StartsWith("nuo_"))
                        this.animationInfo.Nuo = animation.name;
                }
            }
            else Debug.LogError("<><AccessoryAnimation.FindAnimations>Animator error");
        }
    }

    [Serializable]
    public class AnimationInfo
    {
        public string Purpie;
        public string Donny;
        public string Ninji;
        public string Sansa;
        public string Yoyo;
        public string Nuo;
    }
}