namespace Hank
{
    /// <summary>
    /// 息屏时间管理接口
    /// </summary>
    public interface ISleepTimeManager
    {
        //息屏时间压栈
        void AddSleepStatus(int sleepTimeout);
        //息屏时间出栈
        void PopSleepStatus();
        //重设息屏时间
        void ResetSleepStatus();
        //息屏时间改变委托
        System.Action<string, int> SleepStatusChanged { get; set; }
    }
}

