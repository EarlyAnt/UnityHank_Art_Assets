using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gululu.Util;
using System;
using strange.extensions.mediation.impl;
using Hank.MainScene;
using Cup.Utils.Screen;

namespace Hank
{
    public class SilenceTimeProcess : View
    {
        [Inject]
        public IDrinkWaterUIPresentManager uiState { get; set; }
        [Inject]
        public ISilenceTimeDataManager silenceTimeDataManager { get; set; }

        [Inject]
        public SilenceTimeProcessSignal silenceTimeProcessSignal { get; set; }
        [Inject]
        public IInteractiveCountManager interactiveCountManager { get; set; }
        [Inject]
        public IPlayerDataManager dataManager { get; set; }


        // Use this for initialization
        protected override void Start()
        {
            base.Start();
            silenceTimeProcessSignal.AddListener(ProcessNowTime);
        }
        protected override void OnDestroy()
        {
            silenceTimeProcessSignal.RemoveListener(ProcessNowTime);
            base.OnDestroy();
        }
        // Update is called once per frame
        void Update()
        {

        }

        IEnumerator DelayForSilenceTime(int second, SilenceTimeSection timeSection)
        {
            GuLog.Debug("<><ProcessSilenceTime>: WaitForSeconds:" + second + "    type:" + timeSection.sectionType.ToString());

            yield return new WaitForSeconds(second);
            DateTime time = DateTime.Now;
            DateTime beginTime = new DateTime(time.Year, time.Month, time.Day, timeSection.beginHour, timeSection.beginMinute, 0);
            DateTime endTime = new DateTime(time.Year, time.Month, time.Day, timeSection.endHour, timeSection.endMinute, 0);
            double escapeTime = (endTime - beginTime).TotalSeconds;
            PushNewState(timeSection, escapeTime, true);
            DateTime oneSecondAfterEndTime = new DateTime(time.Year, time.Month, time.Day, timeSection.endHour, timeSection.endMinute, 0);
            ProcessSilenceTime(timeSection.sectionType, oneSecondAfterEndTime);
        }

        private void PushNewState(SilenceTimeSection timeSection, double escapeTime, bool bForceStartAni)
        {
            //if (timeSection.endHour == 23 && timeSection.endMinute == 59)
            //{
            //    escapeTime += 60;
            //}

            if (timeSection.sectionType == SilenceTimeSection.SectionType.Study)
            {
                uiState.PushNewState(UIStateEnum.eSchoolMode, (float)escapeTime, bForceStartAni);
            }
            else if (timeSection.sectionType == SilenceTimeSection.SectionType.Sleep)
            {
                uiState.PushNewState(UIStateEnum.eSleepMode, (float)escapeTime, bForceStartAni);
            }
            uiState.PushNewState(UIStateEnum.eRefreshMainByData);
            uiState.PushNewState(UIStateEnum.eStartDailyProcessBar);
        }


        void ProcessSilenceTime(SilenceTimeSection.SectionType type, DateTime time)
        {
            int second = 0;
            SilenceTimeSection timeSection = silenceTimeDataManager.GetSilenceTimeFromTime(type, time, out second);
            if (timeSection != null)
            {
                if (second == 0)
                {
                    DateTime endTime = new DateTime(time.Year, time.Month, time.Day, timeSection.endHour, timeSection.endMinute, 0);

                    double escapeTime = (endTime - time).TotalSeconds;
                    PushNewState(timeSection, escapeTime, false);

                }
                //else
                //{
                //    StartCoroutine(DelayForSilenceTime(second, timeSection));
                //}
            }
        }
        void ProcessNowTime()
        {
            if (interactiveCountManager.IsNeedNoviceGuide())
            {
                return;
            }
#if CUP_AUTOTEST_SILENCE
            silenceTimeDataManager.test_school();
            //silenceTimeDataManager.test_schoolBak();
#endif

            if (uiState.IsInSilenceMode())
            {
                uiState.ClearState();
                _ProcessSilenceTime(DateTime.Now);
                if (!uiState.IsInSilenceMode())
                {
                    uiState.PushNewState(UIStateEnum.eRefreshMainByData);
                    uiState.PushNewState(UIStateEnum.eStartDailyProcessBar);
                }
            }
            else
            {
                _ProcessSilenceTime(DateTime.Now);
            }

            if (!uiState.IsInSilenceMode())
            {
                dataManager.RecoverVolume();
            }
        }

        private void _ProcessSilenceTime(DateTime time)
        {
            GuLog.Info("<><ProcessSilenceTime> time:" + time.ToShortTimeString());
            StopAllCoroutines();
            ProcessSilenceTime(SilenceTimeSection.SectionType.Study, time);
            ProcessSilenceTime(SilenceTimeSection.SectionType.Sleep, time);
        }

        public void ProcessSilenceTime(DateTime time)
        {
            _ProcessSilenceTime(time);
        }
        private void OnDisable()
        {
#if (UNITY_ANDROID) && (!UNITY_EDITOR)
            //ScreenManager.removeListener(ScreenOn);
#endif
        }

        private void OnEnable()
        {
#if (UNITY_ANDROID) && (!UNITY_EDITOR)
            //ScreenManager.addListener(ScreenOn);
#endif

        }
        void ScreenOn(string status)
        {
            switch (status)
            {
                case "ACTION_SCREEN_ON":
                    {
                        Debug.Log("<><ProcessSilenceTime>ScreenOn");

                        _ProcessSilenceTime(DateTime.Now);
                    }
                    break;
            }

        }

    }
}

