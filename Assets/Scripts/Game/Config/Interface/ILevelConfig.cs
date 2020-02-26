using System;

namespace Gululu.Config
{
    public interface ILevelConfig
    {
        void LoadLevelConfig();
        int GetExpForLevelUp(int level);
        bool IsLoadedOK();

        int GetHunger(int level);
        int GetLevelCoin(int level);
        int GetDrinkWaterCoin(int level, int drinkTimes);
        int GetDrinkWaterExpNormal(int level);
        int GetDrinkWaterExpGoal(int level);
        [Obsolete]
        int GetDailyGoalCoin(int level, float percent);
        DailyGoal GetLevelDailyGoal(int level, float percent);
        int GetDailyGoalExp(int level, float percent);
    }
}
