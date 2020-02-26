using Gululu.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cup.Utils.Screen;
using strange.extensions.mediation.impl;

namespace Hank
{
	public class DailyTaskResetProcess : View
    {
        [Inject]
		public IDrinkWaterUIPresentManager uiState{ get; set; }
        [Inject]
        public IPlayerDataManager dataManager { get; set; }
        [Inject]
        public IInteractiveCountManager interactiveCountManager { get; set; }

        const int oneDaySeconds = 86400;

		UnityEngine.Coroutine myCoroutine = null;

        private void CheckNewDay()
        {
            int currDay = DateUtil.ConvertToDay(DateTime.Today);
            if (dataManager.date != currDay)
            {
                dataManager.NewDay(currDay);
                interactiveCountManager.SetInteractive(InteractiveType.ScreenOn, 0);
                PushNewDayState();
            }
            interactiveCountManager.AddInteractive(InteractiveType.ScreenOn, 1);
        }

        IEnumerator CheckDayChange(float seconds)
		{
            GuLog.Debug("<><DailyTaskResetProcess>CheckDayChange after:" + seconds);
			yield return new WaitForSeconds(seconds);

            CheckNewDay();
            myCoroutine = StartCoroutine(CheckDayChange(oneDaySeconds));

		}

        public void ScreenOn()
        {
            FlurryUtil.LogEvent("Screen_On_daily_count");

            if(myCoroutine != null)
            {
                StopCoroutine(myCoroutine);
                myCoroutine = null;
            }
            StartCheckDayChangeCoroutine();
            CheckNewDay();
        }

        private void PushNewDayState()
        {
            uiState.PushNewState(UIStateEnum.eRefreshMainByData);
            uiState.PushNewState(UIStateEnum.eRolePlayAction, "hello", 0, false);
            uiState.PushNewState(UIStateEnum.eStartDailyProcessBar);
            uiState.ProcessStateRightNow();
        }

        private void StartCheckDayChangeCoroutine()
        {
            DateTime ZeroClockOfToday = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            TimeSpan ZeroClock2Now = ZeroClockOfToday - DateTime.Now;
            int leftSecond = oneDaySeconds - (int)ZeroClock2Now.TotalSeconds + 1;
            myCoroutine = StartCoroutine(CheckDayChange(leftSecond));
        }

        // Use this for initialization
        protected override void Start()
		{
            base.Start();
#if (UNITY_ANDROID) && (!UNITY_EDITOR)
            ScreenManager.addListener((status) =>
            {
                switch(status)
                {
                    case "ACTION_SCREEN_OFF":
                        {
                        }
                        break;
                    case "ACTION_SCREEN_ON":
                        {
                            ScreenOn();
                        }
                        break;
                }
                Debug.Log("<><>status<><>" + status);
            });
#endif
            StartCheckDayChangeCoroutine();

            PushNewDayState();


            if (dataManager.percentOfDailyGoal >= 1.0f)
                uiState.PushNewState(UIStateEnum.eDailyGoal100CompleteNotify, dataManager.missionId);


        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
