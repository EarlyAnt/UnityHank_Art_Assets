using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpineTest : MonoBehaviour
{
    [SerializeField]
    private SkeletonGraphic spineGraphic;
    [SerializeField]
    private string startAnimationName;
    [SerializeField]
    private bool loop = true;
    private List<string> animations = new List<string>();
    private int animationIndex = 0;

    private void Start()
    {
        if (this.spineGraphic == null)
            this.spineGraphic = this.GetComponent<SkeletonGraphic>();

        foreach (var animation in this.spineGraphic.SkeletonData.Animations)
        {
            this.animations.Add(animation.Name);
        }
        this.PlayAnimation(!string.IsNullOrEmpty(this.startAnimationName) ? this.startAnimationName : this.animations[0]);
    }

    private void Update()
    {
        if (this.spineGraphic == null || this.animations.Count == 0)
            return;

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            this.animationIndex = (this.animationIndex + this.animations.Count - 1) % this.animations.Count;
            this.PlayAnimation(this.animations[this.animationIndex]);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            this.animationIndex = (this.animationIndex + 1) % this.animations.Count;
            this.PlayAnimation(this.animations[this.animationIndex]);
        }
    }

    private void PlayAnimation(string animationName)
    {
        //this.spineGraphic.AnimationState.ClearTracks();//这里不能ClearTracks，否则电池的所有格子不能全部正常显示
        this.spineGraphic.AnimationState.SetAnimation(0, animationName, this.loop);
        Debug.LogFormat("<><SpineTest.PlayAnimation>currrent animation: {0}", animationName);
    }
}

