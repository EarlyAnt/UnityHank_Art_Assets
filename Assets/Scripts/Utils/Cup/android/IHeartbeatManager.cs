using UnityHank.Assets.Script.Utils.Cup.android;

namespace Cup.Utils.android
{
    public interface IHeartbeatManager
    {
        void setHeartbeatListener(HeartBeatReveive results);

        void notifyHeartBeatDone();
    }
}