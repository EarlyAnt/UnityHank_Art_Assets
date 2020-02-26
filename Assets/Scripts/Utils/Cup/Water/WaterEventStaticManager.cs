using CupSdk.BaseSdk.Water;
using Gululu.LocalData.DataManager;
using Gululu.Util;
using UnityEngine;

namespace Cup.Utils.Water
{
    public class WaterEventManager : IWaterEventManager
    {
        private WaterManager mWaterManager;

        private event DrinkWater DrinkWaterCallBackEvent = delegate(int intake, long drinkDuration){};

        private event StartDrink StartDrinkCallBackEvent = delegate(){};

        private event StopDrink StopDrinkCallBackEvent = delegate(long currentDuration, long totalDuration){};

        [Inject]
        public IIntakeLogDataManager mIntakeLogDataManager{get;set;}

        
        public void Init(){
            if(mWaterManager == null){
                Debug.Log("<><>WaterEventStaticManager<>Init<>");
                mWaterManager = new WaterManager(DrinkWaterCallBack);
                mWaterManager.registerDrinkStateListener(StartDrinkCallBack,StopDrinkCallBack);
            }
            
        }

        public void startMonitor(){
            Debug.Log("<><>WaterEventStaticManager<>startMonitor<>");
            mWaterManager.startMonitor();
        }

        public void stopMonitor(){
            Debug.Log("<><>WaterEventStaticManager<>stopMonitor<>");
            mWaterManager.stopMonitor();
        }

        private void DrinkWaterCallBack(int intake, long drinkDuration){
            Loom.QueueOnMainThread(() =>
            {
                Debug.Log("<><>DrinkWaterCallBack<><>");
                DrinkWaterCallBackEvent(intake,drinkDuration);
                if(DrinkUtils.isValidDrink(intake,drinkDuration)){
                      mIntakeLogDataManager.addIntakeLog("TYDK",intake,DateUtil.GetTimeStamp());
                }
               
            });
        }

        private void StartDrinkCallBack(){
            Loom.QueueOnMainThread(() =>
            {
                Debug.Log("<><>StartDrinkCallBack<><>");
                StartDrinkCallBackEvent();
            });
        }

        private void StopDrinkCallBack(long currentDuration, long totalDuration){
            Loom.QueueOnMainThread(() =>
            {
                Debug.Log("<><>StopDrinkCallBack<><>");
                StopDrinkCallBackEvent(currentDuration,totalDuration);
            });
        }

        public void AddDrinkWaterCallBack(DrinkWater callBack){
            DrinkWaterCallBackEvent += callBack;
        }

        public void RemoveDrinkWaterCallBack(DrinkWater callBack){
            DrinkWaterCallBackEvent -= callBack;
        }

        public void AddStartDrinkCallBack(StartDrink callCack){
            StartDrinkCallBackEvent += callCack;
        }

        public void RemoveStartDrinkCallBack(StartDrink callCack){
            StartDrinkCallBackEvent -= callCack;
        }

        public void AddStopDrinkCallBack(StopDrink callCack){
            StopDrinkCallBackEvent += callCack;
        }

        public void RemoveStopDrinkCallBack(StopDrink callCack){
            StopDrinkCallBackEvent -= callCack;
        }

    }
}