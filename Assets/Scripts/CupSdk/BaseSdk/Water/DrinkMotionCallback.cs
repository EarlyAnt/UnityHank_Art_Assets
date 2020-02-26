using UnityEngine;

namespace CupSdk.BaseSdk.Water
{
    public class DrinkMotionCallback : AndroidJavaProxy
    {
        public DrinkMotionCallback() : base("com.bowhead.sheldon.water.DrinkMotion$Callback"){}

        public void onDrinkWater(int intake, long drinkDuration){
            if (mDrinkWater != null) {
                Debug.Log("<><>DrinkMotionCallback onDrinkWater<><>");
//                 if(DrinkUtils.isValidDrink(intake,drinkDuration))
                {
                    mDrinkWater(intake, drinkDuration);
                }
            }
        }
        private DrinkWater mDrinkWater;
        public void addCallback(DrinkWater drinkWater){
            mDrinkWater = drinkWater;
        }
    }


    public delegate void DrinkWater(int intake, long drinkDuration);
}