namespace Gululu.LocalData.DataManager
{
    public class HeartbeatCountDownLatch
    {
        
        private object mObject = new object();
        private int mCount;
        private DoAction mDoAction;
        public void SetCount(int count)
        {
            mCount = count;
        }

        public void Done()
        {
            lock (mObject)
            {
                mCount--;

                if (mCount == 0 && mDoAction != null)
                {
                    mDoAction();
                }
            }

            GuLog.Info("HeartbeatCountDownLatch count: "+mCount);

        }

        public void setAction(DoAction doAction)
        {
            mDoAction = doAction;
        }
    }

    public delegate void DoAction();
}