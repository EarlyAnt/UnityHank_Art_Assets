using Cup.Utils.android;
using Gululu.LocalData.Agents;
using Hank.Api;
using System;
using System.Collections.Generic;

namespace Gululu.LocalData.DataManager
{
    public interface ICupClockManager
    {

        IAlarmManager mAlarmManager { get; set; }
        ILocalClockInfoAgent mLocalClockInfoAgent { get; set; }

        IJsonUtils mJsonUtils { get; set; }
        void setNetClock(string childsn, string netClockInfo, Action success, Action error);

        void setOtherClock(string childsn, List<ClocksItem> clocks, Action success, Action error);

        void setOtherClock(string childsn, string silenceDatasString, Action success, Action error);
    }
}