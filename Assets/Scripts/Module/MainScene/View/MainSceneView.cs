using Cup.Utils.android;
using Cup.Utils.audio;
using Cup.Utils.CupMotion;
using Cup.Utils.Touch;
using Cup.Utils.Water;
using CupSdk.BaseSdk.Water;
using Gululu;
using Gululu.Config;
using Gululu.LocalData.Agents;
using Gululu.Util;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Hank.MainScene
{
    public class MainSceneView : BaseView
    {
        /************************************************属性与变量命名************************************************/
        #region 注入与界面绑定属性
        [Inject]
        public IPlayerDataManager dataManager { get; set; }
        [Inject]
        public IWaterEventManager mWaterEventManager { get; set; }
        [Inject]
        public IDrinkWaterUIPresentManager uiState { get; set; }
        [Inject]
        public ISleepTimeManager sleepTimeManager { get; set; }
        [Inject]
        public IVolumeSetting VolumeSetting { get; set; }//声音设置
        [Inject]
        public IPrefabRoot PrefabRoot { get; set; }
        [SerializeField]
        public RolePlayer rolePlayer;
        [SerializeField]
        private GuestPlayer guestPlayer;
        [SerializeField]
        private PetTypes petType;
        [SerializeField]
        private bool bGuest;
        [SerializeField]
        private bool autoDrink = true;
        [SerializeField]
        private int waterIntake = 150;
        [SerializeField]
        private float timeInterval = 1.5f;
        [SerializeField]
        private GameObject objControl;
        [SerializeField]
        private Animator aniTmallRemind;
        public NavigatorView Navigator;
        public Spine.Unity.SkeletonGraphic GuideTipSpine;
        public GameObject mainUIObject;
        public GameObject TipPageRoot;
        public GameObject MainMenuRoot;
        public GameObject objTextTmallRoot;
        #endregion
        #region 信号与私有变量
        public Signal PowerClickSignal = new Signal();
        public Signal PowerHoldSignal = new Signal();
        public Signal CupShakeCupSignal = new Signal();
        public Signal CupTiltLeftSignal = new Signal();
        public Signal CupTiltRightSignal = new Signal();
        public Signal CupKeepFlatSignal = new Signal();
        public Signal CupTapLeftSignal = new Signal();
        public Signal CupTapRightSignal = new Signal();
        public Signal CupLeftSwipeUpSignal = new Signal();
        public Signal CupLeftSwipeDownSignal = new Signal();
        public Signal CupRightSwipeUpSignal = new Signal();
        public Signal CupRightSwipeDownSignal = new Signal();
        public Signal CupRightSwipeUpDownSignal = new Signal();

        public Signal showSystemMenuSignal = new Signal();
        public Signal showMainMenuSignal = new Signal();
        public Signal showTmallHouseSignal = new Signal();
        public Signal showMissionBrowerSignal = new Signal();
        public Signal showNoviceGuideView = new Signal();
        public Signal clearLocalDataSignalForTest = new Signal();
        public Signal<bool> showMakeFriendSignal = new Signal<bool>();
        public Signal showDrinkRemindSignal = new Signal();
        public Signal<int, long> drinkWaterSignal = new Signal<int, long>();
        public Signal goNextMissionSignal = new Signal();
        public Signal goPreMissionSignal = new Signal();
        public Signal showNewsEventSignal = new Signal();
        public Signal showSleepPageSignal = new Signal();

        private bool bInAni = false;
        private bool nextBack = true;
        private float delta_time = 0f;

        public enum PetTypes { Purpie = 0, Donny = 1, Ninji = 2, Sansa = 3, Yoyo = 4, Nuo = 5 }
        #endregion
        /************************************************Unity方法与事件***********************************************/
        protected override void Start()
        {
            base.Start();
#if (UNITY_ANDROID) && (!UNITY_EDITOR)
            RegisterDrinkWater();
#endif
            sleepTimeManager.ResetSleepStatus();
            Application.targetFrameRate = 12;
            bInAni = false;
            objControl.SetActive(false);
            autoDrink = false;
#if UNITY_EDITOR
            objControl.SetActive(true);
#endif
#if CUP_AUTOTEST
            autoDrink = true;
#else
            showNoviceGuideView.Dispatch();
#endif
            this.VolumeSetting.SetCurrentVolume();
        }
        protected override void OnDestroy()
        {
#if (UNITY_ANDROID) && (!UNITY_EDITOR)
            UnregisterDrinkWater();
#endif
            base.OnDestroy();
        }
        private void Update()
        {
#if CUP_AUTOTEST
            autoDrinkFunc();
#endif
            //autoChangeBack();
        }
        /************************************************自 定 义 方 法************************************************/
        /*============================================硬件输入及事件绑定==============================================*/
        private void RegisterDrinkWater()
        {
            mWaterEventManager.Init();
            mWaterEventManager.AddDrinkWaterCallBack(DrinkWaterCallback);
            mWaterEventManager.AddStartDrinkCallBack(StartDrinkWater);
            mWaterEventManager.AddStopDrinkCallBack(EndDrinkWater);
            mWaterEventManager.startMonitor();
        }
        private void UnregisterDrinkWater()
        {
            mWaterEventManager.RemoveDrinkWaterCallBack(DrinkWaterCallback);
            mWaterEventManager.RemoveStartDrinkCallBack(StartDrinkWater);
            mWaterEventManager.RemoveStopDrinkCallBack(EndDrinkWater);
        }
        /*========================================硬件输入及界面按钮事件绑定==========================================*/
        //左倾
        public void OnTiltLeft()
        {
            Debug.Log("----Input Detecte----MainSceneView.EmuTiltLeft--------");
            this.CupTiltLeftSignal.Dispatch();
        }
        //右倾
        public void OnTiltRight()
        {
            Debug.Log("----Input Detecte----MainSceneView.EmuTiltRight--------");
            this.CupTiltRightSignal.Dispatch();
        }
        //平放
        public void OnKeepFlat()
        {
            this.CupKeepFlatSignal.Dispatch();
        }
        //上划
        public void OnSwipeUp()
        {
            this.CupRightSwipeUpSignal.Dispatch();
        }
        //下划
        public void OnSwipeDown()
        {
            this.CupRightSwipeDownSignal.Dispatch();
        }
        //左拍
        public void OnTapLeft()
        {
            this.CupTapLeftSignal.Dispatch();
        }
        //右拍
        public void OnTapRight()
        {
            GuLog.Debug("<><MainSceneView>onRightTap");
            if (IsNormalStatus())
            {
                this.CupTapRightSignal.Dispatch();
            }
        }
        //摇晃
        public void OnShake()
        {
            //轻摇杯子
            Debug.Log("----Input Detecte----MainSceneView.EmuShake--------");
            this.CupShakeCupSignal.Dispatch();

            if (this.IsNormalStatus())
            {
                this.showMakeFriendSignal.Dispatch(true);
            }
        }
        //喝水
        public void OnDrink()
        {
            DrinkWaterCallback(waterIntake, 10000);
        }
        //单击
        public void OnPowerPress()
        {
            Debug.Log("----Input Detecte----MainSceneView.PowerKeyPress--------");
            this.PowerClickSignal.Dispatch();
        }
        //长按
        public void OnPowerLongPress()
        {
            Debug.Log("----Input Detecte----MainSceneView.PowerKeyLongPress--------");
            this.PowerHoldSignal.Dispatch();
        }
        //重置
        public void OnReset()
        {
            this.clearLocalDataSignalForTest.Dispatch();
        }
        //系统
        public void OnSystemMenu()
        {
            Debug.Log("<><>MainSceneView ShowSystemMenu<><>");
            this.showSystemMenuSignal.Dispatch();
        }
        //交友
        public void OnFriend()
        {
            //ShowAlertPair(AlertBoard.AlertType.AlertType_DailyGoal);
            //ShowAlertPair(AlertBoard.AlertType.AlertType_SilenceMode);
            //ShowAlertPair(AlertBoard.AlertType.AlertType_ClockAlarm);
            //MakeFriend(petType, bGuest);
            //DispatchMakeFriend();
            //onRightSwipeUp();
#if UNITY_EDITOR
            MakeFriend(this.petType.ToString("G").ToUpper(), this.bGuest);
#endif
        }
        //天猫
        public void OnTmall()
        {
            if (!IsNormalStatus(false, false))
            {
                return;
            }
            Debug.Log("<><MainSceneView> ShowTmall<><>");
            this.showTmallHouseSignal.Dispatch();
        }
        //后一个任务
        public void OnNextMission()
        {
            GuLog.Debug("<><MainSceneView>goNextMission");
            this.goNextMissionSignal.Dispatch();
            FlurryUtil.LogEvent("Petscreen_left_swipeup_event");
        }
        //前一个任务
        public void OnPreMission()
        {
            GuLog.Debug("<><MainSceneView>goPreMission");
            this.goPreMissionSignal.Dispatch();
            FlurryUtil.LogEvent("Petscreen_left_swipedown_event");
        }
        //右侧上下划
        public void OnRightSwipeUpAndDown()
        {
            this.CupRightSwipeUpDownSignal.Dispatch();
        }
        //新闻
        public void OnNewsEvent()
        {
            this.showNewsEventSignal.Dispatch();
        }
        //睡眠
        public void OnSleep()
        {
            this.showSleepPageSignal.Dispatch();
        }
        //宠物右后
        private void OnRightBackward()
        {
            GuLog.Debug("<><MainSceneView>onRightBackward");
            if (IsNormalStatus())
            {
                this.rolePlayer.Play("right_down");
            }
        }
        //宠物右前
        private void OnRightForward()
        {
            GuLog.Debug("<><MainSceneView>onRightForward");
            if (IsNormalStatus())
            {
                this.rolePlayer.Play("right_up");
            }
        }
        //宠物右前后
        private void OnRightBackwardForward()
        {
            GuLog.Debug("<><MainSceneView>onRightBackwardForward");
            if (IsNormalStatus())
            {
                this.rolePlayer.Play("right_updown");
            }
        }

        //上划
        public void SwipeUp()
        {
            //GuLog.Debug("<><MainSceneView>onRightSwipeUp");
            //if (IsNormalStatus())
            //{
            //    rolePlayer.Play("right_up");
            //    FlurryUtil.LogEvent("Petscreen_right_swipeup_event");
            //}

            GuLog.Debug("<><MainSceneView>onRightSwipeUp");
            FlurryUtil.LogEvent("Petscreen_right_swipeup_event");
            this.Navigator.TurnLeft();
        }
        //下划
        public void SwipeDown()
        {
            //GuLog.Debug("<><MainSceneView>onRightSwipeDown");
            //if (IsNormalStatus())
            //{
            //    rolePlayer.Play("right_down");
            //    FlurryUtil.LogEvent("Petscreen_right_swipedown_event");
            //}

            GuLog.Debug("<><MainSceneView>onRightSwipeDown");
            FlurryUtil.LogEvent("Petscreen_right_swipedown_event");
            this.Navigator.TurnRight();
        }
        //左拍
        public void TapLeft()
        {
        }
        //右拍
        public void TapRight()
        {
            if (IsNormalStatus())
            {
                rolePlayer.Play("right_tap");
                FlurryUtil.LogEvent("Petscreen_right_tap_event");
            }
        }
        //右侧上下划
        public void RightSwipeUpAndDown()
        {
            GuLog.Debug("<><MainSceneView>onRightSwipeUpAndDown");
            if (IsNormalStatus())
            {
                this.rolePlayer.Play("right_updown");
                FlurryUtil.LogEvent("Petscreen_right_updown_event");
            }
        }
        /*===========================================部分特定业务相关方法=============================================*/
        public void MakeFriend(string petType, bool bGuest)
        {
            if (bGuest)
            {
                rolePlayer.Play("friend_guest_out");
                StartCoroutine(WaitMakeFriendOver("friend_guest_out"));
            }
            else
            {
                rolePlayer.Play("friend_host");
                guestPlayer.RefreshRole(petType);
                guestPlayer.Play("friend_guest");
                StartCoroutine(WaitMakeFriendOver("friend_host"));
            }
        }
        public void MakeFriendFail()
        {
            rolePlayer.Play("friend_fail");
            showMakeFriendSignal.Dispatch(false);
        }
        private IEnumerator WaitMakeFriendOver(string actionName)
        {
            sleepTimeManager.AddSleepStatus(20);

            while (!rolePlayer.IsActionOnPlaying(actionName))
            {
                yield return null;
            }
            while (!rolePlayer.IsActionOnPlaying("idle"))
            {
                yield return null;
            }
            showMakeFriendSignal.Dispatch(false);
            guestPlayer.DestoryModel();
        }
        public bool IsNormalStatus(bool checkRole = true, bool checkIdle = true)
        {
            //            if (TopViewHelper.Instance.HasModulePageOpened())
            //            {
            //                GuLog.Debug("<><MainSceneView>MainView not top!");
            //                return false;
            //            }
            if (uiState.IsInRunning())
            {
                GuLog.Debug("<><MainSceneView>IsInRunning!");
                return false;
            }
            if (checkRole && !rolePlayer.gameObject.activeInHierarchy)
            {
                GuLog.Debug("<><MainSceneView>rolePlayer is hide!");
                return false;
            }
            if (checkIdle && !rolePlayer.IsActionOnPlaying("idle"))
            {
                GuLog.Debug("<><MainSceneView>rolePlayer not in idle!");
                return false;
            }
            return true;
        }

        public void DrinkRemind()
        {
            if (TopViewHelper.Instance.IsInGuideView())
            {
                return;
            }
            if (bInAni)
                return;
            bInAni = true;
            rolePlayer.Play("skill");
            aniTmallRemind.gameObject.SetActive(true);
            aniTmallRemind.Play("TmallRemindShow", 0, 0);
            StartCoroutine(DelayDrinkRemind());
        }
        private IEnumerator DelayDrinkRemind()
        {

            yield return new WaitForSeconds(2);
            rolePlayer.Play("idle");
            bInAni = false;
        }

        public void StartDrinkWater()
        {
            if (TopViewHelper.Instance.IsInGuideView())
            {
                return;
            }
            Debug.Log("<><>StartDrinkWater:<><>");

        }
        public void EndDrinkWater(long currentDuration, long totalDuration)
        {
            if (TopViewHelper.Instance.IsInGuideView())
            {
                return;
            }
            Debug.Log("<><>DrinkWaterCallback currentDuration:<><>" + currentDuration + "     totalDuration:" + totalDuration);

        }
        private void DrinkWaterCallback(int intake, long drinkDuration)
        {
            drinkWaterSignal.Dispatch(intake, drinkDuration);
        }

        //public void ShowAlertPair(AlertBoard.AlertType type)
        //{
        //    alertBoardSignal.Dispatch(type);
        //}
        public override void TestSerializeField()
        {
            //Assert.IsNotNull(silenceTimeProcess);
            Assert.IsNotNull(mainUIObject);
            Assert.IsNotNull(rolePlayer);
            Assert.IsNotNull(guestPlayer);
            Assert.IsNotNull(objTextTmallRoot);
            MainPresentBase[] allIPresents = transform.GetComponentsInChildren<MainPresentBase>(true);
            for (UIStateEnum i = UIStateEnum.eStartDailyProcessBar; i < UIStateEnum.eTotalUiState; ++i)
            {
                bool bFound = false;
                foreach (var p in allIPresents)
                {
                    if (p.GetStateEnum() == i)
                    {
                        bFound = true;
                        break;
                    }
                }
                Assert.IsTrue(bFound, "No present:" + i.ToString());
            }
            //Assert.IsTrue(aniTmallRemind != null);
        }

        #region 没啥用的方法
        private void autoChangeBack()
        {
            delta_time += (float)(Time.deltaTime * 0.2);
            if (delta_time >= timeInterval)
            {
                delta_time = 0f;

                if (nextBack)
                {
                    OnNextMission();
                    nextBack = false;
                }
                else
                {
                    OnPreMission();
                    nextBack = true;
                }
            }
        }
        private void autoDrinkFunc()
        {
            if (autoDrink)
            {
                delta_time += (float)(Time.deltaTime * 0.2);
                if (delta_time >= timeInterval)
                {
                    delta_time = 0f;
                    DrinkWaterCallback(waterIntake, 10000);
                }
            }
            else
            {
                delta_time = timeInterval;
            }
        }
        private void DispatchDrinkRemind()
        {
            showDrinkRemindSignal.Dispatch();
        }
        #endregion
    }
}