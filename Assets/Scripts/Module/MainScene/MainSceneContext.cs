using Gululu;
using Gululu.Events;
using Gululu.LocalData.DataManager;
using Gululu.net;
using Gululu.Util;
using Hank.Api;
using strange.extensions.command.api;
using strange.extensions.command.impl;
using strange.extensions.context.api;
using strange.extensions.context.impl;
using strange.extensions.signal.api;
using UnityEngine;

namespace Hank.MainScene
{
    public class MainSceneContext : MVCSContext
    {
        public MainSceneContext(MonoBehaviour view) : base(view)
        {
        }

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

        public MainSceneContext(MonoBehaviour view, ContextStartupFlags flags) : base(view, flags)
        {
        }

        protected override void mapBindings()
        {
            mediationBinder.Bind<MainSceneView>().To<MainSceneMediator>();
            mediationBinder.Bind<NavigatorView>().To<NavigatorMediator>();
            mediationBinder.Bind<HeartBeatView>().To<HeartBeatMediator>();

            injectionBinder.Bind<CloseSystemMenuSignal>().ToSingleton().CrossContext();
            injectionBinder.Bind<SetValuesLongOkSignal>().ToSingleton().CrossContext();
            injectionBinder.Bind<SilenceTimeProcessSignal>().ToSingleton().CrossContext();
            injectionBinder.Bind<SleepModeTimeInSignal>().ToSingleton();
            injectionBinder.Bind<SchoolModeTimeInSignal>().ToSingleton();
            injectionBinder.Bind<SleepModeTimeOutSignal>().ToSingleton();
            injectionBinder.Bind<SchoolModeTimeOutSignal>().ToSingleton();
            //injectionBinder.Bind<CloseTmallSignal>().ToSingleton();
            injectionBinder.Bind<IBaseSignal>().To<FreshNewsSignal>().ToName("FreshNewsSignal").ToSingleton().CrossContext();
            injectionBinder.Bind<ProcessFreshNewsSignal>().ToSingleton().CrossContext();
            injectionBinder.Bind<HideTextTmallViewSignal>().ToSingleton().CrossContext();
            commandBinder.Bind<SyncAllGameDataSignal>().To<SyncAllGameDataCommand>();
            injectionBinder.Bind<UIStateCompleteSignal>().ToSingleton().CrossContext();
            injectionBinder.Bind<RolePlayerPlayActionSignal>().ToSingleton().CrossContext();
            injectionBinder.Bind<CloseTreasureBoxSignal>().ToSingleton().CrossContext();
            injectionBinder.Bind<PauseNoviceGuideSoundSignal>().ToSingleton().CrossContext();
            injectionBinder.Bind<ShowPetPlayerSignal>().ToSingleton().CrossContext();
            injectionBinder.Bind<ShowClockSignal>().ToSingleton().CrossContext();
            injectionBinder.Bind<LevelUpSignal>().ToSingleton().CrossContext();
            injectionBinder.Bind<UpdateExpAndCoinSignal>().ToSingleton().CrossContext();
            injectionBinder.Bind<ChangeRoleSignal>().ToSingleton().CrossContext();
            
            injectionBinder.Bind<IAbsDoHeartbeatAction>().To<GameHeartbeatAction>().ToName("GameHeartbeatAction").ToSingleton();
            injectionBinder.Bind<INewsEventManager>().To<NewsEventManager>().ToSingleton().CrossContext();
            injectionBinder.Bind<ISpriteDownloadManager>().To<SpriteDownloadManager>().ToSingleton().CrossContext();
            injectionBinder.Bind<IAudioDownloadManager>().To<AudioDownloadManager>().ToSingleton().CrossContext();
            injectionBinder.Bind<IVolumeSetting>().To<VolumeSetting>().ToSingleton().CrossContext();
            injectionBinder.Bind<IInteractiveCountManager>().To<InteractiveCountManager>().ToSingleton().CrossContext();
            injectionBinder.Bind<ISleepTimeManager>().To<SleepTimeManager>().ToSingleton().CrossContext();

            injectionBinder.Bind<IHeartBeatModel>().To<HeartBeatModel>().ToSingleton();
            injectionBinder.Bind<IHeartBeatService>().To<HeartBeatService>().ToSingleton();
            injectionBinder.Bind<ServiceHeartBeatBackSignal>().ToSingleton();
            injectionBinder.Bind<ServiceHeartBeatErrBackSignal>().ToSingleton();
            injectionBinder.Bind<MediatorHeartBeatSignal>().ToSingleton();
            injectionBinder.Bind<MediatorHeartBeatErrSignal>().ToSingleton();
            commandBinder.Bind<StartHeartBeatCommandSignal>().To<HeartBeatCommand>();

            injectionBinder.Bind<IHeatBeatSendBackAckModel>().To<HeatBeatSendBackAckModel>().ToSingleton();
            injectionBinder.Bind<IHeatBeatSendBackAckService>().To<HeatBeatSendBackAckService>().ToSingleton();
            injectionBinder.Bind<ServiceHeatBeatSendBackAckBackSignal>().ToSingleton();
            injectionBinder.Bind<ServiceHeatBeatSendBackAckErrBackSignal>().ToSingleton();
            injectionBinder.Bind<MediatorHeatBeatSendBackAckSignal>().ToSingleton();
            injectionBinder.Bind<MediatorHeatBeatSendBackAckErrSignal>().ToSingleton();
            commandBinder.Bind<StartHeatBeatSendBackAckCommandSignal>().To<HeatBeatSendBackAckCommand>();

            injectionBinder.Bind<IPostGameDataModel>().To<PostGameDataModel>().ToSingleton().CrossContext();
            injectionBinder.Bind<IPostGameDataService>().To<PostGameDataService>().ToSingleton().CrossContext();
            injectionBinder.Bind<ServicePostGameDataBackSignal>().ToSingleton().CrossContext();
            injectionBinder.Bind<ServicePostGameDataErrBackSignal>().ToSingleton().CrossContext();
            injectionBinder.Bind<MediatorPostGameDataSignal>().ToSingleton().CrossContext(); ;
            injectionBinder.Bind<MediatorPostGameDataErrSignal>().ToSingleton().CrossContext(); ;
            commandBinder.Bind<StartPostGameDataCommandSignal>().To<PostGameDataCommand>();

            injectionBinder.Bind<ITmallAuthenModel>().To<TmallAuthenModel>().ToSingleton();
            injectionBinder.Bind<ITmallAuthenService>().To<TmallAuthenService>().ToSingleton();
            injectionBinder.Bind<ServiceTmallAuthBackSignal>().ToSingleton();
            injectionBinder.Bind<ServiceTmallAuthErrBackSignal>().ToSingleton();
            injectionBinder.Bind<MediatorTmallAuthSignal>().ToSingleton();
            injectionBinder.Bind<MediatorTmallAuthErrSignal>().ToSingleton();
            commandBinder.Bind<StartTmallAuthCommandSignal>().To<TmallAuthCommand>();
            /*Insert Bingder Here*/

            commandBinder.Bind<StartSignal>().To<StartCommand>();            
        }
    }
}