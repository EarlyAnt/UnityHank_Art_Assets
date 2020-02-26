using Cup.Utils.android;
using Cup.Utils.ble;
using CupSdk.BaseSdk.Water;
using Gululu;
using Gululu.Config;
using Gululu.Events;
using Gululu.LocalData.Agents;
using Gululu.Util;
using Hank.Api;
using Hank.CommingSoon;
using Hank.MainMenu;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using System;
using System.Collections;
using System.Collections.Generic;
using Gululiu.Hank.Common;
using UnityEngine;
using Gululu.net;
using strange.extensions.signal.api;
using Hank.DownloadPage2;
using Cup.Utils.Wifi;
using Hank.PetDress;

namespace Hank.MainScene
{
    public class MainSceneMediator : BaseMediator
    {
        /************************************************属性与变量命名************************************************/
        #region 注入属性
        [Inject]
        public MainSceneView viewMainScene { get; set; }
        [Inject]
        public IDrinkWaterUIPresentManager uiState { get; set; }
        [Inject]
        public IPlayerDataManager dataManager { get; set; }
        [Inject]
        public IMissionDataManager missionDataManager { get; set; }
        [Inject]
        public IDrinkWaterProcess dataProcess { get; set; }
        [Inject]
        public CloseSystemMenuSignal closeSystemMenuSignal { get; set; }
        [Inject]
        public ILocalChildInfoAgent mLocalChildInfoAgent { get; set; }
        [Inject]
        public ILocalPetInfoAgent mLocalPetInfoAgent { get; set; }
        [Inject]
        public IP2PManagerWrap mP2PManagerWrap { get; set; }
        [Inject]
        public ClearLocalDataSignal mClearLocalDataSignal { get; set; }
        [Inject]
        public StartNewGameSignal mStartNewGameSignal { get; set; }
        [Inject]
        public ILocalDataManager localDataRepository { set; get; }
        [Inject]
        public SilenceTimeProcessSignal silenceTimeProcessSignal { get; set; }
        [Inject]
        public SleepModeTimeInSignal SleepModeTimeInSignal { get; set; }
        [Inject]
        public SchoolModeTimeInSignal SchoolModeTimeInSignal { get; set; }
        [Inject]
        public IMissionConfig missionConifg { get; set; }
        [Inject]
        public ISilenceTimeDataManager SilenceTimeDataManager { get; set; }
        [Inject]
        public ISleepTimeManager SleepTimeManager { get; set; }//息屏状态控制器
        [Inject]
        public IItemsDataManager itemsDataManager { get; set; }
        [Inject]
        public IAccessoryDataManager AccessoryDataManager { get; set; }
        [Inject]
        public IPrefabRoot mPrefabRoot { get; set; }
        [Inject]
        public IRoleConfig RoleConfig { get; set; }//角色配置
        [Inject]
        public ILanConfig LanConfig { get; set; }//语言配置
        [Inject]
        public IWorldMapUtils WorldMapUtils { get; set; }
        [Inject]
        public StartHeartBeatCommandSignal StartHeartBeatCommandSignal { get; set; }
        [Inject]
        public IUnlockedPetsUtils UnlockedPetsUtils { get; set; }
        [Inject]
        public IInteractiveCountManager interactiveCountManager { get; set; }
        [Inject]
        public IAuthenticationUtils AuthenticationUtils { get; set; }
        [Inject]
        public ICommonResourceUtils CommonResourceUtils { get; set; }
        [Inject]
        public MediatorTmallAuthSignal mMediatorTmallAuthSignal { get; set; }
        [Inject]
        public MediatorTmallAuthErrSignal mMediatorTmallAuthErrSignal { get; set; }
        [Inject]
        public StartTmallAuthCommandSignal mStartTmallAuthCommandSignal { get; set; }
        [Inject]
        public ShowPetPlayerSignal ShowPetPlayerSignal { get; set; }
        [Inject]
        public ShowClockSignal ShowClockSignal { get; set; }
        [Inject]
        public ShowLanguageSignal ShowLanguageSignal { get; set; }
        [Inject]
        public ChangeRoleSignal ChangeRoleSignal { get; set; }
        [Inject(name = "FreshNewsSignal")]
        public IBaseSignal freshNewsSignal { get; set; }
        #endregion
        #region 界面根物体与私有变量
        private TopViewHelper.ETopView currentView = TopViewHelper.ETopView.eNone;
        private GameObject showMainMenuRootView = null;
        private GameObject commingSoonRootView = null;
        private GameObject petDressRootView = null;
        private GameObject downloadPage2RootView;//下载页根物体

        private Queue<System.Action> crossContextAction = new Queue<System.Action>();
        private List<string> roleNames = new List<string>();
        private int roleIndex = 0;
        private Coroutine changeRoleCoroutine;
        private static bool checkedDownload = false;
        #endregion
        /************************************************Unity方法与事件***********************************************/
        private void Start()
        {
            uiState.allIPresents = transform.GetComponentsInChildren<MainPresentBase>(true);
            silenceTimeProcessSignal.Dispatch();
            this.Initialize();
#if UNITY_ANDROID && !UNITY_EDITOR
            this.AuthenticationUtils.reNewToken(null, null);
#endif
            this.itemsDataManager.SendAllItemsData(true);
            RoleManager.Instance.ResetAccessoriesAndMates();
        }
        private void Update()
        {
            uiState.Update();
        }
        private void LateUpdate()
        {
            while (this.crossContextAction.Count > 0)
            {
                try
                {
                    this.crossContextAction.Dequeue().Invoke();
                }
                catch (Exception ex)
                {
                    Debug.LogErrorFormat("响应CrossContext事件时发生异常：{0}", ex.Message);
                }
            }
        }
        private void OnDestroy()
        {
        }
        private void OnApplicationQuit()
        {
        }
        /************************************************自 定 义 方 法************************************************/
        /*=================================================事件绑定===================================================*/
        public override void OnRegister()
        {
            this.View = TopViewHelper.ETopView.eMainView;
            base.OnRegister();
            this.viewMainScene.Navigator.ViewSelectedAction += this.OnOpenView;
            this.viewMainScene.PowerClickSignal.AddListener(this.OnPowerClick);
            this.viewMainScene.PowerHoldSignal.AddListener(this.OnPowerHold);
            this.viewMainScene.CupShakeCupSignal.AddListener(this.OnCupShake);
            this.viewMainScene.CupTiltLeftSignal.AddListener(this.OnCupTiltLeft);
            this.viewMainScene.CupTiltRightSignal.AddListener(this.OnCupTiltRight);
            this.viewMainScene.CupKeepFlatSignal.AddListener(this.OnCupKeepFlat);
            this.viewMainScene.CupTapLeftSignal.AddListener(this.OnCupTapLeft);
            this.viewMainScene.CupTapRightSignal.AddListener(this.OnCupTapRight);
            this.viewMainScene.CupLeftSwipeUpSignal.AddListener(this.OnCupLeftSwipeUp);
            this.viewMainScene.CupLeftSwipeDownSignal.AddListener(this.OnCupLeftSwipeDown);
            this.viewMainScene.CupRightSwipeUpSignal.AddListener(this.OnCupRightSwipeUp);
            this.viewMainScene.CupRightSwipeDownSignal.AddListener(this.OnCupRightSwipeDown);
            this.viewMainScene.CupRightSwipeUpDownSignal.AddListener(this.OnCupRightSwipeUpDown);
            this.viewMainScene.goPreMissionSignal.AddListener(this.OnCupLeftSwipeDown);
            this.viewMainScene.goNextMissionSignal.AddListener(this.OnCupLeftSwipeUp);

            this.viewMainScene.showMainMenuSignal.AddListener(ShowMainMenu);
            this.viewMainScene.showMakeFriendSignal.AddListener(ShowMakeFriend);
            this.viewMainScene.drinkWaterSignal.AddListener(OnDrinkWater);
            this.viewMainScene.clearLocalDataSignalForTest.AddListener(ClearLocalDataForTest);
            this.viewMainScene.showNewsEventSignal.AddListener(this.ShowNewsEvent);
            this.viewMainScene.showSleepPageSignal.AddListener(this.ShowSleepPage);
            this.mClearLocalDataSignal.AddListener(ClearLocalData);
            this.mStartNewGameSignal.AddListener(StartNewGame);
            this.StartHeartBeatCommandSignal.AddListener(this.HandleHeartBeat);

            this.dispatcher.UpdateListener(true, MainEvent.RefreshMainDrinkViewAfterPairEvent, this.RefreshMainDrinkViewAfterPair);
            this.dispatcher.UpdateListener(true, MainEvent.ShowNavigator, this.OnShowNavigator);
            this.dispatcher.UpdateListener(true, MainEvent.OpenView, this.OnOpenView);
            this.dispatcher.UpdateListener(true, MainEvent.CloseMainMenu, this.CloseMainMenu);
            this.dispatcher.UpdateListener(true, MainEvent.CloseCommingSoon, this.CloseCommingSoon);
            this.dispatcher.UpdateListener(true, MainEvent.ClosePetDress, this.ClosePetDress);
            this.dispatcher.UpdateListener(true, MainEvent.OpenDownloadPage2, this.ShowDownloadPage2View);
            this.dispatcher.UpdateListener(true, MainEvent.CloseDownloadPage2, this.CloseDownloadPage2View);

            this.SleepModeTimeInSignal.AddListener(this.HandleSilenceTimeProcess);
            this.SchoolModeTimeInSignal.AddListener(this.HandleSilenceTimeProcess);
            this.ShowClockSignal.AddListener(this.HandleClock);
        }
        public override void OnRemove()
        {
            base.OnRemove();
            this.viewMainScene.Navigator.ViewSelectedAction += this.OnOpenView;
            this.viewMainScene.PowerClickSignal.RemoveListener(this.OnPowerClick);
            this.viewMainScene.PowerHoldSignal.RemoveListener(this.OnPowerHold);
            this.viewMainScene.CupShakeCupSignal.RemoveListener(this.OnCupShake);
            this.viewMainScene.CupTiltLeftSignal.RemoveListener(this.OnCupTiltLeft);
            this.viewMainScene.CupTiltRightSignal.RemoveListener(this.OnCupTiltRight);
            this.viewMainScene.CupKeepFlatSignal.RemoveListener(this.OnCupKeepFlat);
            this.viewMainScene.CupTapLeftSignal.RemoveListener(this.OnCupTapLeft);
            this.viewMainScene.CupTapRightSignal.RemoveListener(this.OnCupTapRight);
            this.viewMainScene.CupLeftSwipeUpSignal.RemoveListener(this.OnCupLeftSwipeUp);
            this.viewMainScene.CupLeftSwipeDownSignal.RemoveListener(this.OnCupLeftSwipeDown);
            this.viewMainScene.CupRightSwipeUpSignal.RemoveListener(this.OnCupRightSwipeUp);
            this.viewMainScene.CupRightSwipeDownSignal.RemoveListener(this.OnCupRightSwipeDown);
            this.viewMainScene.CupRightSwipeUpDownSignal.RemoveListener(this.OnCupRightSwipeUpDown);
            this.viewMainScene.goPreMissionSignal.RemoveListener(this.OnCupLeftSwipeDown);
            this.viewMainScene.goNextMissionSignal.RemoveListener(this.OnCupRightSwipeUp);

            this.viewMainScene.showMainMenuSignal.RemoveListener(ShowMainMenu);
            this.viewMainScene.showMakeFriendSignal.RemoveListener(ShowMakeFriend);
            this.viewMainScene.drinkWaterSignal.RemoveListener(OnDrinkWater);
            this.viewMainScene.clearLocalDataSignalForTest.RemoveListener(ClearLocalDataForTest);
            this.viewMainScene.showNewsEventSignal.RemoveListener(this.ShowNewsEvent);
            this.viewMainScene.showSleepPageSignal.RemoveListener(this.ShowSleepPage);
            this.mClearLocalDataSignal.RemoveListener(ClearLocalData);
            this.mStartNewGameSignal.RemoveListener(StartNewGame);
            this.StartHeartBeatCommandSignal.RemoveListener(this.HandleHeartBeat);

            this.dispatcher.UpdateListener(false, MainEvent.RefreshMainDrinkViewAfterPairEvent, this.RefreshMainDrinkViewAfterPair);
            this.dispatcher.UpdateListener(false, MainEvent.ShowNavigator, this.OnShowNavigator);
            this.dispatcher.UpdateListener(false, MainEvent.OpenView, this.OnOpenView);
            this.dispatcher.UpdateListener(false, MainEvent.CloseMainMenu, this.CloseMainMenu);
            this.dispatcher.UpdateListener(false, MainEvent.CloseCommingSoon, this.CloseCommingSoon);
            this.dispatcher.UpdateListener(false, MainEvent.ClosePetDress, this.ClosePetDress);
            this.dispatcher.UpdateListener(false, MainEvent.OpenDownloadPage2, this.ShowDownloadPage2View);
            this.dispatcher.UpdateListener(false, MainEvent.CloseDownloadPage2, this.CloseDownloadPage2View);

            this.SleepModeTimeInSignal.RemoveListener(this.HandleSilenceTimeProcess);
            this.SchoolModeTimeInSignal.RemoveListener(this.HandleSilenceTimeProcess);
            this.ShowClockSignal.RemoveListener(this.HandleClock);
        }
        /*=================================================事件回调===================================================*/
        protected override void PowerClick()
        {
            this.CloseGuideTip();
        }
        protected override void PowerHold()
        {
//            if (TopViewHelper.Instance.IsOpenedView(TopViewHelper.ETopView.eLanguageSetting))
//                return;//语言设置页显示时，此页面不响应电源键长按操作(语言设置页放到独立场景中了，不需要此处显示系统页进行关机)

//            string currentChildSN = mLocalChildInfoAgent.getChildSN();
//            //Debug.LogFormat("<><MainSceneMediator.PowerKeyLongPress>GetChildSN: [{0}]", currentChildSN);
//            if (string.IsNullOrEmpty(currentChildSN))
//            {//如果处在放打扰模式(睡眠模式or学习模式)下，进入SystemMenu
//                this.ShowSystemMenu();
//            }
//            else
//            {
//                if (this.SilenceTimeDataManager.InSilenceMode(DateTime.Now))
//                {
//                    this.ShowSystemMenu();
//                }
//                else
//                {
//#if !UNITY_EDITOR
//                    if (interactiveCountManager.IsNeedNoviceGuide())
//                    {//如果处在引导环节，进入SystemMenu
//                        this.ShowSystemMenu();
//                    }
//                    else this.ShowMainMenu();
//#else
//                    this.ShowMainMenu();
//#endif
//                }
//            }
        }
        protected override void CupShake()
        {
            if (this.viewMainScene.IsNormalStatus())
                this.ShowMakeFriend(true);
        }
        protected override void CupTiltLeft()
        {
#if UNITY_EDITOR
            if (this.changeRoleCoroutine != null)
                this.StopCoroutine(this.changeRoleCoroutine);
            this.changeRoleCoroutine = this.StartCoroutine(this.KeepTilting(true));
#endif
        }
        protected override void CupTiltRight()
        {
#if UNITY_EDITOR
            if (this.changeRoleCoroutine != null)
                this.StopCoroutine(this.changeRoleCoroutine);
            this.changeRoleCoroutine = this.StartCoroutine(this.KeepTilting(false));
#endif
        }
        protected override void CupKeepFlat()
        {
#if UNITY_EDITOR
            if (this.changeRoleCoroutine != null)
                this.StopCoroutine(this.changeRoleCoroutine);
#endif
        }
        protected override void CupTapRight()
        {
            int number = UnityEngine.Random.Range(1, 10) % 3;
            switch (number)
            {
                case 0:
                    this.viewMainScene.SwipeUp();
                    break;
                case 1:
                    this.viewMainScene.TapRight();
                    break;
                case 2:
                    this.viewMainScene.SwipeDown();
                    break;
            }
        }
        protected override void CupLeftSwipeUp()
        {
            if (!TopViewHelper.Instance.IsBackToMainView() || this.SilenceTimeDataManager.InSilenceMode(DateTime.Now))
                return;

            int nextMission = missionConifg.GetNextMissionId(ShowMissionHelper.Instance.currShowMissionId);
            if (nextMission != -1 && missionDataManager.GetMisionState(nextMission) != MissionState.eLock)
            {
                Debug.Log("<><MainSceneMediator>goNextMission");
                uiState.PushNewState(UIStateEnum.eMissionFadeInOut, nextMission);
            }
            else
            {
                //this.ShowWorldMap();
            }
        }
        protected override void CupLeftSwipeDown()
        {
            if (!TopViewHelper.Instance.IsBackToMainView() || this.SilenceTimeDataManager.InSilenceMode(DateTime.Now))
                return;

            int preMission = missionConifg.GetLastMissionId(ShowMissionHelper.Instance.currShowMissionId);
            if (preMission != -1)
            {
                Debug.Log("<><MainSceneMediator>goPreMission");
                uiState.PushNewState(UIStateEnum.eMissionFadeInOut, preMission);
            }
        }
        protected override void CupRightSwipeUp()
        {
            if (!TopViewHelper.Instance.HasModulePageOpened() && !SilenceTimeDataManager.InSilenceMode(DateTime.Now))
                this.viewMainScene.SwipeUp();

        }
        protected override void CupRightSwipeDown()
        {
            if (!TopViewHelper.Instance.HasModulePageOpened() && !SilenceTimeDataManager.InSilenceMode(DateTime.Now))
                this.viewMainScene.SwipeDown();
        }
        protected override void CupRightSwipeUpDown()
        {
            if (!TopViewHelper.Instance.IsBackToMainView())
                return;
            this.viewMainScene.OnRightSwipeUpAndDown();
        }
        protected override void OnTopViewChanged(string operate, TopViewHelper.ETopView topView)
        {
            base.OnTopViewChanged(operate, topView);
            if (operate == TopViewHelper.REMOVE_VIEW || operate == TopViewHelper.RESET_VIEW)
            {
                this.CancelInvoke();
                this.Invoke("UnloadAllUnusedAssets", 0.5f);
            }
        }

        private void OnPowerClick()
        {
            dispatcher.Dispatch(MainKeys.PowerClick);
        }
        private void OnPowerHold()
        {
            dispatcher.Dispatch(MainKeys.PowerHold);
        }
        private void OnCupShake()
        {
            dispatcher.Dispatch(MainKeys.CupShake);
        }
        private void OnCupTiltLeft()
        {
            dispatcher.Dispatch(MainKeys.CupTiltLeft);
        }
        private void OnCupTiltRight()
        {
            dispatcher.Dispatch(MainKeys.CupTiltRight);
        }
        private void OnCupKeepFlat()
        {
            dispatcher.Dispatch(MainKeys.CupKeepFlat);
        }
        private void OnCupTapLeft()
        {
            dispatcher.Dispatch(MainKeys.CupTapLeft);
        }
        private void OnCupTapRight()
        {
            dispatcher.Dispatch(MainKeys.CupTapRight);
        }
        private void OnCupDoubleTapLeft()
        {
            dispatcher.Dispatch(MainKeys.CupDoubleTapLeft);
        }
        private void OnCupDoubleTapRight()
        {
            dispatcher.Dispatch(MainKeys.CupDoubleTapRight);
        }
        private void OnCupLeftSwipeUp()
        {
            dispatcher.Dispatch(MainKeys.CupLeftSwipeUp);
        }
        private void OnCupLeftSwipeDown()
        {
            dispatcher.Dispatch(MainKeys.CupLeftSwipeDown);
        }
        private void OnCupLeftSwipeUpDown()
        {
            dispatcher.Dispatch(MainKeys.CupLeftSwipeUpDown);
        }
        private void OnCupRightSwipeUp()
        {
            dispatcher.Dispatch(MainKeys.CupRightSwipeUp);
        }
        private void OnCupRightSwipeDown()
        {
            dispatcher.Dispatch(MainKeys.CupRightSwipeDown);
        }
        private void OnCupRightSwipeUpDown()
        {
            dispatcher.Dispatch(MainKeys.CupRightSwipeUpDown);
        }

        private void OnDrinkWater(int intake, long drinkDuration)
        {
            if (TopViewHelper.Instance.IsInGuideView() || TopViewHelper.Instance.IsOpenedView(TopViewHelper.ETopView.eLanguageSetting))
            {
                GuLog.Debug("<><MainSceneView>eNoviceGuide is top!");
                return;
            }

            if (DrinkUtils.isValidDrink(intake, drinkDuration))
            {
                GuLog.Debug("<><MainSceneMediator>DrinkWaterCallback intake:<><>" + intake + "   drinkDuration:" + drinkDuration);
                FlurryUtil.LogEvent("Drink_Daily_count");

                dataProcess.DrinkWater(intake);
                dataManager.TodayDrinkTimes += 1;
            }
            else
            {
                dataProcess.DrinkFail();
            }
            closeSystemMenuSignal.Dispatch();
        }
        private void ClearLocalDataForTest()
        {
            if (mLocalChildInfoAgent.getChildSN() != null && mLocalChildInfoAgent.getChildSN() != string.Empty)
            {
                localDataRepository.deleteAll();
                mLocalChildInfoAgent.Clear();
                mLocalPetInfoAgent.Clear();
                dataManager.CurEnergy = 0;
                StartNewGame();
            }
        }
        private void ClearLocalData()
        {
#if (UNITY_ANDROID) && (!UNITY_EDITOR)
            ClearLocalDataForTest();
#endif
        }
        private void RefreshMainDrinkViewAfterPair()
        {
            GuLog.Debug("<><MainSceneMediator>RefreshMainDrinkViewAfterPair");

            dispatcher.Dispatch(MainEvent.CloseCupPair);
            dispatcher.Dispatch(MainEvent.CloseAlarmClock);
            dispatcher.Dispatch(MainEvent.CloseNewsEvent);

            TopViewHelper.Instance.Reset();
            StartNewGame();
            RoleManager.Instance.Reset(false);
        }
        private void StartNewGame()
        {
            dataManager.LoadPlayerDatas();
            missionDataManager.LoadMissionData();
            itemsDataManager.LoadItemsData();
            AccessoryDataManager.LoadItemsData();
            SilenceTimeDataManager.LoadSilenceTimeSection();
            uiState.ClearState();
            uiState.PushNewState(UIStateEnum.eRefreshMainByData);
            uiState.PushNewState(UIStateEnum.eRolePlayAction, "hello", 0, false);
            uiState.PushNewState(UIStateEnum.eStartDailyProcessBar);
            uiState.ProcessStateRightNow();
            checkedDownload = false;
        }
        /*===============================================界面显示与关闭===============================================*/
        private void OnOpenView(IEvent evt)
        {
            if (evt == null || evt.data == null)
                return;

            this.currentView = (TopViewHelper.ETopView)evt.data;
            //Debug.LogFormat("<><MainSceneMediator.OnOpenView>{0}", this.currentView);
            this.crossContextAction.Enqueue((System.Action)(() =>
            {
                switch (this.currentView)
                {                    
                    case TopViewHelper.ETopView.eAchievement:
                        if (!this.ViewOpened(this.currentView))
                        {
                            this.ShowCommingSoon();
                            this.CloseMainMenuModule(TopViewHelper.ETopView.eCommingSoon);
                        }
                        break;                    
                    case TopViewHelper.ETopView.eFriend:
                        if (!this.ViewOpened(this.currentView))
                        {
                            this.ShowCommingSoon();
                            this.CloseMainMenuModule(TopViewHelper.ETopView.eCommingSoon);
                        }
                        break;                    
                    case TopViewHelper.ETopView.ePetDress:
                        RoleInfo roleInfo = this.RoleConfig.GetRoleInfo(this.dataManager.CurrentPet);
                        if (roleInfo != null && !roleInfo.IsLocal && !this.mLocalPetInfoAgent.ModelExists(roleInfo.Name))
                        {//当前选中的小宠物的资源不存在，不进入换装页，显示下载页或提示请联网下载资源
                            if (NativeWifiManager.Instance.IsConnected())
                                this.ShowDownloadPage2View();
                            else
                                SoundPlayer.GetInstance().PlaySoundType("download_popup_no_network");
                            return;
                        }
                        else if (RoleManager.Instance.CurrentRole.PetName != this.dataManager.CurrentPet)
                        {//当前选中的小宠物的资源已存在，但可能还没加载出来
                            //Debug.LogFormat("<><MainSceneMediator.OnOpenView>PetDress, CurrentName: {0}, Actual Name: {1}", RoleManager.Instance.CurrentRole.PetName, this.dataManager.CurrentPet);
                            return;
                        }

                        if (!this.ViewOpened(this.currentView))
                        {
                            this.ShowPetDress();
                            this.CloseMainMenuModule(TopViewHelper.ETopView.ePetDress);
                        }
                        break;
                }
            }));
        }
        private void OnOpenView(TopViewHelper.ETopView currentView)
        {
            this.crossContextAction.Enqueue((System.Action)(() =>
            {

                switch (currentView)
                {
                    case TopViewHelper.ETopView.eHome:
                        this.CloseNavigatorModule();
                        break;
                    case TopViewHelper.ETopView.ePetDress:
                        RoleInfo roleInfo = this.RoleConfig.GetRoleInfo(this.dataManager.CurrentPet);
                        if (roleInfo != null && !roleInfo.IsLocal && !this.mLocalPetInfoAgent.ModelExists(roleInfo.Name))
                        {//当前选中的小宠物的资源不存在，不进入换装页，显示下载页或提示请联网下载资源
                            if (NativeWifiManager.Instance.IsConnected())
                                this.ShowDownloadPage2View();
                            else
                                SoundPlayer.GetInstance().PlaySoundType("download_popup_no_network");
                            return;
                        }
                        else if (RoleManager.Instance.CurrentRole.PetName != this.dataManager.CurrentPet)
                        {//当前选中的小宠物的资源已存在，但可能还没加载出来
                            //Debug.LogFormat("<><MainSceneMediator.OnOpenView>PetDress, CurrentName: {0}, Actual Name: {1}", RoleManager.Instance.CurrentRole.PetName, this.dataManager.CurrentPet);
                            return;
                        }

                        if (!this.ViewOpened(currentView))
                        {
                            this.ShowPetDress();
                        }
                        break;
                }
            }));
        }
        private void OnShowNavigator(IEvent evt)
        {
            if (evt == null) return;

            MainEvent navigatorEvent = (MainEvent)evt.data;
            if (navigatorEvent == MainEvent.NavigatorUp)
                this.viewMainScene.Navigator.TurnLeft();
            else if (navigatorEvent == MainEvent.NavigatorDown)
                this.viewMainScene.Navigator.TurnRight();
        }
        /*---------------------------------------------------主界面---------------------------------------------------*/
        private void ShowMainMenu()
        {
            if (!checkedDownload)
            {//每天第一次进导航页的时候检查资源更新
                checkedDownload = true;
                this.CheckUpdate();
            }

            if (showMainMenuRootView != null)
            {
                showMainMenuRootView.SetActive(true);
            }
            else
            {
                mPrefabRoot.persentView<MainMenuRootView>(ref showMainMenuRootView, viewMainScene.MainMenuRoot.transform);
            }
        }
        private void CloseMainMenu(IEvent evt = null)
        {
            if (showMainMenuRootView != null)
            {
                Destroy(showMainMenuRootView);
                showMainMenuRootView = null;
            }
        }
        private void CloseMainMenuModule(params TopViewHelper.ETopView[] topViews)
        {
            if (topViews == null || !Array.Exists(topViews, t => t == TopViewHelper.ETopView.ePetDress))
            {
                this.ClosePetDress();
                RoleManager.Instance.Reset(false);
                this.dispatcher.Dispatch(MainEvent.ResetNavigator);
            }
        }
        private void CloseNavigatorModule(params TopViewHelper.ETopView[] topViews)
        {
            if ((topViews == null || !Array.Exists(topViews, t => t == TopViewHelper.ETopView.ePetFeed)) &&
                TopViewHelper.Instance.IsOpenedView(TopViewHelper.ETopView.ePetFeed))
                this.dispatcher.Dispatch(MainEvent.CloseView, TopViewHelper.ETopView.ePetFeed);

            if ((topViews == null || !Array.Exists(topViews, t => t == TopViewHelper.ETopView.ePetDress)) &&
                TopViewHelper.Instance.IsOpenedView(TopViewHelper.ETopView.ePetDress))
                this.dispatcher.Dispatch(MainEvent.CloseView, TopViewHelper.ETopView.ePetDress);
        }
        private bool ViewOpened(params TopViewHelper.ETopView[] openedViews)
        {
            if (openedViews == null || openedViews.Length == 0)
                return false;
            else
                return Array.Exists(openedViews, t => TopViewHelper.Instance.IsOpenedView(t));
        }
        /*-----------------------------------------------未开放功能界面-----------------------------------------------*/
        private void ShowCommingSoon()
        {
            if (commingSoonRootView != null)
            {
                commingSoonRootView.SetActive(true);
            }
            else
            {
                mPrefabRoot.persentView<CommingSoonRootView>(ref commingSoonRootView, viewMainScene.mainUIObject.transform);
            }
        }
        private void CloseCommingSoon(IEvent evt = null)
        {
            if (commingSoonRootView != null)
            {
                Destroy(commingSoonRootView);
                commingSoonRootView = null;
            }
        }
        /*--------------------------------------------------换装界面--------------------------------------------------*/
        private void ShowPetDress()
        {
            if (this.petDressRootView != null)
            {
                this.petDressRootView.SetActive(true);
            }
            else
            {
                mPrefabRoot.persentView<PetDressRootView>(ref petDressRootView, viewMainScene.mainUIObject.transform);
            }
        }
        private void ClosePetDress(IEvent evt = null)
        {
            if (this.petDressRootView != null)
            {
                Destroy(this.petDressRootView);
                this.petDressRootView = null;

                if (evt != null && evt.data != null && (bool)evt.data)
                {
                    uiState.PushNewState(UIStateEnum.eRefreshMainByData);
                    uiState.PushNewState(UIStateEnum.eStartDailyProcessBar);
                }
            }
        }        
        /*--------------------------------------------------好友界面--------------------------------------------------*/
        public void ShowMakeFriend(bool bStart)
        {
//#if (UNITY_ANDROID) && (!UNITY_EDITOR)
//            string childSn = mLocalChildInfoAgent.getChildSN();
//            if (childSn == null || childSn == string.Empty)
//            {
//                GuLog.Debug("<><MainSceneMediator><MakeFriend>childSn is null!");
//                viewMainScene.ShowAlertPair(AlertType.AlertType_MakeFriend);
//                return;
//            }

//            if (bStart)
//            {
//                SleepTimeManager.AddSleepStatus(20);
//                viewMainScene.rolePlayer.Play("friend_search");
//                FlurryUtil.LogEvent("Friending_Search_Event");

//                uiState.SetHoldStatus(true);
//                GuLog.Debug("<><MainSceneMediator><MakeFriend>new P2PManager!");

//                P2PSendInfo info = new P2PSendInfo();

//                info.childsn = mLocalChildInfoAgent.getChildSN();
//                info.cupSn = CupBuild.getCupSn();
//                info.petType = mLocalPetInfoAgent.getCurrentPet();
//                info.scanTimeOut = 8000;
//                info.waitTimeOut = 8000;
//                SoundPlayer.GetInstance().PlayMusic("friend_bgm");

//                mP2PManagerWrap.startP2P(info, OnP2PSuccess, OnP2PFail);
//                GuLog.Info("<><MainSceneMediator><MakeFriend>startP2P!");
//            }
//            else
//            {
//                uiState.SetHoldStatus(false);
//                mP2PManagerWrap.closeP2P();
//                SoundPlayer.GetInstance().StopMusic();
//                GuLog.Info("<><MainSceneMediator><MakeFriend>closeP2P!");
//            }
//#endif
        }
        private void OnP2PSuccess(P2PInfo info)
        {
            FlurryUtil.LogEvent("Friending_Search_Success_Event");

            viewMainScene.MakeFriend(info.pet_type, (info.role == "1"));
            GuLog.Debug("<><MainSceneMediator><MakeFriend>_P2PSuccess!");
        }
        private void OnP2PFail()
        {
            FlurryUtil.LogEvent("Friending_Search_Failed_Event");

            viewMainScene.MakeFriendFail();
            GuLog.Debug("<><MainSceneMediator><MakeFriend>_P2PFail!");
        }
        /*--------------------------------------------------新闻界面--------------------------------------------------*/
        private void ShowNewsEvent()
        {
            this.freshNewsSignal.Dispatch(new string[] { "test" });
        }
        /*--------------------------------------------------静默模式--------------------------------------------------*/
        private void HandleSilenceTimeProcess()
        {
            this.CloseMainMenu();
            this.CloseCommingSoon();
            this.ClosePetDress();
        }
        private void HandleClock()
        {
            this.CloseMainMenu();
        }
        private void ShowSleepPage()
        {
            this.uiState.PushNewState(UIStateEnum.eSleepMode, 10, false);
        }
        /*--------------------------------------------------切换宠物--------------------------------------------------*/
        private void Initialize()
        {
            this.roleNames = this.RoleConfig.GetAllRoleNames();
            if (this.roleNames == null || this.roleNames.Count == 0) return;
            int index = Array.IndexOf(this.roleNames.ToArray(), this.mLocalPetInfoAgent.getCurrentPet());
            if (index > 0) this.roleIndex = index;
        }
        private IEnumerator KeepTilting(bool left)
        {
            while (true)
            {
                this.ChangeRole(left);
                yield return new WaitForSeconds(0.7f);
            }
        }
        private void ChangeRole(bool left)
        {
            if (left && this.roleIndex > 0)
            {
                this.roleIndex -= 1;
                this.mLocalPetInfoAgent.saveCurrentPet(this.roleNames[this.roleIndex]);
                uiState.PushNewState(UIStateEnum.eRefreshMainByData);
                uiState.PushNewState(UIStateEnum.eStartDailyProcessBar);
                this.viewMainScene.rolePlayer.ResetStatus();
                this.ChangeRoleSignal.Dispatch();
            }
            else if (!left && this.roleIndex + 1 < this.roleNames.Count)
            {
                this.roleIndex += 1;
                this.mLocalPetInfoAgent.saveCurrentPet(this.roleNames[this.roleIndex]);
                uiState.PushNewState(UIStateEnum.eRefreshMainByData);
                uiState.PushNewState(UIStateEnum.eStartDailyProcessBar);
                this.viewMainScene.rolePlayer.ResetStatus();
                this.ChangeRoleSignal.Dispatch();
            }
        }
        /*--------------------------------------------------功能介绍--------------------------------------------------*/
        private void ShowGuideTip(IEvent evt)
        {
            if (evt != null && evt.data != null)
            {//切换星球
                object viewObject = Enum.Parse(typeof(TopViewHelper.ETopView), evt.data.ToString());
                if (viewObject == null) return;
                TopViewHelper.ETopView view = (TopViewHelper.ETopView)viewObject;
                bool viewValid = false;
                switch (view)
                {//只当这些页面关闭时，才按几率播放功能介绍提示音
                    case TopViewHelper.ETopView.eSystemMenu:
                    case TopViewHelper.ETopView.eCommingSoon:
                    case TopViewHelper.ETopView.eCollection:
                    case TopViewHelper.ETopView.ePetPage:
                    case TopViewHelper.ETopView.ePetFeed:
                    case TopViewHelper.ETopView.eWorldMap:
                        viewValid = true;
                        break;
                }
                if (!viewValid) return;
            }

            int seed = UnityEngine.Random.Range(1, 100);
            if (seed <= 30 && SoundPlayer.GetInstance() != null)
            {//30%的几率播放功能介绍提示音
                TopViewHelper.Instance.AddTopView(TopViewHelper.ETopView.eGuideTip);
                this.viewMainScene.GuideTipSpine.AnimationState.SetAnimation(0, "tips_ani01", false).Complete += delegate
                {
                    this.viewMainScene.GuideTipSpine.AnimationState.SetAnimation(0, "tips_ani02", true);
                };
                SoundPlayer.GetInstance().PlaySoundType("guide_homepage");
            }
        }
        private void CloseGuideTip()
        {
            TopViewHelper.Instance.RemoveView(TopViewHelper.ETopView.eGuideTip);
            if (this.viewMainScene.GuideTipSpine.AnimationState != null)
                this.viewMainScene.GuideTipSpine.AnimationState.SetAnimation(0, "tips_ani03", false);
            if (SoundPlayer.GetInstance() != null)
            {
                SoundPlayer.GetInstance().StopSound("mainview_tips_sfx");
                SoundPlayer.GetInstance().StopSound("guide_homepage");
            }
        }
        /*--------------------------------------------------资源下载--------------------------------------------------*/
        private void ShowDownloadPage2View()
        {
            this.HandleSilenceTimeProcess();//开始下载前，关闭所有功能页面
            if (this.downloadPage2RootView != null)
            {
                this.downloadPage2RootView.SetActive(true);
            }
            else
            {
                this.mPrefabRoot.persentView<DownloadPage2RootView>(ref this.downloadPage2RootView, viewMainScene.mainUIObject.transform);
            }
        }
        private void CloseDownloadPage2View()
        {
            if (this.downloadPage2RootView != null)
            {
                GameObject.Destroy(this.downloadPage2RootView);
                this.downloadPage2RootView = null;
                RoleManager.Instance.Reset(true);
            }
            this.viewMainScene.rolePlayer.StatusCheckingEnable = true;
        }
        private void CheckUpdate(bool forcedCheck = false)
        {
            if (PlayerDataManager.ResourceDownloaded && !forcedCheck)
            {
                Debug.Log("<><MainSceneMediator.CheckUpdate>ResourceDownloaded");
                return;
            }

            if (NativeWifiManager.Instance.IsConnected())
            {
                this.CommonResourceUtils.CheckResources((needToUpdate) =>
                {
                    Debug.LogFormat("<><MainSceneMediator.CheckUpdate>needToUpdate: {0}", needToUpdate);
                    this.crossContextAction.Enqueue((System.Action)(() =>
                    {
                        if (needToUpdate)
                            this.ShowDownloadPage2View();//有资源文件需要下载
                        else
                            this.viewMainScene.rolePlayer.StatusCheckingEnable = true;//不显示下载页，就先开启宠物饥饿状态监测，否则等到下载页关闭时再开启
                    }));
                });
            }
            else this.viewMainScene.rolePlayer.StatusCheckingEnable = true;
        }
        /*--------------------------------------------------处理心跳--------------------------------------------------*/
        private void HandleHeartBeat()
        {
            this.UnlockedPetsUtils.SyncUnlockedPets();
        }
        /*--------------------------------------------------资源回收--------------------------------------------------*/
        private void UnloadAllUnusedAssets()
        {
            Resources.UnloadUnusedAssets();
        }
    }
}