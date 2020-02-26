using System;

namespace CupSdk.BaseSdk.Water
{
    public class DrinkUtils
    {
        public static bool isValidDrink(int intakeData, long drinkDuration){
            if (intakeData == 0){
                return false;
            }
            double inTime;
            double mvps = 25.0;

            if (intakeData <= 56.0){
                inTime = intakeData/50.0;
            }else{
                inTime = (intakeData - 56.0)/mvps + (56/50.0); 
            }
            if (inTime * 1000 <= drinkDuration){
                return true;
            }
            return false; 
        } 
    }
}