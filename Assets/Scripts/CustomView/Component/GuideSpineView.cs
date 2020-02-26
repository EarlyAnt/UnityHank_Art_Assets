using Gululu;
using Gululu.Config;
using Spine;
using Spine.Unity;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 引导动画页面
/// </summary>
public class GuideSpineView : BaseView
{
    /************************************************属性与变量命名************************************************/
    [SerializeField]
    private SkeletonGraphic spine;
    [SerializeField, Range(1, 10)]
    private int playTimes = 5;
    [SerializeField, Range(0f, 10f)]
    private float guideWait = 3f;
    [SerializeField, Range(0f, 60f)]
    private float interval = 20f;
    private Coroutine showGuideCoroutine;
    public Action<bool> Stopped { get; set; }
    /************************************************Unity方法与事件***********************************************/

    /************************************************自 定 义 方 法************************************************/
    //显示页面
    public void Play(string animationName)
    {
        this.spine.gameObject.SetActive(true);
        if (this.gameObject.activeInHierarchy)
            this.showGuideCoroutine = this.StartCoroutine(this.PlayAnimation(animationName));
    }
    //关闭页面
    public void Stop()
    {
        if (this.showGuideCoroutine != null)
            this.StopCoroutine(this.showGuideCoroutine);
        this.spine.gameObject.SetActive(false);
        this.OnStopped(false);
    }
    //定时循环播放动画
    private IEnumerator PlayAnimation(string animationName)
    {
        TrackEntry trackEntry = null;
        yield return new WaitForSeconds(this.guideWait);
        for (int times = 1; times <= this.playTimes && this.gameObject.activeInHierarchy; times++)
        {
            this.spine.gameObject.SetActive(true);
            trackEntry = this.spine.AnimationState.SetAnimation(0, animationName, false);
            Debug.LogFormat("<><GuideSpineView.PlayAnimation>Start play, name: {0}, times: {1}, animation duration: {2}, interval: {3}, totalWaitTime: {4}",
                            animationName, times, trackEntry.Animation.duration, this.interval, this.interval + trackEntry.Animation.duration);
            if (times <= this.playTimes) yield return new WaitForSeconds(trackEntry.Animation.duration);
            Debug.LogFormat("<><GuideSpineView.PlayAnimation>Animation completed, name: {0}, times: {1}", animationName, times);
            if (times <= this.playTimes) yield return new WaitForSeconds(this.interval);
        }
        this.spine.gameObject.SetActive(false);
        this.OnStopped(true);
    }
    //当停止播放动画时
    private void OnStopped(bool complete)
    {
        if (this.Stopped != null)
            this.Stopped(complete);
    }
}