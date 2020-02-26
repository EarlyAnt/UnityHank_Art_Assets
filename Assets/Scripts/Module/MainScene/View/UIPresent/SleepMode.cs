using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Cup.Utils.audio;
using System;
using Cup.Utils.Screen;
using UnityEngine.Assertions;

namespace Hank.MainScene
{
    public class SleepMode : SilenceMode
    {

        [SerializeField]
        RolePlayer rolePlayerSC;
        [SerializeField]
        MissionBackground missionBackgroundSC;
        [SerializeField]
        GameObject objNormal;
        [Inject]
        public SleepModeTimeInSignal SleepModeTimeInSignal { get; set; }
        [Inject]
        public SleepModeTimeOutSignal sleepModeTimeOutSignal { get; set; }
        [Inject]
        public IDrinkWaterUIPresentManager uiState { get; set; }

        public override void TestSerializeField()
        {
            Assert.IsNotNull(processBar);
            Assert.IsNotNull(rolePlayerSC);
            Assert.IsNotNull(missionBackgroundSC);
            Assert.IsNotNull(objNormal);
        }

        protected override void Start()
        {
            base.Start();
            sleepModeTimeOutSignal.AddListener(CloseSleepMode);
        }

        protected override void OnDestroy()
        {
            sleepModeTimeOutSignal.RemoveListener(CloseSleepMode);
            base.OnDestroy();
        }


        Coroutine delayForSilenceCoroutine = null;
        //Coroutine delayForEndCoroutine = null;
        Coroutine actionPlayCoroutine = null;
        //DateTime endTime;
        bool bForceStartAni = false;

        IEnumerator DelayOneFrameForEnd()
        {
            bFinish = true;
            yield return null;
            objNormal.SetActive(true);
            gameObject.SetActive(false);
            FlurryUtil.LogEvent("Petscreen_Sleepmode_exit_ani_View");
            processFreshNewsSignal.Dispatch();
            if (!this.uiState.IsExistState(UIStateEnum.eRefreshMainByData))
                this.uiState.PushNewState(UIStateEnum.eRefreshMainByData);
        }


        IEnumerator DelayForSilence(float second)
        {
            sleepTimeManager.AddSleepStatus(SleepTimeout.NeverSleep);

            yield return new WaitForSeconds(second);
            dataManager.SetVolumeZero();
            sleepTimeManager.PopSleepStatus();

        }

        IEnumerator WaitActionPlay(string actionName, System.Action callback = null)
        {
            while (!rolePlayerSC.IsActionOnPlaying(actionName))
            {
                yield return null;
            }
            if (actionName == "sleep_begin")
            {
                rolePlayerSC.PlayActionSound("sleep_begin");
            }
            else if (actionName == "sleep_end")
            {
                rolePlayerSC.PlayActionSound("sleep_end");
            }

            float time = rolePlayerSC.GetCurrentAnimTime();

            yield return new WaitForSeconds(time);

            actionPlayCoroutine = null;
            if (actionName == "sleep_end")
            {
                StartCoroutine(DelayOneFrameForEnd());
            }

            if (callback != null)
                callback();
        }

        public override UIStateEnum GetStateEnum()
        {
            return UIStateEnum.eSleepMode;
        }
        public override void ProcessState(UIState state)
        {
            Debug.Log("----enter sleep mode");
            FlurryUtil.LogEvent("Petscreen_Sleepmode_enter_ani_View");
            bFinish = false;
            objNormal.SetActive(false);
            gameObject.SetActive(true);
            missionBackgroundSC.ChangeCurrBackGroudSprite(PlayerData.sleepMission);
            processBar.setAwardAndHandle(dataManager.percentOfDailyGoal);
            bForceStartAni = (bool)state.value1;

            StopMyCoroutine();
            rolePlayerSC.gameObject.SetActive(true);
            if (!bForceStartAni && dataManager.voicePercentBeforeSilenceMode > 0)
            {
                dataManager.SetVolumeZero();
                rolePlayerSC.Play("sleep_loop");
            }
            else
            {
                delayForSilenceCoroutine = StartCoroutine(DelayForSilence(15));
                rolePlayerSC.SetParameter("SleepBegin", true);
                rolePlayerSC.SetParameter("SleepEnd", false);
                actionPlayCoroutine = StartCoroutine(WaitActionPlay("sleep_begin"));
            }
            this.SleepModeTimeInSignal.Dispatch();
            if (RoleManager.Instance.CurrentRole != null)
                RoleManager.Instance.CurrentRole.TakeOffAllAccessories();
            RoleManager.Instance.HideMates();
        }


        private void StopMyCoroutine()
        {
            if (actionPlayCoroutine != null)
            {
                StopCoroutine(actionPlayCoroutine);
                actionPlayCoroutine = null;
            }
            if (delayForSilenceCoroutine != null)
            {
                StopCoroutine(delayForSilenceCoroutine);
                delayForSilenceCoroutine = null;
            }
        }

        override public void Stop()
        {
            base.Stop();
            StopMyCoroutine();

            objNormal.SetActive(true);

            dataManager.RecoverVolume();
            rolePlayerSC.SetParameter("SleepBegin", false);
            rolePlayerSC.SetParameter("SleepEnd", false);
            rolePlayerSC.Play("idle");
            gameObject.SetActive(false);

            if (RoleManager.Instance.CurrentRole != null)
            {
                RoleManager.Instance.CurrentRole.SetupPetAccessories();
                RoleManager.Instance.CurrentRole.TakeOffAllAccessories();
                RoleManager.Instance.CurrentRole.ResetStatus();
            }
            RoleManager.Instance.ShowMates();
        }

        void CloseSleepMode()
        {
            if (gameObject.activeInHierarchy)
            {
                dataManager.RecoverVolume();
                rolePlayerSC.SetParameter("SleepBegin", false);
                rolePlayerSC.SetParameter("SleepEnd", true);

                //rolePlayerSC.Play("sleep_end");
                StopMyCoroutine();
                actionPlayCoroutine = StartCoroutine(WaitActionPlay("sleep_end", () =>
                {
                    if (RoleManager.Instance.CurrentRole != null)
                    {
                        RoleManager.Instance.CurrentRole.SetupPetAccessories();
                        RoleManager.Instance.CurrentRole.ResetStatus();
                    }
                    RoleManager.Instance.ShowMates();
                }));
            }

        }
    }
}

