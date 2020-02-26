using Gululu;
using Gululu.Config;
using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 计时器页面
/// </summary>
public class TimerView : BaseView
{
    /************************************************属性与变量命名************************************************/
    [Inject]
    public II18NUtils I18NUtils { get; set; }
    [SerializeField]
    private GameObject rootObject;
    [SerializeField]
    private Text timer;
    [SerializeField]
    private string textKey = "倒计天";
    private DateTime dateTime;
    public Action OnCompleted;//页面关闭时的委托
    /************************************************Unity方法与事件***********************************************/

    /************************************************自 定 义 方 法************************************************/
    //开启倒计时
    public void StartTimer(DateTime dateTime)
    {
        this.dateTime = dateTime;
        this.rootObject.SetActive(true);
        this.InvokeRepeating("UpdateTimer", 0f, 0.001f);
    }
    //停止倒计时
    public void StopTimer()
    {
        this.CancelInvoke();
        this.rootObject.SetActive(false);
    }
    //更新倒计时
    private void UpdateTimer()
    {
        TimeSpan timeSpan = (this.dateTime - DateTime.Now);
        if (timeSpan.TotalDays >= 3)
        {
            this.I18NUtils.SetText(this.timer, textKey, true, (int)timeSpan.TotalDays);
        }
        else if (timeSpan.TotalHours >= 1)
        {
            this.timer.text = string.Format("{0}:{1}:{2}", timeSpan.Hours.ToString().PadLeft(2, '0'),
                                            timeSpan.Minutes.ToString().PadLeft(2, '0'), timeSpan.Seconds.ToString().PadLeft(2, '0'));
        }
        else if (1 > timeSpan.TotalHours && timeSpan.TotalHours >= 0)
        {
            this.timer.text = string.Format("{0}:{1}:{2}:{3}", timeSpan.Hours.ToString().PadLeft(2, '0'), timeSpan.Minutes.ToString().PadLeft(2, '0'),
                                            timeSpan.Seconds.ToString().PadLeft(2, '0'), timeSpan.Milliseconds.ToString().PadLeft(3, '0'));
        }
        else if (timeSpan.TotalSeconds <= 0)
        {
            this.OnComplete();
            this.StopTimer();
        }
    }

    //当倒计时结束时
    private void OnComplete()
    {
        if (this.OnCompleted != null)
        {
            try
            {
                this.OnCompleted();
            }
            catch
            {
            }
        }
    }
}