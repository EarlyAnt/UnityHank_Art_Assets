using Gululu;
using Gululu.Util;
using System;
using System.Collections;
using UnityEngine;

namespace Hank.StatusWatcher
{
    /// <summary>
    /// 状态检测器
    /// </summary>
    public class StatusWatcherView : BaseView
    {
        /************************************************属性与变量命名************************************************/
        [Inject]
        public ISleepTimeManager SleepTimeManager { get; set; }//息屏状态控制器
        [Inject]
        public IPlayerDataManager PlayerDataManager { get; set; }//用户数据管理
        private DateTime beginTime;//起始时间
        private const int MAX_TIME_LIMIT = 600;//最大息屏时长
        private const int DEFAULT_SLEEP_TIME = 60;//默认息屏时间
        private Coroutine timerCoroutine;//亮屏时间监测协程
        private Coroutine monitorCoroutine;//用户数据监测协程
        /************************************************Unity方法与事件***********************************************/
        protected override void Start()
        {
            this.StartCoroutine(this.Initialize());
        }
        /************************************************自 定 义 方 法************************************************/
        //初始化
        IEnumerator Initialize()
        {
            while (this.SleepTimeManager == null) yield return null;
            this.SleepTimeManager.SleepStatusChanged += this.OnSleepStatusChanged;
            this.timerCoroutine = this.StartCoroutine(this.StartTimer("InitTimer", DEFAULT_SLEEP_TIME));
            this.monitorCoroutine = this.StartCoroutine(this.MonitorData());
        }
        //开始计时
        IEnumerator StartTimer(string operate, int seconds)
        {
            this.beginTime = DateTime.Now;
            GuLog.Info(string.Format("----StatusWatcherView.StartTimer->operate: '{0}', seconds: {1}", operate, seconds));
            TimeSpan timeSpan;
            while (true)
            {
                timeSpan = DateTime.Now - this.beginTime;
                if (timeSpan.TotalSeconds >= seconds)
                {
                    GuLog.Info(string.Format("----StatusWatcherView.StartTimer->overtime({0}, {1}), operate is '{2}', seconds is '{3}'\nTopViewList: {4}",
                                              this.beginTime, DateTime.Now, operate, seconds, TopViewHelper.Instance.ToString()));
                    this.SleepTimeManager.ResetSleepStatus();
                    yield break;
                }
                //GuLog.Info(string.Format("----StatusWatcherView.Timing ->Setting: {0}, TotalSeconds: {1}, TotalMinutes: {2}",
                //                         seconds, timeSpan.TotalSeconds, timeSpan.TotalMinutes));
                yield return new WaitForSeconds(1f);
            }
        }
        //监测用户数据
        IEnumerator MonitorData()
        {
            while (true)
            {
                yield return new WaitForSeconds(10);
                if (this.PlayerDataManager.Date != DateTime.Today)
                {
                    //新的一天开始了
                    Debug.Log(string.Format("<><StatusWatcherView.MonitorData><Today is gone>DrinkTimes: {0}, Date: {1}",
                                            this.PlayerDataManager.TodayDrinkTimes, this.PlayerDataManager.Date));
                    this.PlayerDataManager.Date = DateTime.Today;
                    this.PlayerDataManager.TodayDrinkTimes = 0;
                    Debug.Log(string.Format("<><StatusWatcherView.MonitorData><New day>DrinkTimes: {0}, Date: {1}",
                                            this.PlayerDataManager.TodayDrinkTimes, this.PlayerDataManager.Date));
                }
            }
        }
        //当息屏模式改变时
        void OnSleepStatusChanged(string operate, int seconds)
        {
            if (!this.gameObject.activeInHierarchy)
                return;

            //如果seconds是设置为息屏，则计时时长设置为MAX_TIME_LIMIT
            if (seconds == SleepTimeout.NeverSleep)
                seconds = MAX_TIME_LIMIT;
            //不管上层设置的息屏时间是多少，都启动超时检测
            if (this.timerCoroutine != null)
                this.StopCoroutine(this.timerCoroutine);
            this.StartCoroutine(this.StartTimer(operate, seconds));
            GuLog.Info(string.Format("<><StatusWatcherView.OnSleepStatusChanged>operate: '{0}', seconds: {1}", operate, seconds));
            GuLog.Info(TopViewHelper.Instance.ToString());
        }
    }
}
