using Hank;
using Mono.Xml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;

namespace Gululu.Config
{
    public class LevelConfig : AbsConfigBase, ILevelConfig
    {
        public Dictionary<int, LevelInfo> levelLists = new Dictionary<int, LevelInfo>();

        public void LoadLevelConfig()
        {
            if (m_LoadOk)
                return;
            levelLists.Clear();
            ConfigLoader.Instance.LoadConfig("Config/LevelConfig.xml", Parser);
            //            StartCoroutine(_LoadLevelConfig());
        }

        private void Parser(WWW www)
        {
            try
            {
                GuLog.Debug("LevelConfigPath WWW::" + www.error);

                if (www.isDone && (www.error == null || www.error.Length == 0))
                {
                    SecurityParser xmlDoc = new SecurityParser();
                    GuLog.Debug("LevelConfigPath" + www.text);

                    xmlDoc.LoadXml(www.text);
                    ArrayList worldXmlList = xmlDoc.ToXml().Children;

                    foreach (SecurityElement xeRoot in worldXmlList)
                    {
                        if (xeRoot.Tag == "Levels")
                        {
                            ArrayList levelsXmlList = xeRoot.Children;
                            foreach (SecurityElement xeLevel in levelsXmlList)
                            {
                                if (xeLevel.Tag == "LevelUp")
                                {
                                    LevelInfo levelInfo = new LevelInfo();
                                    levelInfo.level = Convert.ToInt32(xeLevel.Attribute("level"));
                                    levelInfo.exp = Convert.ToInt32(xeLevel.Attribute("exp"));
                                    levelInfo.hunger = Convert.ToInt32(xeLevel.Attribute("hunger"));
                                    levelInfo.coinDL = Convert.ToInt32(xeLevel.Attribute("coinDL"));
                                    levelInfo.coinUL = Convert.ToInt32(xeLevel.Attribute("coinUL"));
                                    ArrayList awardXmlList = xeLevel.Children;
                                    foreach (SecurityElement xeAward in awardXmlList)
                                    {
                                        switch (xeAward.Tag)
                                        {
                                            case "DrinkWater":
                                                levelInfo.awardList.Add(new DrinkWater()
                                                {
                                                    CoinDL = Convert.ToInt32(xeAward.Attribute("coinDL")),
                                                    CoinUL = Convert.ToInt32(xeAward.Attribute("coinUL")),
                                                    ExpNormal = Convert.ToInt32(xeAward.Attribute("expN")),
                                                    ExpGoal = Convert.ToInt32(xeAward.Attribute("expG")),
                                                    Rate30 = (float)Convert.ToDouble(xeAward.Attribute("rate30")),
                                                    RateMore = (float)Convert.ToDouble(xeAward.Attribute("rateMore"))
                                                });
                                                break;
                                            case "DailyGoal":
                                                levelInfo.awardList.Add(new DailyGoal()
                                                {
                                                    Percent = (float)Convert.ToDouble(xeAward.Attribute("percent")),
                                                    Coin = Convert.ToInt32(xeAward.Attribute("coin")),
                                                    Award = Convert.ToInt32(xeAward.Attribute("award")),
                                                    ExpDL = Convert.ToInt32(xeAward.Attribute("expDL")),
                                                    ExpUL = Convert.ToInt32(xeAward.Attribute("expUL"))
                                                });
                                                break;
                                        }
                                    }
                                    levelLists.Add(levelInfo.level, levelInfo);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogErrorFormat("<><LevelConfig.Parser>Error: {0}", ex.Message);
            }
            m_LoadOk = true;
        }

        public int GetExpForLevelUp(int level)
        {
            if (levelLists.ContainsKey(level))
                return this.levelLists[level].exp;
            else
                return 30;
        }

        public int GetHunger(int level)
        {
            return levelLists.ContainsKey(level) ? this.levelLists[level].hunger : int.MaxValue;
        }

        public int GetLevelCoin(int level)
        {
            if (levelLists.ContainsKey(level))
                return UnityEngine.Random.Range(this.levelLists[level].coinDL, this.levelLists[level].coinUL);
            else
                return 0;
        }

        public int GetDrinkWaterCoin(int level, int drinkTimes)
        {
            if (levelLists.ContainsKey(level))
            {
                DrinkWater drinkWater = this.levelLists[level].awardList.Find(t => t.GetType() == typeof(DrinkWater)) as DrinkWater;
                if (drinkWater != null)
                {
                    float rate = drinkTimes <= 50 ? drinkWater.Rate30 : drinkWater.RateMore;//根据当天的喝水次数，选取金币奖励的概率
                    rate = Mathf.Clamp01(rate);//格式化概率的小数为0～1之间
                    int target = (int)(rate * 100);//将概率的取值范围转成100以内的某个数字
                    int seed = UnityEngine.Random.Range(1, 100);//从1～100之间随机一个数字
                    int coin = seed <= target ? UnityEngine.Random.Range(drinkWater.CoinDL, drinkWater.CoinUL) : 0;//seed小于等于target，视为在该概率下，中奖了，否则为没中奖
                    Debug.LogFormat("<><LevelConfig.GetDrinkWaterCoin>DrinkTimes: {0}, Target: {1}, Seed: {2}, Coin: {3}", drinkTimes, target, seed, coin);
                    return coin;
                }
                else return 0;
            }
            else return 0;
        }

        public int GetDrinkWaterExpNormal(int level)
        {
            if (levelLists.ContainsKey(level))
            {
                DrinkWater drinkWater = this.levelLists[level].awardList.Find(t => t.GetType() == typeof(DrinkWater)) as DrinkWater;
                return drinkWater != null ? drinkWater.ExpNormal : 0;
            }
            else return 0;
        }

        public int GetDrinkWaterExpGoal(int level)
        {
            if (levelLists.ContainsKey(level))
            {
                DrinkWater drinkWater = this.levelLists[level].awardList.Find(t => t.GetType() == typeof(DrinkWater)) as DrinkWater;
                return drinkWater != null ? drinkWater.ExpGoal : 0;
            }
            else return 0;
        }

        [Obsolete]
        public int GetDailyGoalCoin(int level, float percent)
        {
            if (levelLists.ContainsKey(level))
            {
                DailyGoal dailyGoal = this.levelLists[level].awardList.Find(t => t.GetType() == typeof(DailyGoal) && ((DailyGoal)t).Percent == percent) as DailyGoal;
                return dailyGoal != null ? UnityEngine.Random.Range(dailyGoal.Coin, dailyGoal.Award) : 0;
            }
            else return 0;
        }

        public DailyGoal GetLevelDailyGoal(int level, float percent)
        {
            return levelLists.ContainsKey(level) ? this.levelLists[level].awardList.Find(t => t.GetType() == typeof(DailyGoal) && ((DailyGoal)t).Percent == percent) as DailyGoal : null;
        }

        public int GetDailyGoalExp(int level, float percent)
        {
            if (levelLists.ContainsKey(level))
            {
                DailyGoal dailyGoal = this.levelLists[level].awardList.Find(t => t.GetType() == typeof(DailyGoal) && ((DailyGoal)t).Percent == percent) as DailyGoal;
                return dailyGoal != null ? UnityEngine.Random.Range(dailyGoal.ExpDL, dailyGoal.ExpUL) : 0;
            }
            else return 0;
        }
    }

    #region 自定义类
    public class LevelInfo
    {
        public int level { get; set; }
        public int exp { get; set; }
        public int hunger { get; set; }
        public int coinDL { get; set; }
        public int coinUL { get; set; }
        public List<AwardBase> awardList { get; set; }
        public LevelInfo()
        {
            this.awardList = new List<AwardBase>();
        }
    }
    public abstract class AwardBase
    {
    }
    public class DrinkWater : AwardBase
    {
        public int CoinDL { get; set; }
        public int CoinUL { get; set; }
        public int ExpNormal { get; set; }
        public int ExpGoal { get; set; }
        public float Rate30 { get; set; }
        public float RateMore { get; set; }
    }
    public class DailyGoal : AwardBase
    {
        public float Percent { get; set; }
        public int Coin { get; set; }
        public int Award { get; set; }
        public int ExpDL { get; set; }
        public int ExpUL { get; set; }
    }
    #endregion
}
