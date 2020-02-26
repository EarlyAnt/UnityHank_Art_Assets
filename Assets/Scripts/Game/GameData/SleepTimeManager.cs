using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace Hank
{
    /// <summary>
    /// 息屏时间管理
    /// </summary>
    public class SleepTimeManager : ISleepTimeManager
    {
        /************************************************属性与变量命名************************************************/
        public System.Action<string, int> SleepStatusChanged { get; set; }//息屏时间改变委托
        private List<int> topViewList = new List<int>();//息屏时间栈
        private const int DEFAULT_SLEEP_TIME = 60;//默认息屏时间
        private const string ADD_SLEEP_STATUS = "Add new status";//压栈操作
        private const string POP_SLEEP_STATUS = "Remove current status";//出栈操作
        /************************************************构  造  函  数************************************************/

        /************************************************自 定 义 方 法************************************************/
        //息屏时间压栈
        public void AddSleepStatus(int sleepTimeout)
        {
            GuLog.Info("<><SleepTimeManager>AddSleepStatus: " + sleepTimeout);
            this.SetSleepTimeout(ADD_SLEEP_STATUS, sleepTimeout);
            topViewList.Add(sleepTimeout);
            this.PrintTrack(sleepTimeout);
        }
        //息屏时间出栈
        public void PopSleepStatus()
        {
            if (topViewList.Count > 0)
            {
                GuLog.Info("<><SleepTimeManager>PopSleepStatus1: " + topViewList[topViewList.Count - 1]);
                topViewList.RemoveAt(topViewList.Count - 1);
            }

            if (topViewList.Count == 0)
            {
                AddSleepStatus(DEFAULT_SLEEP_TIME);
            }
            else
            {
                GuLog.Info("<><SleepTimeManager>PopSleepStatus2: " + topViewList[topViewList.Count - 1]);
                this.SetSleepTimeout(POP_SLEEP_STATUS, topViewList[topViewList.Count - 1]);
            }
        }
        //重设息屏时间
        public void ResetSleepStatus()
        {
            this.topViewList.Clear();
            this.topViewList.Add(DEFAULT_SLEEP_TIME);
            Screen.sleepTimeout = int.MaxValue;
            GuLog.Info("<><SleepTimeManager>ResetSleepStatus");
        }
        //设置息屏时间
        private void SetSleepTimeout(string operate, int sleepTimeout)
        {
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            GuLog.Info("<><SleepTimeManager>SetSleepTimeout: " + sleepTimeout);
            this.OnSleepStatusChanged(operate, sleepTimeout);
        }
        //当息屏时间改变时
        private void OnSleepStatusChanged(string operate, int seconds)
        {
            GuLog.Info(string.Format("<><SleepTimeManager>OnSleepStatusChanged: {0}, {1}", operate, seconds));
            GuLog.Info(TopViewHelper.Instance.ToString());
            if (this.SleepStatusChanged != null)
                this.SleepStatusChanged(operate, seconds);
        }
        //打印上层调用Track
        private void PrintTrack(int sleepTimeout)
        {
            if (sleepTimeout == SleepTimeout.NeverSleep)
            {
                string info = null;
                StackTrace st = new StackTrace(true);
                StackFrame[] sf = st.GetFrames();
                for (int i = 0; i < sf.Length; ++i)
                {
                    info = info + "\r\n" + " FileName=" + sf[i].GetFileName() + " fullname=" + sf[i].GetMethod().DeclaringType.FullName + " function=" + sf[i].GetMethod().Name + " FileLineNumber=" + sf[i].GetFileLineNumber();
                }
                GuLog.Info("sleep == -1 trace: " + info);
            }
        }
    }
}

