using Gululu;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Cup.Utils.android
{
    public class AlarmManager : IAlarmManager
    {
        [Inject]
        public IJsonUtils JsonUtils { get; set; }

        AndroidJavaClass alarmManagerWrap;
        AndroidJavaClass getAlarmManagerWrap()
        {
            if (alarmManagerWrap == null)
            {
                alarmManagerWrap = new AndroidJavaClass("com.bowhead.hank.manager.AlarmManagerWrap");
            }
            return alarmManagerWrap;
        }

        public void dismiss()
        {
            getAlarmManagerWrap().CallStatic("dismiss");
        }

        public void replaceAlarmList(string alarmInfo, Action success, Action error)
        {
            if (string.IsNullOrEmpty(alarmInfo))
            {
                GuLog.Error("----AlarmManager.replaceAlarmList----parameter[string: alarmInfo] is null or empty");
                return;
            }

            ClocksInfo clocksInfo = this.JsonUtils.String2Json<ClocksInfo>(alarmInfo);
            if (clocksInfo == null || clocksInfo.clocks == null)
            {
                GuLog.Error("----AlarmManager.replaceAlarmList----parameter[string: alarmInfo] can't convert to the type[ClocksInfo]");
                return;
            }

            List<ClocksItem> deleteItems = new List<ClocksItem>();
            clocksInfo.clocks.ForEach((clocksItem) =>
            {
                if (clocksItem != null && clocksItem.clock_enable == 0)
                    deleteItems.Add(clocksItem);
            });
            if (deleteItems.Count > 0)
                deleteItems.ForEach((clocksItem) => { clocksInfo.clocks.Remove(clocksItem); });

            alarmInfo = this.JsonUtils.Json2String(clocksInfo);
            GuLog.Debug(string.Format("----AlarmManager.replaceAlarmList----formatted alarmInfo: {0}", alarmInfo));

            SetAlarmCallback callback = new SetAlarmCallback();
            callback.setCallback(success, error);
            GuLog.Debug("<><AlarmManager> replaceAlarmList:" + alarmInfo);
            getAlarmManagerWrap().CallStatic("replaceAlarmList", alarmInfo, callback);
        }

        private AlarmListener listener;

        void mAlarmAlert(AlarmInstance alarm)
        {
            GuLog.Debug("<><AlarmManager> mAlarmAlert");

            if (mStringAlarmAlert != null)
            {
                mStringAlarmAlert(alarm.getAction());
            }
        }
        void mAlarmMissed(AlarmInstance alarm)
        {
            GuLog.Debug("<><AlarmManager> mAlarmMissed");
            if (mStringAlarmMissed != null)
            {
                mStringAlarmMissed(alarm.getAction());
            }
        }

        private Action<string> mStringAlarmAlert;
        private Action<string> mStringAlarmMissed;

        public void setAlarmListener(Action<string> alarmAlert, Action<string> alarmMissed)
        {
            mStringAlarmAlert = alarmAlert;
            mStringAlarmMissed = alarmMissed;
            if (listener == null)
            {
                listener = new AlarmListener();
            }
            listener.setListener(mAlarmAlert, mAlarmMissed);
            getAlarmManagerWrap().CallStatic("setAlarmListener", listener);
        }

        public void snooze()
        {
            getAlarmManagerWrap().CallStatic("snooze");
        }
        string currentAlarm;
        public string GetCurrentAlarm()
        {
            return currentAlarm;
        }
        public void SetCurrentAlarm(string alarm)
        {
            currentAlarm = alarm;
        }
    }
}