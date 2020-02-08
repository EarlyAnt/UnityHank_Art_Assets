using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ModelTest
{
    /// <summary>
    /// 小宠物动画控制
    /// </summary>
    public class PetPlayer : MonoBehaviour
    {
        /************************************************属性与变量命名************************************************/
        [SerializeField]
        private string petName;
        [SerializeField]
        private Animator animator;
        [SerializeField]
        private bool isPlaying;
        private string[] funnyAnimations = new string[] { "play_up_01", "play_down_01", "play_up_down_01" };
        /************************************************Unity方法与事件***********************************************/
        private void Start()
        {
            if (this.animator == null)
                this.animator = this.GetComponent<Animator>();
        }
        /************************************************自 定 义 方 法************************************************/
        public void SetPetName(string petName)
        {
            this.petName = petName;
        }
        public void ShowFunny()
        {
            if (!this.isPlaying)
            {
                this.StopAllCoroutines();
                int index = Random.Range(1, 9) % 3;
                this.StartCoroutine(this.PlayByName(this.funnyAnimations[index]));
            }
        }
        public void ShowHunger()
        {
            if (!this.isPlaying)
            {
                this.StopAllCoroutines();
                this.StartCoroutine(this.PlayByState("eat_hungry", "Hunger", true));
            }
        }
        public void ShowSatisfaction()
        {
            if (!this.isPlaying)
            {
                this.StopAllCoroutines();
                this.StartCoroutine(this.PlayByState("eat_hungry", "Hunger", false));
            }
        }
        public void ShowSleep()
        {
            if (!this.isPlaying)
            {
                this.StopAllCoroutines();
                this.StartCoroutine(this.PlayByState("sleep_end_01", "SleepEnd", false));
                this.StartCoroutine(this.PlayByState("sleep_begin_01", "SleepBegin", true));
            }
        }
        public void ShowWakeUp()
        {
            if (!this.isPlaying)
            {
                this.StopAllCoroutines();
                this.StartCoroutine(this.PlayByState("sleep_begin_01", "SleepBegin", false));
                this.StartCoroutine(this.PlayByState("sleep_end_01", "SleepEnd", true));
            }
        }
        private IEnumerator PlayByName(string animationName)
        {
            this.isPlaying = true;
            this.animator.Play(animationName);
            yield return new WaitForSeconds(this.GetAnimationLength(animationName));
            this.isPlaying = false;
        }
        private IEnumerator PlayByState(string animationName, string stateName, bool stateValue)
        {
            this.isPlaying = true;
            this.animator.SetBool(stateName, stateValue);
            yield return new WaitForSeconds(this.GetAnimationLength(animationName));
            this.isPlaying = false;
        }
        private float GetAnimationLength(string animationName)
        {
            string animationFullName = string.Format("{0}_{1}", this.petName.ToLower(), animationName);
            Debug.LogFormat("<><PetPlayer.GetAnimationLength>1-PetName: {0}, AnimationName: {1}, AnimationFullName: {2}",
                            this.petName, animationName, animationFullName);
            AnimationClip[] clips = this.animator.runtimeAnimatorController.animationClips;
            for (int i = 0; i < clips.Length; i++)
            {
                if (clips[i].name == animationFullName)
                {
                    Debug.LogFormat("<><PetPlayer.GetAnimationLength>2-AnimationFullName: {0}, Length: {1}", animationFullName, clips[i].length);
                    return clips[i].length;
                }
            }
            Debug.LogFormat("<><PetPlayer.GetAnimationLength>3-AnimationFullName: {0}, Length: 0", animationFullName);
            return 0;
        }
    }
}
