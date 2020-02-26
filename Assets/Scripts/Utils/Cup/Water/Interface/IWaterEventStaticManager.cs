using CupSdk.BaseSdk.Water;
using Gululu.LocalData.DataManager;

namespace Cup.Utils.Water
{
    public interface IWaterEventManager
    {

        IIntakeLogDataManager mIntakeLogDataManager{get;set;}
        void Init();

        void startMonitor();

        void stopMonitor();
        void AddDrinkWaterCallBack(DrinkWater callBack);

        void RemoveDrinkWaterCallBack(DrinkWater callBack);

        void AddStartDrinkCallBack(StartDrink callCack);

        void RemoveStartDrinkCallBack(StartDrink callCack);

        void AddStopDrinkCallBack(StopDrink callCack);

        void RemoveStopDrinkCallBack(StopDrink callCack);
    }
}