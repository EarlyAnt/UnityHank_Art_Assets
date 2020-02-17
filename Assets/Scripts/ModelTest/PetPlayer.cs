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
        private string[] funnyAnimations = new string[] { Animations.PlayUp, Animations.PlayDown, Animations.PlayUpDown };
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
        public void ShowIdle()
        {
            if (!this.isPlaying)
            {
                Queue<AnimationInfo> animations = new Queue<AnimationInfo>();
                if (this.GetState(AnimationParams.Hunger))
                    animations.Enqueue(new AnimationInfo() { DelayReturn = 5, PlayMethod = this.PlayByState(Animations.EatHungry, AnimationParams.Hunger, false) });
                animations.Enqueue(new AnimationInfo() { PlayMethod = this.PlayByName(Animations.Idle) });
                this.StopAllCoroutines();
                this.StartCoroutine(this.PlayAnimations(animations));
            }
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
                this.StartCoroutine(this.PlayByState(Animations.EatHungry, AnimationParams.Hunger, true));
            }
        }
        public void ShowSatisfaction()
        {
            if (!this.isPlaying)
            {
                Queue<AnimationInfo> animations = new Queue<AnimationInfo>();
                if (this.GetState(AnimationParams.Hunger))
                    animations.Enqueue(new AnimationInfo() { DelayReturn = 5, PlayMethod = this.PlayByState(Animations.EatHungry, AnimationParams.Hunger, false) });
                animations.Enqueue(new AnimationInfo() { PlayMethod = this.PlayByName(Animations.EatSatisfaction) });
                animations.Enqueue(new AnimationInfo() { PlayMethod = this.PlayByName(Animations.Idle) });
                this.StopAllCoroutines();
                this.StartCoroutine(this.PlayAnimations(animations));
            }
        }
        public void ShowSleep()
        {
            if (!this.isPlaying)
            {
                Queue<AnimationInfo> animations = new Queue<AnimationInfo>();
                if (this.GetState(AnimationParams.SleepEnd))
                    animations.Enqueue(new AnimationInfo() { PlayMethod = this.PlayByState(Animations.SleepEnd, AnimationParams.SleepEnd, false) });
                animations.Enqueue(new AnimationInfo() { PlayMethod = this.PlayByState(Animations.SleepBegin, AnimationParams.SleepBegin, true) });
                this.StopAllCoroutines();
                this.StartCoroutine(this.PlayAnimations(animations));
            }
        }
        public void ShowWakeUp()
        {
            if (!this.isPlaying)
            {
                Queue<AnimationInfo> animations = new Queue<AnimationInfo>();
                if (this.GetState(AnimationParams.SleepBegin))
                    animations.Enqueue(new AnimationInfo() { PlayMethod = this.PlayByState(Animations.SleepBegin, AnimationParams.SleepBegin, false) });
                animations.Enqueue(new AnimationInfo() { PlayMethod = this.PlayByState(Animations.SleepEnd, AnimationParams.SleepEnd, true) });
                this.StopAllCoroutines();
                this.StartCoroutine(this.PlayAnimations(animations));
            }
        }
        private IEnumerator PlayAnimations(Queue<AnimationInfo> animations)
        {
            while (animations.Count > 0)
            {
                AnimationInfo animationInfo = animations.Dequeue();
                yield return new WaitForSeconds(animationInfo.DelayStart);
                yield return this.StartCoroutine(animationInfo.PlayMethod);
                yield return new WaitForSeconds(animationInfo.DelayReturn);
            }
        }
        private IEnumerator PlayByName(string animationName, System.Action callback = null)
        {
            this.isPlaying = true;
            this.animator.CrossFade(animationName, 0.2f);
            yield return new WaitForSeconds(this.GetAnimationLength(animationName));
            if (callback != null) callback(); else this.isPlaying = false;
        }
        private IEnumerator PlayByState(string animationName, string stateName, bool stateValue, System.Action callback = null)
        {
            this.isPlaying = true;
            this.animator.SetBool(stateName, stateValue);
            yield return new WaitForSeconds(this.GetAnimationLength(animationName));
            if (callback != null) callback(); else this.isPlaying = false;
        }
        private bool GetState(string stateName)
        {
            return this.animator.GetBool(stateName);
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

    public class AnimationInfo
    {
        public float DelayStart { get; set; }
        public float DelayReturn { get; set; }
        public IEnumerator PlayMethod { get; set; }
    }
}
