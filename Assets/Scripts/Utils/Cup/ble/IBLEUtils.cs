namespace Cup.Utils.ble
{
    public interface IBLEUtils
    {
        void init();
        void StartPairBLE(BlePairCallbackSuccess success, BlePairCallbackError error);

        void StopPairBLE();

        bool ISBLEEnable();

        void SetBleEnable(bool isEnable);

        void sendPairState(string pairStatus);

        void sendPairState(PairMessage message);
    }
}