namespace Gululu.LocalData.DataManager
{
    public interface IHeartbeatActionManager
    {
        void startHeartbeat();

        void addEventListener(IAbsDoHeartbeatAction mNetNotify);

        void removeEventListener(IAbsDoHeartbeatAction mNetNotify);
    }
}