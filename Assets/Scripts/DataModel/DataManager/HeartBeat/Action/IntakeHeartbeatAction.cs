using System;

namespace Gululu.LocalData.DataManager
{
    public class IntakeHeartbeatAction : AbsDoHeartbeatAction
    {
        [Inject]
        public IIntakeLogDataManager mIntakeLogDataManager{get;set;}

        public override void onStart()
        {
             GuLog.Info("<><IntakeHeartbeatAction> Time:" + DateTime.Now.ToShortTimeString());
            mIntakeLogDataManager.upload((result)=>{
                onFinish();
            },(errorInfo)=>{
                onFinish();
            });
        }
    }
}