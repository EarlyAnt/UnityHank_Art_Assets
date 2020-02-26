using Cup.Utils.android;
using CupSdk.BaseSdk.Water;
using UnityEngine;

namespace Cup.Utils.Water
{
    public class WaterManager
    {
        private DrinkMotion drinkMotion;
        private AndroidJavaObject context;
        public WaterManager(DrinkWater drinkWater){
            drinkMotion = new DrinkMotion(drinkWater); 
            context = AndroidContextHolder.GetAndroidContext();
        }

        public void registerDrinkStateListener(StartDrink startDrink,StopDrink stopDrink){
            drinkMotion.registerDrinkStateListener(startDrink,stopDrink);
        }

        public void unregisterDrinkStateListener(){
            drinkMotion.unregisterDrinkStateListener();
        }

        public void stopMonitor(){
            drinkMotion.stopMonitor();
        }

        public void startMonitor(){
            drinkMotion.startMonitor();
        }
    }
}