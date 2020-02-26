using System;

namespace Gululu.LocalData.DataManager
{
    public class UploadFriendsHeatbeatAction : AbsDoHeartbeatAction
    {
        [Inject]
        public IAddFriendsInfoManager mAddFriendsInfoManager{get;set;}
        public override void onStart()
        {
            GuLog.Info("<><UploadFriendsHeatbeatAction> Time:" + DateTime.Now.ToShortTimeString());
            mAddFriendsInfoManager.upload((result)=>{
                onFinish();
            },(errorInfo)=>{
                onFinish();
            });
        }
    }
}