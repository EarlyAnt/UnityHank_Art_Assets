using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using UnityEngine.Assertions;
using Cup.Utils.Screen;

namespace Hank.MainScene
{
    public class SchoolMode : SilenceMode
    {
        [Inject]
        public SchoolModeTimeInSignal SchoolModeTimeInSignal { get; set; }
        [Inject]
        public SchoolModeTimeOutSignal schoolModeTimeOutSignal { get; set; }
        [SerializeField]
        public ClockWithAmPm clockWithAmPm;
        [Inject]
        public IDrinkWaterUIPresentManager uiState { get; set; }

        public override void TestSerializeField()
        {
            Assert.IsNotNull(processBar);
            Assert.IsNotNull(clockWithAmPm);
        }

        IEnumerator DelayOneFrameForEnd()
        {
            bFinish = true;
            FlurryUtil.LogEvent("Petscreen_Schoolmode_exit_ani_View");
            //Debug.Log("<><SchoolMode><>----exit school mode0");

            yield return null;
            myCoroutine = null;
            gameObject.SetActive(false);
            Debug.Log("<><SchoolMode><>----exit school mode");

            dataManager.RecoverVolume();
            processFreshNewsSignal.Dispatch();

            if (!this.uiState.IsExistState(UIStateEnum.eRefreshMainByData))
                this.uiState.PushNewState(UIStateEnum.eRefreshMainByData);
        }


        Coroutine myCoroutine = null;
        DateTime endTime;

        public override UIStateEnum GetStateEnum()
        {
            return UIStateEnum.eSchoolMode;
        }
        public override void ProcessState(UIState state)
        {
            Debug.Log("<><SchoolMode><>----enter school mode");
            FlurryUtil.LogEvent("Petscreen_Schoolmode_enter_ani_View");
            bFinish = false;
            gameObject.SetActive(true);

            processBar.setAwardAndHandle(dataManager.percentOfDailyGoal);

            dataManager.SetVolumeZero();

            endTime = DateTime.Now.AddSeconds((float)state.value0);
            Debug.Log("<><SchoolMode><>EndTime: " + endTime.ToShortTimeString());
            this.SchoolModeTimeInSignal.Dispatch();
        }

        protected override void Start()
        {
            base.Start();
            schoolModeTimeOutSignal.AddListener(CloseSchoolMode);

        }

        protected override void OnDestroy()
        {
            schoolModeTimeOutSignal.RemoveListener(CloseSchoolMode);
            base.OnDestroy();
        }


        protected void CloseSchoolMode()
        {
            if (gameObject.activeInHierarchy)
            {
                clockWithAmPm.updateTime();
                GuLog.Debug("<><SchoolMode><>CloseSchoolMode:");
                if (myCoroutine != null)
                {
                    StopCoroutine(myCoroutine);
                }
                myCoroutine = StartCoroutine(DelayOneFrameForEnd());
            }
            RoleManager.Instance.CurrentRole.ResetStatus();
        }
        protected void ScreenOnOff(string status)
        {
            switch (status)
            {
                case "ACTION_SCREEN_ON":
                    {
                        clockWithAmPm.updateTime();
                        GuLog.Debug("<><SchoolMode><>ScreenOn:");
                    }
                    break;
            }
        }

        private void OnDisable()
        {
#if (UNITY_ANDROID) && (!UNITY_EDITOR)
            ScreenManager.removeListener(ScreenOnOff);
#endif
        }

        private void OnEnable()
        {
#if (UNITY_ANDROID) && (!UNITY_EDITOR)
            ScreenManager.addListener(ScreenOnOff);
#endif
        }

        override public void Stop()
        {
            base.Stop();
            dataManager.RecoverVolume();
            gameObject.SetActive(false);
        }

    }
}

