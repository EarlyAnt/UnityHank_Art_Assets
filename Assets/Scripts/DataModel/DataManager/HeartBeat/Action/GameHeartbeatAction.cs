using Gululu.LocalData.DataManager;
using System;

namespace Hank.MainScene
{

    public class GameHeartbeatAction : AbsDoHeartbeatAction
    {
        [Inject]
        public StartHeartBeatCommandSignal startHeartBeatCommandSignal { get; set; }
        [Inject]
        public SyncAllGameDataSignal syncAllGameDataSingal { get; set; }

        public override void onStart()
        {
            if (TopViewHelper.Instance.IsInGuideView())
            {
                return;
            }

            GuLog.Info("<><GameHeartbeatAction> Time:" + DateTime.Now.ToShortTimeString());
            //syncAllGameDataSingal.Dispatch();
            startHeartBeatCommandSignal.Dispatch();
        }
    }
}