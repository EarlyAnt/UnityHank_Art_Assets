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

        //���÷�����ʱ��(�˽ű��е�net�������ӣ�other���������ʱ�䣬����ѧϰʱ���˯��ʱ��)
        public void setOtherClock(string childsn, string silenceDatasString, Action success, Action error)
        {
            GuLog.Debug("<><CupClockManager> setOtherClock-New");

            GuLog.Debug(string.Format("----CupClockManager.setOtherClock-New---->childSN: {0}, originData: {1}", childsn, silenceDatasString));
            AllSilenceTimeSectionFromServer silenceDatas = mJsonUtils.String2Json<AllSilenceTimeSectionFromServer>(silenceDatasString);
            ClocksInfo clocksInfo = this.SilenceDatas2ClockInfos(childsn, silenceDatas);//�������·��ķ��������ݵĸ�ʽתΪ���ظ�ʽ
            string localClockInfo = mJsonUtils.Json2String(clocksInfo);
            mLocalClockInfoAgent.saveOtherClockInfo(childsn, localClockInfo);//��ת�ͺ�ķ��������ݴ��뱾��
            GuLog.Debug(string.Format("----CupClockManager.setOtherClock-New---->childSN: {0}, formatedData: {1}", childsn, localClockInfo));

            string netClockInfo = mLocalClockInfoAgent.getNetClockInfo(childsn);
            if (string.IsNullOrEmpty(netClockInfo))
            {//�����ʱˮ����û�д˺��ӵ��������ݣ�����ת�ͺ�ķ�����ʱ�串��
                mAlarmManager.replaceAlarmList(localClockInfo, success, error);
            }
            else
            {//�����ʱˮ�����д˺��ӵ��������ݣ��������������ƴ������һ�����͸��ײ�
                ClocksInfo netinfo = mJsonUtils.String2Json<ClocksInfo>(netClockInfo);
                netinfo.clocks.AddRange(clocksInfo.clocks);
                string allData = mJsonUtils.Json2String(netinfo);
                mAlarmManager.replaceAlarmList(allData, success, error);
                GuLog.Debug(string.Format("----CupClockManager.setOtherClock-New---->childSN: {0}, allData: {1}", childsn, allData));
            }
        }
        //����ת��(�������·��ķ��������ݵĸ�ʽתΪ���ظ�ʽ)
        private ClocksInfo SilenceDatas2ClockInfos(string childsn, AllSilenceTimeSectionFromServer silenceDatas)
        {
            if (silenceDatas != null && silenceDatas.configs != null && silenceDatas.configs.Count > 0)
            {
                int id = 1;
                ClocksInfo clocksInfo = new ClocksInfo();
                foreach (SilenceTimeSectionFromServer silenceData in silenceDatas.configs)
                {
                    clocksInfo.clocks.Add(new ClocksItem()
                    {//��ʼʱ��
                        id = id++,
                        x_child_sn = childsn,
                        clock_type = this.GetClockType(silenceData.type, "_begin"),
                        clock_enable = silenceData.enable,
                        clock_hour = silenceData.start_hour,
                        clock_min = silenceData.start_min,
                        clock_weekdays = silenceData.weekdays
                    });
                    clocksInfo.clocks.Add(new ClocksItem()
                    {//����ʱ��
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
        //��ȡ��������(�������ӣ�˯��ʱ�䣬ѧϰʱ��)
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