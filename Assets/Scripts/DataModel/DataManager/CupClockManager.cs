using System;
using System.Collections.Generic;
using Cup.Utils.android;
using Gululu.LocalData.Agents;
using Hank.Api;

namespace Gululu.LocalData.DataManager
{
    public class CupClockManager : ICupClockManager
    {
        [Inject]
        public IAlarmManager mAlarmManager { get; set; }

        [Inject]
        public ILocalClockInfoAgent mLocalClockInfoAgent { get; set; }

        [Inject]
        public IJsonUtils mJsonUtils { get; set; }

        public void setNetClock(string childsn, string netClockInfo, Action success, Action error)
        {
            GuLog.Debug("<><CupClockManager> setNetClock");

            string otherClockInfo = mLocalClockInfoAgent.getOtherClockInfo(childsn);

            mLocalClockInfoAgent.saveNetClockInfo(childsn, netClockInfo);
            UnityEngine.Debug.LogFormat("----{0}----CupClockManager.setNetClock: {1}, {2}", System.DateTime.Now.ToString("HH:mm:ss:fff"), childsn, netClockInfo);
            if (string.IsNullOrEmpty(otherClockInfo))
            {
                UnityEngine.Debug.LogFormat("----{0}----CupClockManager.replaceAlarmList_1", System.DateTime.Now.ToString("HH:mm:ss:fff"));
                mAlarmManager.replaceAlarmList(netClockInfo, success, error);
            }
            else
            {
                ClocksInfo otherinfo = mJsonUtils.String2Json<ClocksInfo>(otherClockInfo);
                ClocksInfo netinfo = mJsonUtils.String2Json<ClocksInfo>(netClockInfo);

                netinfo.clocks.AddRange(otherinfo.clocks);
                string allinfoStr = mJsonUtils.Json2String(netinfo);

                mAlarmManager.replaceAlarmList(allinfoStr, success, error);
                UnityEngine.Debug.LogFormat("----{0}----CupClockManager.replaceAlarmList_2: {1}, {2}, {3}", System.DateTime.Now.ToString("HH:mm:ss:fff"), allinfoStr, otherinfo, netinfo);
            }
        }

        public void setOtherClock(string childsn, List<ClocksItem> clocks, Action success, Action error)
        {
            GuLog.Debug("<><CupClockManager> setOtherClock");

            string netClockInfo = mLocalClockInfoAgent.getNetClockInfo(childsn);

            ClocksInfo localinfo = new ClocksInfo();
            localinfo.clocks = clocks;
            string localClockInfo = mJsonUtils.Json2String(localinfo);
            mLocalClockInfoAgent.saveOtherClockInfo(childsn, localClockInfo);
            if (string.IsNullOrEmpty(netClockInfo))
            {
                mAlarmManager.replaceAlarmList(localClockInfo, success, error);
            }
            else
            {
                ClocksInfo netinfo = mJsonUtils.String2Json<ClocksInfo>(netClockInfo);
                netinfo.clocks.AddRange(clocks);

                string allinfoStr = mJsonUtils.Json2String(netinfo);

                mAlarmManager.replaceAlarmList(allinfoStr, success, error);
            }
        }

        //设置防打扰时间(此脚本中的net代表闹钟，other代表防打扰时间，包括学习时间和睡眠时间)
        public void setOtherClock(string childsn, string silenceDatasString, Action success, Action error)
        {
            GuLog.Debug("<><CupClockManager> setOtherClock-New");

            GuLog.Debug(string.Format("----CupClockManager.setOtherClock-New---->childSN: {0}, originData: {1}", childsn, silenceDatasString));
            AllSilenceTimeSectionFromServer silenceDatas = mJsonUtils.String2Json<AllSilenceTimeSectionFromServer>(silenceDatasString);
            ClocksInfo clocksInfo = this.SilenceDatas2ClockInfos(childsn, silenceDatas);//将网络下发的防打扰数据的格式转为本地格式
            string localClockInfo = mJsonUtils.Json2String(clocksInfo);
            mLocalClockInfoAgent.saveOtherClockInfo(childsn, localClockInfo);//将转型后的防打扰数据存入本地
            GuLog.Debug(string.Format("----CupClockManager.setOtherClock-New---->childSN: {0}, formatedData: {1}", childsn, localClockInfo));

            string netClockInfo = mLocalClockInfoAgent.getNetClockInfo(childsn);
            if (string.IsNullOrEmpty(netClockInfo))
            {//如果此时水杯中没有此孩子的闹钟数据，则用转型后的防打扰时间覆盖
                mAlarmManager.replaceAlarmList(localClockInfo, success, error);
            }
            else
            {//如果此时水杯中有此孩子的闹钟数据，则与防打扰数据拼接起来一并推送给底层
                ClocksInfo netinfo = mJsonUtils.String2Json<ClocksInfo>(netClockInfo);
                netinfo.clocks.AddRange(clocksInfo.clocks);
                string allData = mJsonUtils.Json2String(netinfo);
                mAlarmManager.replaceAlarmList(allData, success, error);
                GuLog.Debug(string.Format("----CupClockManager.setOtherClock-New---->childSN: {0}, allData: {1}", childsn, allData));
            }
        }
        //数据转型(将网络下发的防打扰数据的格式转为本地格式)
        private ClocksInfo SilenceDatas2ClockInfos(string childsn, AllSilenceTimeSectionFromServer silenceDatas)
        {
            if (silenceDatas != null && silenceDatas.configs != null && silenceDatas.configs.Count > 0)
            {
                int id = 1;
                ClocksInfo clocksInfo = new ClocksInfo();
                foreach (SilenceTimeSectionFromServer silenceData in silenceDatas.configs)
                {
                    clocksInfo.clocks.Add(new ClocksItem()
                    {//起始时间
                        id = id++,
                        x_child_sn = childsn,
                        clock_type = this.GetClockType(silenceData.type, "_begin"),
                        clock_enable = silenceData.enable,
                        clock_hour = silenceData.start_hour,
                        clock_min = silenceData.start_min,
                        clock_weekdays = silenceData.weekdays
                    });
                    clocksInfo.clocks.Add(new ClocksItem()
                    {//结束时间
                        id = id++,
                        x_child_sn = childsn,
                        clock_type = this.GetClockType(silenceData.type, "_end"),
                        clock_enable = silenceData.enable,
                        clock_hour = silenceData.end_hour,
                        clock_min = silenceData.end_min,
                        clock_weekdays = silenceData.weekdays
                    });
                }
                return clocksInfo;
            }
            else
            {
                GuLog.Error("----CupClockManager.SilenceDatas2ClockInfos---->silenceDatas is null");
                return null;
            }
        }
        //获取闹钟类型(包括闹钟，睡眠时间，学习时间)
        private string GetClockType(string originType, string suffix = "")
        {
            string type = "sleep";
            switch (originType.ToLower())
            {
                case "sleep":
                    type = "sleep";
                    break;
                case "school":
                    type = "study";
                    break;
            }
            return type + (suffix != null ? suffix : "");
        }
    }
}