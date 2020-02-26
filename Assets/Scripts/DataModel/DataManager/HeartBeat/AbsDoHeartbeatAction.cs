namespace Gululu.LocalData.DataManager
{
    public abstract class AbsDoHeartbeatAction : IAbsDoHeartbeatAction
    {

        public HeartbeatCountDownLatch mHeartbeatCountDownLatch;

        public void setHeartbeatCountDownLatch(HeartbeatCountDownLatch heartbeatCountDownLatch)
        {
            mHeartbeatCountDownLatch = heartbeatCountDownLatch;
        }

        public abstract void onStart();

        public void onFinish()
        {
            if (mHeartbeatCountDownLatch != null)
            {
                mHeartbeatCountDownLatch.Done();
                mHeartbeatCountDownLatch = null;
            }
        }
    }
}