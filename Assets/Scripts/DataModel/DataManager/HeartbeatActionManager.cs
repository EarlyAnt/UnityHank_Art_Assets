using System;
using System.Collections.Generic;
using Cup.Utils.android;

namespace Gululu.LocalData.DataManager
{
    public class HeartbeatActionManager : IHeartbeatActionManager
    {
        [Inject]
        public IHeartbeatManager mHeartbeatManager{set;get;}
        private List<IAbsDoHeartbeatAction> mHeartbeatActions = new List<IAbsDoHeartbeatAction>();
        
        public void startHeartbeat()
        {
            int size = mHeartbeatActions.Count;
            GuLog.Info("HeartbearActionManager size: "+size);
            if(size <= 0){
                mHeartbeatManager.notifyHeartBeatDone();
            }else{
                handleHeartBeat(size);
            }
        }

        private void handleHeartBeat(int size)
        {
            GuLog.Info("HeartbearActionManager mHeartbeatActions");
            HeartbeatCountDownLatch mHeartbeatCountDownLatch = new HeartbeatCountDownLatch();
            mHeartbeatCountDownLatch.SetCount(size);
            mHeartbeatCountDownLatch.setAction(()=>{
                mHeartbeatManager.notifyHeartBeatDone();
            });
            for(int i = 0; i < size; i++){
                IAbsDoHeartbeatAction action = mHeartbeatActions[i];
                action.setHeartbeatCountDownLatch(mHeartbeatCountDownLatch);
                action.onStart();
            }
        }

        public void addEventListener(IAbsDoHeartbeatAction mNetNotify)
        {
            mHeartbeatActions.Add(mNetNotify);
        }

        public void removeEventListener(IAbsDoHeartbeatAction mNetNotify)
        {
            mHeartbeatActions.Remove(mNetNotify);
        }
    }
    


}