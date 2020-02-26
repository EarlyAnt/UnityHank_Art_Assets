using Gululu.Config;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Hank
{
    /// <summary>
    /// 奖品产出计算工具类
    /// </summary>
    public class AwardDataHelper : IAwardDataHelper
    {
        [Inject]
        public IAwardConfig AwardConfig { get; set; }
        [Inject]
        public IPropertyConfig PropertyConfig { get; set; }
        [Inject]
        public IFundDataManager FundDataManager { get; set; }
        [Inject]
        public IItemsDataManager ItemDataManager { get; set; }
        [Inject]
        public IPlayerDataManager PlayerDataManager { get; set; }

        /// <summary>
        /// 根据指定的总价格及产出规则，计算获得的金币数及道具数量
        /// </summary>
        /// <param name="totalCost">产出物总价格</param>
        /// <param name="awardID"></param>
        /// <returns>返回计算得出的奖品列表，或返回null</returns>
        public AwardDatas GetAwards(int totalCost, int awardID)
        {
            AwardInfo awardInfo = this.AwardConfig.GetAward(awardID);
            if (awardInfo != null)
            {
                Debug.LogFormat("<><AwardDataHelper.GetAwards>AwardInfo: {0}", awardInfo);
                AwardDatas awardDatas = new AwardDatas();
                try
                {
                    foreach (var awardConfig in awardInfo.Awards)
                    {
                        if (awardConfig is CoinAward)
                        {
                            awardDatas.Datas.Add(new AwardData()
                            {
                                ID = this.FundDataManager.GetCoinId(),
                                Count = (int)(totalCost * awardConfig.Percent),
                                Icon = "coin",
                                AwardType = AwardTypes.COIN
                            });
                        }
                        else if (awardConfig is PropAward)
                        {
                            PropAward propAward = awardConfig as PropAward;//转成道具奖品产出规则
                            int cost = (int)(totalCost * propAward.Percent);//计算道具奖品的总价格
                            List<AwardData> sourceData = new List<AwardData>();
                            if (propAward.FoodAward)
                            {//将食物表中的所有数据转成统一格式
                                List<PropertyItem> foods = this.PropertyConfig.GetPropertyList();
                                sourceData.AddRange(this.PropertyItemList2AwardDataList(foods));
                            }

                            if (propAward.MaterialAward)
                            {//将配饰表中所有的材料数据转成统一格式

                            }

                            List<AwardData> propAwards = this.GetPropAwards(cost, sourceData);//根据金币数，从源数据中随机取出1～2个奖品
                            if (propAwards != null && propAwards.Count > 0)
                                awardDatas.Datas.AddRange(propAwards);
                        }
                    }

                    if (awardDatas.Datas.Count == 0)
                        Debug.LogErrorFormat("<><AwardDataHelper.GetAwards>Award config dosen't work, you should get a few coins as least. (totalCost: {0}, awardID: {1})", totalCost, awardID);

                    this.PrintAwardDatas(awardDatas);
                    return awardDatas;
                }
                catch (System.Exception ex)
                {
                    Debug.LogErrorFormat("<><AwardDataHelper.GetAwards>Unknown error: {0}", ex.Message);
                    return null;
                }
            }
            else
            {
                Debug.LogErrorFormat("<><AwardDataHelper.GetAwards>Can't find the award config: {0}", awardID);
                return null;
            }
        }
        /// <summary>
        /// 从源数据列表中，随机取出1～2个价格小于等于金币数，或不大于金币数110%的奖品
        /// </summary>
        /// <param name="coins">金币数</param>
        /// <param name="sourceData">源数据(包括从食物表，配饰表取出的，并转成统一格式AwardData的数据列表)</param>
        /// <returns>返回计算得出的奖品列表，或返回null</returns>
        private List<AwardData> GetPropAwards(int coins, List<AwardData> sourceData)
        {
            if (coins <= 0)
            {
                throw new System.Exception("<><AwardDataHelper.GetPropAwards>Paramater 'cost' must not be less than zero");
            }
            else if (sourceData == null || sourceData.Count == 0)
            {
                throw new System.Exception("<><AwardDataHelper.GetPropAwards>Paramater 'sourceData' is null or its length is zero");
            }

            int leftCost = coins;
            List<AwardData> result = new List<AwardData>();
            List<AwardData> moreExpensiveAwards = sourceData.FindAll(t => t.Price >= this.PropertyConfig.GetMedianPrice());//从列表中取出价格大于等于中位数价格的奖品
            List<AwardData> lessExpensiveAwards = sourceData.FindAll(t => t.Price < this.PropertyConfig.GetMedianPrice());//从列表中取出价格小于中位数价格的奖品
            List<AwardData> resultMoreExpensiveAwards = this.FindPropAwards(coins, moreExpensiveAwards, ref leftCost);//先按照原始的金币数取出贵的奖品
            if (resultMoreExpensiveAwards != null && resultMoreExpensiveAwards.Count > 0) result.AddRange(resultMoreExpensiveAwards);
            if (leftCost > 0)
            {
                List<AwardData> resultLessExpensiveAwards = this.FindPropAwards(leftCost, lessExpensiveAwards, ref leftCost);//再按照剩余的金币数取出便宜的奖品
                if (resultLessExpensiveAwards != null && resultLessExpensiveAwards.Count > 0) result.AddRange(resultLessExpensiveAwards);
            }
            if (result.Count > 0) result = result.OrderByDescending(t => t.Price).Take(2).ToList();//所有取到的物品，只取价格最高的2个
            return result;
        }
        /// <summary>
        /// 从源数据列表中随机取出一个奖品
        /// </summary>
        /// <param name="coins">金币数</param>
        /// <param name="sourceData">源数据</param>
        /// <returns></returns>
        private List<AwardData> FindPropAwards(int coins, List<AwardData> sourceData, ref int leftCoins)
        {
            if (coins <= 0)
            {
                throw new System.Exception("<><AwardDataHelper.FindPropAwards>Paramater 'cost' must not be less than zero");
            }
            else if (sourceData == null || sourceData.Count == 0)
            {
                throw new System.Exception("<><AwardDataHelper.FindPropAwards>Paramater 'sourceData' is null or its length is zero");
            }

            List<AwardData> result = new List<AwardData>();
            sourceData = sourceData.FindAll(t => t.Price <= coins);//过滤出价格小于金币数的奖品
            if (sourceData.Count == 0) return result;//如果一个都没有，则返回空的集合
            int randomIndex = Random.Range(0, sourceData.Count * 10) % sourceData.Count;
            AwardData awardData = sourceData[randomIndex];//随机取出一个奖品
            int count = coins / awardData.Price;//计算金币数一共购买几个奖品
            if (coins % awardData.Price != 0 && (awardData.Price * (count + 1)) / (float)coins <= 1.1f)//如果金币还有剩余，尝试再多买一个，但不能超出所有金币数的10%
                count += 1;
            leftCoins = coins - awardData.Price * count;
            awardData.Count = count;
            result.Add(awardData);
            return result;
        }
        /// <summary>
        /// 食物数据列表转为统一格式AwardData
        /// </summary>
        /// <param name="propertyItemList"></param>
        /// <returns></returns>
        private List<AwardData> PropertyItemList2AwardDataList(List<PropertyItem> propertyItemList)
        {
            if (propertyItemList == null || propertyItemList.Count == 0)
            {
                throw new System.Exception("<><AwardDataHelper.PropertyItemList2AwardDataList>Paramater 'propertyItemList' is null or its length is zero");
            }

            List<AwardData> awardDataList = new List<AwardData>();
            foreach (var propertyItem in propertyItemList)
            {
                if (propertyItem.UnlockLevel > this.PlayerDataManager.playerLevel)
                    continue;//只抓取解锁等级小于等于玩家当前等级的食物(否则即使获得此食物的奖励，喂食页上也是锁着的状态)

                awardDataList.Add(new AwardData()
                {
                    ID = propertyItem.Id,
                    Count = 1,
                    Price = propertyItem.Price,
                    Icon = "PetFeed/" + propertyItem.HighLightIcon,
                    AwardType = AwardTypes.FOOD
                });
            }
            return awardDataList;
        }
        /// <summary>
        /// 打印获得的奖品
        /// </summary>
        /// <param name="awardDatas"></param>
        private void PrintAwardDatas(AwardDatas awardDatas)
        {
            if (awardDatas != null && awardDatas.Datas != null && awardDatas.Datas.Count > 0)
            {
                foreach (var awardData in awardDatas.Datas)
                    Debug.LogFormat("<><AwardDataHelper.PrintAwardDatas>{0}", awardData);
            }
        }
        /// <summary>
        /// 增加奖品
        /// </summary>
        /// <param name="awardData">奖品数据</param>
        public void AddItem(AwardData awardData)
        {
            if (awardData != null)
            {
                if (awardData.AwardType.ToLower() == AwardTypes.COIN.ToLower() ||
                    awardData.AwardType.ToLower() == AwardTypes.FOOD.ToLower())
                    this.FundDataManager.AddProperty(awardData.ID, awardData.Count);
                else
                    this.ItemDataManager.AddItem(awardData.ID, awardData.Count);
            }
            else Debug.LogError("<><AwardDataHelper.AddItem>Parameter 'awardData' is null");
        }
    }
}
