using Cup.Utils.android;
using Cup.Utils.ble;
using Cup.Utils.Water;
using Gululu;
using Gululu.Config;
using Gululu.Events;
using Gululu.LocalData.Agents;
using Gululu.LocalData.DataManager;
using Gululu.net;
using Gululu.Util;
using Hank.Action;
using Hank.Api;
using Hank.MainScene;
using strange.extensions.command.api;
using strange.extensions.command.impl;
using strange.extensions.context.api;
//using Hank.MainScene;
using strange.extensions.context.impl;
using UnityEngine;

namespace Hank.PreLoad
{
    public class GlobalContext : MVCSContext
    {

        // Use this for initialization
        public GlobalContext(MonoBehaviour view, ContextStartupFlags flags) : base(view, flags) { }

        protected override void addCoreComponents()
        {
            base.addCoreComponents();
            injectionBinder.Unbind<ICommandBinder>();
            injectionBinder.Bind<ICommandBinder>().To<SignalCommandBinder>().ToSingleton();
        }

        override public void Launch()
        {
            base.Launch();
            StartSignal startSignal = (StartSignal)injectionBinder.GetInstance<StartSignal>();
            startSignal.Dispatch();
        }

        protected override void mapBindings()
        {
            base.mapBindings();

            injectionBinder.Bind<IUploadChildFriendsModel>().To<UploadChildFriendsModel>().ToSingleton().CrossContext();
            injectionBinder.Bind<IUploadChildIntakeLogsModel>().To<UploadChildIntakeLogsModel>().ToSingleton().CrossContext();

            injectionBinder.Bind<IJsonUtils>().To<JsonUtils>().ToSingleton().CrossContext();
            injectionBinder.Bind<IGululuNetwork>().To<GululuNetwork>().ToSingleton().CrossContext();
            injectionBinder.Bind<IUrlProvider>().To<UrlProvider>().ToSingleton().CrossContext();
            injectionBinder.Bind<IAuthenticationUtils>().To<AuthenticationUtils>().ToSingleton().CrossContext();
            injectionBinder.Bind<IUnlockedPetsUtils>().To<UnlockedPetsUtils>().ToSingleton().CrossContext();
            injectionBinder.Bind<IPetAccessoryUtils>().To<PetAccessoryUtils>().ToSingleton().CrossContext();
            injectionBinder.Bind<IValidateResponseData>().To<ValidateResponseData>().ToSingleton().CrossContext();
            injectionBinder.Bind<ICachePath>().To<CachePath>().ToSingleton().CrossContext();

            injectionBinder.Bind<ILocalAddFriendsInfo>().To<LocalAddFriendsInfo>().ToSingleton().CrossContext();
            injectionBinder.Bind<ILocalChildInfoAgent>().To<LocalChildInfoAgent>().ToSingleton().CrossContext();
            injectionBinder.Bind<ILocalPetAccessoryAgent>().To<LocalPetAccessoryAgent>().ToSingleton().CrossContext();
            injectionBinder.Bind<ILocalCupAgent>().To<LocalCupAgent>().ToSingleton().CrossContext();
            injectionBinder.Bind<ILocalPetInfoAgent>().To<LocalPetInfoAgent>().ToSingleton().CrossContext();
            injectionBinder.Bind<ILocalUnlockedPetsAgent>().To<LocalUnlockedPetsAgent>().ToSingleton().CrossContext();
            injectionBinder.Bind<IWaterEventManager>().To<WaterEventManager>().ToSingleton().CrossContext();

            injectionBinder.Bind<IUploadChildIntakeLogsService>().To<UploadChildIntakeLogsService>().ToSingleton().CrossContext();
            injectionBinder.Bind<IUploadChildFriendsService>().To<UploadChildFriendsService>().ToSingleton().CrossContext();

            injectionBinder.Bind<IAndroidJumpActivity>().To<AndroidJumpActivity>().ToSingleton().CrossContext();

            injectionBinder.Bind<IAddFriendsInfoManager>().To<AddFriendsInfoManager>().ToSingleton().CrossContext();
            injectionBinder.Bind<IIntakeLogDataManager>().To<IntakeLogDataManager>().ToSingleton().CrossContext();
            injectionBinder.Bind<IHeartbeatActionManager>().To<HeartbeatActionManager>().ToSingleton().CrossContext();
            injectionBinder.Bind<ILocalDataManager>().To<LocalDataManager>().ToSingleton().CrossContext();
            injectionBinder.Bind<ILocalIntakeLogsData>().To<LocalIntakeLogsData>().ToSingleton().CrossContext();
            injectionBinder.Bind<ILogUtils>().To<LogUtils>().ToSingleton().CrossContext();
            injectionBinder.Bind<ILanguageUtils>().To<LanguageUtils>().ToSingleton().CrossContext();
            injectionBinder.Bind<IResourceUtils>().To<ResourceUtils>().ToSingleton().CrossContext();
            injectionBinder.Bind<IAssetBundleUtils>().To<AssetBundleUtils>().ToSingleton().CrossContext();
            injectionBinder.Bind<IHotUpdateUtils>().To<HotUpdateUtils>().ToSingleton().CrossContext();
            injectionBinder.Bind<ICommonResourceUtils>().To<CommonResourceUtils>().ToSingleton().CrossContext();
            injectionBinder.Bind<IPrefabRoot>().To<PrefabRoot>().ToSingleton().CrossContext();
            injectionBinder.Bind<ILangManager>().To<LangManager>().ToSingleton().CrossContext();
            injectionBinder.Bind<II18NUtils>().To<I18NUtils>().ToSingleton().CrossContext();
            injectionBinder.Bind<IWorldMapUtils>().To<WorldMapUtils>().ToSingleton().CrossContext();

            injectionBinder.Bind<GululuNetworkHelper>().ToSingleton().CrossContext();

            injectionBinder.Bind<IGameDataHelper>().To<GameDataHelper>().ToSingleton().CrossContext();
            injectionBinder.Bind<IAwardDataHelper>().To<AwardDataHelper>().ToSingleton().CrossContext();
            injectionBinder.Bind<IAccessoryDataManager>().To<AccessoryDataManager>().ToSingleton().CrossContext();
            injectionBinder.Bind<INetUtils>().To<NetUtils>().ToSingleton().CrossContext();

            injectionBinder.Bind<IDrinkWaterUIPresentManager>().To<DrinkWaterUIPresentManager>().ToSingleton().CrossContext();
            injectionBinder.Bind<IMissionDataManager>().To<MissionDataManager>().ToSingleton().CrossContext();
            injectionBinder.Bind<IFundDataManager>().To<FundDataManager>().ToSingleton().CrossContext();
            injectionBinder.Bind<IPlayerDataManager>().To<PlayerDataManager>().ToSingleton().CrossContext();
            injectionBinder.Bind<IItemsDataManager>().To<ItemsDataManager>().ToSingleton().CrossContext();
            injectionBinder.Bind<IDrinkWaterProcess>().To<DrinkWaterProcess>().ToSingleton().CrossContext();
            injectionBinder.Bind<ISilenceTimeDataManager>().To<SilenceTimeDataManager>().ToSingleton().CrossContext();
            injectionBinder.Bind<IP2PManagerWrap>().To<P2PManagerWrap>().ToSingleton().CrossContext();
            injectionBinder.Bind<ITimeManager>().To<TimeManager>().ToSingleton().CrossContext();
            injectionBinder.Bind<IHeartbeatManager>().To<HeartbeatManager>().ToSingleton().CrossContext();
            injectionBinder.Bind<IAlarmManager>().To<AlarmManager>().ToSingleton().CrossContext();

            injectionBinder.Bind<IActionFactory>().To<ActionFactory>().ToSingleton().CrossContext();
            injectionBinder.Bind<IActionBase>().To<SetValuesAction>().ToName("SetValuesAction").ToSingleton().CrossContext();
            injectionBinder.Bind<IActionBase>().To<GetValuesAction>().ToName("GetValuesAction").ToSingleton().CrossContext();
            injectionBinder.Bind<IActionBase>().To<SetValuesLongAction>().ToName("SetValuesLongAction").ToSingleton().CrossContext();
            injectionBinder.Bind<IActionBase>().To<GetValuesLongAction>().ToName("GetValuesLongAction").ToSingleton().CrossContext();
            injectionBinder.Bind<IActionBase>().To<ResetAction>().ToName("ResetAction").ToSingleton().CrossContext();
            injectionBinder.Bind<IActionBase>().To<RebootAction>().ToName("RebootAction").ToSingleton().CrossContext();
            injectionBinder.Bind<IActionBase>().To<DeleteAction>().ToName("DeleteAction").ToSingleton().CrossContext();
            injectionBinder.Bind<INativeDataManager>().To<NativeDataManager>().ToSingleton().CrossContext();

            injectionBinder.Bind<ClearLocalDataSignal>().ToSingleton().CrossContext();
            injectionBinder.Bind<StartNewGameSignal>().ToSingleton().CrossContext();
            injectionBinder.Bind<ShowLanguageSignal>().ToSingleton().CrossContext();
            injectionBinder.Bind<FundDataChangedSignal>().ToSingleton().CrossContext();
            injectionBinder.Bind<LocalAddFriendsInfoUtils>().ToSingleton().CrossContext();
            injectionBinder.Bind<LocalIntakeLogsDataUtils>().ToSingleton().CrossContext();
            injectionBinder.Bind<OrderReceiptSignal>().ToSingleton().CrossContext();
            injectionBinder.Bind<OrderPaymentSignal>().ToSingleton().CrossContext();
            injectionBinder.Bind<BuglyUtil>().ToSingleton().CrossContext();

            //config
            injectionBinder.Bind<ITreasureBoxConfig>().To<TreasureBoxConfig>().ToSingleton().CrossContext();
            injectionBinder.Bind<ITaskConfig>().To<TaskConfig>().ToSingleton().CrossContext();
            injectionBinder.Bind<IItemConfig>().To<ItemConfig>().ToSingleton().CrossContext();
            injectionBinder.Bind<IMissionConfig>().To<MissionConfig>().ToSingleton().CrossContext();
            injectionBinder.Bind<IPropertyConfig>().To<PropertyConfig>().ToSingleton().CrossContext();
            injectionBinder.Bind<IRoleConfig>().To<RoleConfig>().ToSingleton().CrossContext();
            injectionBinder.Bind<ILanConfig>().To<LanConfig>().ToSingleton().CrossContext();
            injectionBinder.Bind<II18NConfig>().To<I18NConfig>().ToSingleton().CrossContext();
            injectionBinder.Bind<IFontConfig>().To<FontConfig>().ToSingleton().CrossContext();
            injectionBinder.Bind<IAccessoryConfig>().To<AccessoryConfig>().ToSingleton().CrossContext();
            injectionBinder.Bind<IAwardConfig>().To<AwardConfig>().ToSingleton().CrossContext();
            injectionBinder.Bind<IActionConfig>().To<ActionConfig>().ToSingleton().CrossContext();
            injectionBinder.Bind<ISoundConfig>().To<SoundConfig>().ToSingleton().CrossContext();
            injectionBinder.Bind<ILevelConfig>().To<LevelConfig>().ToSingleton().CrossContext();
            injectionBinder.Bind<IIconManager>().To<IconManager>().ToSingleton().CrossContext();
            injectionBinder.Bind<IWWWSupport>().To<WWWSupport>().ToSingleton().CrossContext();
            injectionBinder.Bind<IAbsDoHeartbeatAction>().To<IntakeHeartbeatAction>().ToName("IntakeHeartbeat").ToSingleton().CrossContext();
            injectionBinder.Bind<IAbsDoHeartbeatAction>().To<UploadFriendsHeatbeatAction>().ToName("UploadFriendHeartbeat").ToSingleton().CrossContext();
            injectionBinder.Bind<ILocalClockInfoAgent>().To<LocalClockInfoAgent>().ToSingleton().CrossContext();
            injectionBinder.Bind<ICupClockManager>().To<CupClockManager>().ToSingleton().CrossContext();
            injectionBinder.Bind<IQRCodeUtils>().To<QRCodeUtils>().ToSingleton().CrossContext();
            injectionBinder.Bind<INativeOkHttpMethodWrapper>().To<NativeOkHttpMethodWrapper>().ToSingleton().CrossContext();

            commandBinder.Bind<StartSignal>().To<StartCommand>();
            commandBinder.Bind<StartPostGameDataCommandSignal>().To<PostGameDataCommand>();

            if (null == crossContextBridge.GetBinding(MainKeys.PowerClick))
                crossContextBridge.Bind(MainKeys.PowerClick);
            if (null == crossContextBridge.GetBinding(MainKeys.PowerHold))
                crossContextBridge.Bind(MainKeys.PowerHold);
            if (null == crossContextBridge.GetBinding(MainKeys.CupShake))
                crossContextBridge.Bind(MainKeys.CupShake);
            if (null == crossContextBridge.GetBinding(MainKeys.CupPoseChanged))
                crossContextBridge.Bind(MainKeys.CupPoseChanged);
            if (null == crossContextBridge.GetBinding(MainKeys.CupTiltLeft))
                crossContextBridge.Bind(MainKeys.CupTiltLeft);
            if (null == crossContextBridge.GetBinding(MainKeys.CupTiltRight))
                crossContextBridge.Bind(MainKeys.CupTiltRight);
            if (null == crossContextBridge.GetBinding(MainKeys.CupKeepFlat))
                crossContextBridge.Bind(MainKeys.CupKeepFlat);
            if (null == crossContextBridge.GetBinding(MainKeys.CupTapLeft))
                crossContextBridge.Bind(MainKeys.CupTapLeft);
            if (null == crossContextBridge.GetBinding(MainKeys.CupTapRight))
                crossContextBridge.Bind(MainKeys.CupTapRight);
            if (null == crossContextBridge.GetBinding(MainKeys.CupDoubleTapLeft))
                crossContextBridge.Bind(MainKeys.CupDoubleTapLeft);
            if (null == crossContextBridge.GetBinding(MainKeys.CupDoubleTapRight))
                crossContextBridge.Bind(MainKeys.CupDoubleTapRight);
            if (null == crossContextBridge.GetBinding(MainKeys.CupLeftSwipeUp))
                crossContextBridge.Bind(MainKeys.CupLeftSwipeUp);
            if (null == crossContextBridge.GetBinding(MainKeys.CupLeftSwipeDown))
                crossContextBridge.Bind(MainKeys.CupLeftSwipeDown);
            if (null == crossContextBridge.GetBinding(MainKeys.CupLeftSwipeUpDown))
                crossContextBridge.Bind(MainKeys.CupLeftSwipeUpDown);
            if (null == crossContextBridge.GetBinding(MainKeys.CupRightSwipeUp))
                crossContextBridge.Bind(MainKeys.CupRightSwipeUp);
            if (null == crossContextBridge.GetBinding(MainKeys.CupRightSwipeDown))
                crossContextBridge.Bind(MainKeys.CupRightSwipeDown);
            if (null == crossContextBridge.GetBinding(MainKeys.CupRightSwipeUpDown))
                crossContextBridge.Bind(MainKeys.CupRightSwipeUpDown);

            if (null == crossContextBridge.GetBinding(MainEvent.RefreshMainDrinkViewAfterPairEvent))
                crossContextBridge.Bind(MainEvent.RefreshMainDrinkViewAfterPairEvent);
            if (null == crossContextBridge.GetBinding(MainEvent.HeartBeatEvent))
                crossContextBridge.Bind(MainEvent.HeartBeatEvent);

            if (null == crossContextBridge.GetBinding(MainEvent.ShowNavigator))
                crossContextBridge.Bind(MainEvent.ShowNavigator);
            if (null == crossContextBridge.GetBinding(MainEvent.ResetNavigator))
                crossContextBridge.Bind(MainEvent.ResetNavigator);
            if (null == crossContextBridge.GetBinding(MainEvent.NavigatorUp))
                crossContextBridge.Bind(MainEvent.NavigatorUp);
            if (null == crossContextBridge.GetBinding(MainEvent.NavigatorDown))
                crossContextBridge.Bind(MainEvent.NavigatorDown);

            if (null == crossContextBridge.GetBinding(MainEvent.OpenView))
                crossContextBridge.Bind(MainEvent.OpenView);
            if (null == crossContextBridge.GetBinding(MainEvent.CloseView))
                crossContextBridge.Bind(MainEvent.CloseView);
            if (null == crossContextBridge.GetBinding(MainEvent.OpenGuideTip))
                crossContextBridge.Bind(MainEvent.OpenGuideTip);
            if (null == crossContextBridge.GetBinding(MainEvent.OpenDownloadPage2))
                crossContextBridge.Bind(MainEvent.OpenDownloadPage2);
            if (null == crossContextBridge.GetBinding(MainEvent.CloseCupPair))
                crossContextBridge.Bind(MainEvent.CloseCupPair);
            if (null == crossContextBridge.GetBinding(MainEvent.CloseSystemMenu))
                crossContextBridge.Bind(MainEvent.CloseSystemMenu);
            if (null == crossContextBridge.GetBinding(MainEvent.CloseMainMenu))
                crossContextBridge.Bind(MainEvent.CloseMainMenu);
            if (null == crossContextBridge.GetBinding(MainEvent.CloseCollectMenu))
                crossContextBridge.Bind(MainEvent.CloseCollectMenu);
            if (null == crossContextBridge.GetBinding(MainEvent.CloseCommingSoon))
                crossContextBridge.Bind(MainEvent.CloseCommingSoon);
            if (null == crossContextBridge.GetBinding(MainEvent.ClosePetPage))
                crossContextBridge.Bind(MainEvent.ClosePetPage);
            if (null == crossContextBridge.GetBinding(MainEvent.ClosePetFeed))
                crossContextBridge.Bind(MainEvent.ClosePetFeed);
            if (null == crossContextBridge.GetBinding(MainEvent.ClosePetDress))
                crossContextBridge.Bind(MainEvent.ClosePetDress);
            if (null == crossContextBridge.GetBinding(MainEvent.CloseAnimalPlayer))
                crossContextBridge.Bind(MainEvent.CloseAnimalPlayer);
            if (null == crossContextBridge.GetBinding(MainEvent.CloseTmall))
                crossContextBridge.Bind(MainEvent.CloseTmall);
            if (null == crossContextBridge.GetBinding(MainEvent.CloseNewsEvent))
                crossContextBridge.Bind(MainEvent.CloseNewsEvent);
            if (null == crossContextBridge.GetBinding(MainEvent.CloseAlarmClock))
                crossContextBridge.Bind(MainEvent.CloseAlarmClock);
            if (null == crossContextBridge.GetBinding(MainEvent.CloseMissionBrower))
                crossContextBridge.Bind(MainEvent.CloseMissionBrower);
            if (null == crossContextBridge.GetBinding(MainEvent.CloseAlertBoard))
                crossContextBridge.Bind(MainEvent.CloseAlertBoard);
            if (null == crossContextBridge.GetBinding(MainEvent.CloseNoviceGuide))
                crossContextBridge.Bind(MainEvent.CloseNoviceGuide);
            if (null == crossContextBridge.GetBinding(MainEvent.CloseSystemMenuGuide))
                crossContextBridge.Bind(MainEvent.CloseSystemMenuGuide);
            if (null == crossContextBridge.GetBinding(MainEvent.CloseLanguageSetting))
                crossContextBridge.Bind(MainEvent.CloseLanguageSetting);
            if (null == crossContextBridge.GetBinding(MainEvent.CloseDownloadPage))
                crossContextBridge.Bind(MainEvent.CloseDownloadPage);
            if (null == crossContextBridge.GetBinding(MainEvent.CloseWorldMap))
                crossContextBridge.Bind(MainEvent.CloseWorldMap);
            if (null == crossContextBridge.GetBinding(MainEvent.CloseGuideTip))
                crossContextBridge.Bind(MainEvent.CloseGuideTip);
            if (null == crossContextBridge.GetBinding(MainEvent.CloseDownloadPage2))
                crossContextBridge.Bind(MainEvent.CloseDownloadPage2);
        }
    }
}
