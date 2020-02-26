using UnityEngine;

namespace CupSdk.BaseSdk.Water
{
    public class DrinkStateListener : AndroidJavaProxy
    {
        public DrinkStateListener() : base("com.bowhead.sheldon.water.DrinkMotion$DrinkStateListener"){}

        public void onStartDrink(){
            if (mStartDrink != null) {
                Debug.Log("<><>DrinkStateListener onStartDrink<><>");
                mStartDrink();
            }
               
        }

        public void onStopDrink(long currentDuration, long totalDuration){
            if (mStopDrink != null) {
                Debug.Log("<><>DrinkStateListener onStopDrink<><>");
                mStopDrink(currentDuration, totalDuration);
            }
               
        }

        private StartDrink mStartDrink;
        private StopDrink mStopDrink;
        public void addCallback(StartDrink startDrink,StopDrink stopDrink){
            mStartDrink = startDrink;
            mStopDrink = stopDrink;
        }
    }

    public delegate void StartDrink();

    public delegate void StopDrink(long currentDuration, long totalDuration);

}