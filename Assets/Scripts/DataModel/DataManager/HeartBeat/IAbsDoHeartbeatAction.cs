namespace Gululu.LocalData.DataManager
{
    public interface IAbsDoHeartbeatAction
    {
        void setHeartbeatCountDownLatch(HeartbeatCountDownLatch heartbeatCountDownLatch);
        void onStart();

        void onFinish();
    }
}