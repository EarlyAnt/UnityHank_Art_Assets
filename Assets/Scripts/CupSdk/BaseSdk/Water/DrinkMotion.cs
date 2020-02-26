using Cup.Utils.android;
using UnityEngine;

namespace CupSdk.BaseSdk.Water
{
    public class DrinkMotion
    {
        private AndroidJavaObject javaDrinkMotion;
        private DrinkStateListener drinkStateListener;
        public DrinkMotion(DrinkWater drinkWater){
            DrinkMotionCallback drinkMotionCallback = new DrinkMotionCallback();
            drinkMotionCallback.addCallback(drinkWater);
            javaDrinkMotion = new AndroidJavaObject("com.bowhead.sheldon.water.DrinkMotion", AndroidContextHolder.GetAndroidContext(),drinkMotionCallback);
            
        }

        public void registerDrinkStateListener(StartDrink startDrink,StopDrink stopDrink){
            drinkStateListener = new DrinkStateListener();
            drinkStateListener.addCallback(startDrink, stopDrink);

            javaDrinkMotion.Call("registerDrinkStateListener",drinkStateListener);
        }

        public void unregisterDrinkStateListener(){
            javaDrinkMotion.Call("unregisterDrinkStateListener");
        }

        public void stopMonitor(){
            javaDrinkMotion.Call("stopMonitor");
        }

        public bool startMonitor(){
            return javaDrinkMotion.Call<bool>("startMonitor");
        }
    }



    



}