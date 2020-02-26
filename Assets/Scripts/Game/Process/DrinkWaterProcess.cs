using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gululu.Config;
using Hank.Data;
using Gululu.Util;
using Hank.MainScene;
using strange.extensions.mediation.impl;

namespace Hank
{
    public class DrinkWaterProcess : IDrinkWaterProcess
    {
        [Inject]
        public IDrinkWaterUIPresentManager uiState { get; set; }
        [Inject]
        public IPlayerDataManager dataManager { get; set; }
        [Inject]
        public IMissionDataManager missionDataManager { get; set; }
        [Inject]
        public IItemsDataManager itemsDataManager { get; set; }
        [Inject]
        public ITreasureBoxConfig treasureBoxConfig { get; set; }
        [Inject]
        public ITaskConfig taskConifg { get; set; }
        [Inject]
        public IItemConfig itemConifg { get; set; }
        [Inject]
        public IMissionConfig missionConfig { get; set; }
        [Inject]
        public ILevelConfig LevelConfig { get; set; }
        [Inject]
        public IFundDataManager FundDataManager { get; set; }
        [Inject]
        public IAwardDataHelper AwardDataHelper { get; set; }

        [Inject] public LevelUpSignal levelUpSignal { get; set; }

        [Inject] public UpdateExpAndCoinSignal updateExpAndCoinSignal { get; set; }

        bool bTrigger = false;

        // Update is called once per frame
        void Update()
        {

        }

        float GetExpPercent()
        {
            return dataManager.GetExpPercent();
        }

        private void SwitchCurrMission()
        {
            if (dataManager.missionId != ShowMissionHelper.Instance.currShowMissionId)
            {
                if (/*uiState.IsInSilenceMode() ||*/ (!uiState.IsInRunning()))
                {
                    uiState.PushNewState(UIStateEnum.eMissionFadeInOut, dataManager.missionId);
                }
            }
        }

        public void DrinkFail()
        {
            SwitchCurrMission();
            uiState.PushNewState(UIStateEnum.eDrinkFail, dataManager.missionId);
        }
        float GetIncreasePercent(int intake)
        {
            float missionIntake = (((float)dataManager.GetDailyGoal()) * missionConfig.GetDailyGoalPercent(dataManager.missionId));
            return intake / missionIntake;
        }
        public void DrinkWater(int intake)
        {
            SwitchCurrMission();
            bTrigger = false;
            int expIncrease = 0;
            int coinIncrease = this.LevelConfig.GetDrinkWaterCoin(this.dataManager.playerLevel, this.dataManager.TodayDrinkTimes);
            this.FundDataManager.AddCoin(this.FundDataManager.GetCoinId(), coinIncrease);
            if (IsAchieveGoal())
            {
                expIncrease = this.LevelConfig.GetDrinkWaterExpGoal(this.dataManager.playerLevel);
                dataManager.exp += expIncrease;
                if (dataManager.IsMissionLimit())
                {
                    uiState.PushNewState(UIStateEnum.eDropWaterAni, (float)dataManager.missionPercent, expIncrease, true, coinIncrease);
                    uiState.PushNewState(UIStateEnum.eExpIncrease, GetExpPercent(), expIncrease);
                    RefreshLevelAfterExpChange();
                    uiState.PushNewState(UIStateEnum.eDailyGoal100CompleteNotify, dataManager.missionId, 0, false);
                    uiState.PushNewState(UIStateEnum.eMissionUpperLimitNotify);
                }
                else
                {
                    dataManager.missionPercent += GetIncreasePercent(intake);
                    uiState.PushNewState(UIStateEnum.eDropWaterAni, (float)dataManager.missionPercent, expIncrease, true, coinIncrease);
                    uiState.PushNewState(UIStateEnum.eExpIncrease, GetExpPercent());
                    RefreshLevelAfterExpChange();
                    AddIntake(intake);
                }
            }
            else
            {
                expIncrease = this.LevelConfig.GetDrinkWaterExpNormal(this.dataManager.playerLevel);
                dataManager.missionPercent += GetIncreasePercent(intake);
                uiState.PushNewState(UIStateEnum.eDropWaterAni, (float)dataManager.missionPercent, expIncrease, true, coinIncrease);

                dataManager.exp += expIncrease;
                uiState.PushNewState(UIStateEnum.eExpIncrease, GetExpPercent());

                RefreshLevelAfterExpChange();

                AddIntake(intake);
            }
        }

        public void RefreshLevelAndExpView()
        {
            RefreshLevelAfterExpChange();
        }

        void AddIntake(int intake)
        {
            if (TreasureBoxProcess())
            {
                bTrigger = true;
            }

            if (DailyGoalPercentProcess(intake))
            {
                bTrigger = true;
            }

            if (MissionPercentProcess())
            {
                bTrigger = true;
            }

            if (!bTrigger)
            {
                uiState.PushNewState(UIStateEnum.eRolePlayAction, "DrinkSuccess", 0, false);
                FlurryUtil.LogEvent("Drink_PET_ani_view");
            }
        }

        private void AddItem(int i)
        {
            MissionInfo info = missionConfig.GetMissionInfoByMissionId(dataManager.missionId);
            if (info != null && i < info.listTreasureBox.Count)
            {
                TreasureBox treasureBox = treasureBoxConfig.GetTreasureBoxById(info.listTreasureBox[i].BoxId);
                foreach (var treasure in treasureBox.items)
                {
                    Item item = itemConifg.GetItemById(treasure.itemId);
                    if (item != null && item.Type == 4)
                    {
                        itemsDataManager.AddItem(treasure.itemId);
                    }
                }
            }
        }

        private bool TreasureBoxProcess()
        {
            bool bRet = false;
            //             if (dataManager.missionPercent < 1)
            {
                for (int i = 0; i < missionDataManager.GetTreasureBoxCount(); ++i)
                {
                    if (!missionDataManager.IsTreasureOpen(dataManager.missionId, i))
                    {
                        if (missionDataManager.CanTreasureOpen(dataManager.missionPercent, i))
                        {
                            uiState.PushNewState(UIStateEnum.eTreasureBoxOpen, i, dataManager.missionId);
                            missionDataManager.SetTreasureOpen(dataManager.missionId, i);
                            AddItem(i);
                            dataManager.TrySendPlayerData2Server();
                            bRet = true;
                        }
                    }
                }
            }
            return bRet;
        }
        float[] CrownPersent =
        {
            0.3f,0.6f,1.0f
        };


        private void AddCrownItem(int i)
        {
            int crownid = 0;
            switch (i)
            {
                case 0:
                    crownid = 10003;
                    break;
                case 1:
                    crownid = 10004;
                    break;
                case 2:
                    crownid = 10005;
                    break;
            }
            itemsDataManager.AddItem(crownid);
        }

        bool DailyGoalPercentProcess(int intake)
        {
            if (IsAchieveGoal())
            {
                uiState.PushNewState(UIStateEnum.eDailyGoal100CompleteNotify, dataManager.missionId);
            }
            else
            {
                dataManager.currIntake += intake;

                uiState.PushNewState(UIStateEnum.eDailyProcessIncrease, dataManager.percentOfDailyGoal);
                for (int i = 0; i < CrownPersent.Length; ++i)
                {
                    if (!dataManager.DidCrownGet(i))
                    {
                        if (dataManager.percentOfDailyGoal >= CrownPersent[i])
                        {
                            ++dataManager.todayCrownGotCount;
                            ++dataManager.CrownCount;
                            dataManager.TrySendPlayerData2Server();
                            int expDelta = dataManager.GetDailyGoalExp(this.CrownPersent[i]);//计算获取的经验
                            DailyGoal dailyGoal = this.LevelConfig.GetLevelDailyGoal(this.dataManager.playerLevel, this.CrownPersent[i]);//获取当前等级与当日饮水进度对应的配饰数据
                            AwardDatas awardDatas = this.AwardDataHelper.GetAwards(dailyGoal.Coin, dailyGoal.Award);//根据获得的奖励的总价值及奖品产出规则，计算获得的金币及道具
                            if (awardDatas != null && awardDatas.Datas != null)
                                awardDatas.Datas.ForEach(t => this.AwardDataHelper.AddItem(t));//记录获得的金币和道具(食物，材料等)
                            uiState.PushNewState(UIStateEnum.eDailyGoalCrownComplete, i, expDelta, true, awardDatas);
                            AddCrownItem(i);
                            dataManager.exp += expDelta;
                            uiState.PushNewState(UIStateEnum.eExpIncrease, GetExpPercent());
                            RefreshLevelAfterExpChange();
                            if (i == CrownPersent.Length - 1)
                            {
                                uiState.PushNewState(UIStateEnum.eDailyGoal100CompleteNotify, dataManager.missionId);
                            }
                            else
                            {
                                uiState.PushNewState(UIStateEnum.eDailyProcessIncrease, dataManager.percentOfDailyGoal);
                            }
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private bool MissionPercentProcess()
        {
            if (dataManager.missionPercent >= 1)
            {
                int expIncrease = GetMissionCompleteExp();
                int coinIncrease = GetMissionCompleteCoin();
                this.FundDataManager.AddCoin(this.FundDataManager.GetCoinId(), coinIncrease);
                uiState.PushNewState(UIStateEnum.eRolePlayAction, "level_end");
                FlurryUtil.LogEvent("Mission_Finish_Petani_View");

                uiState.PushNewState(UIStateEnum.eMissionComplete, dataManager.missionId, expIncrease, true, coinIncrease);

                dataManager.exp += expIncrease;
                uiState.PushNewState(UIStateEnum.eExpIncrease, GetExpPercent());

                RefreshLevelAfterExpChange();

                missionDataManager.AddMissionState(dataManager.missionId, MissionState.eComplete);
                if (dataManager.IsMissionLimit())
                {
                    uiState.PushNewState(UIStateEnum.eMissionUpperLimitNotify);
                }
                else
                {
                    bool missionRetry = false;
                    dataManager.missionPercent = 0;
                    int sceneIndex = missionConfig.GetSceneIndexByMissionID(dataManager.missionId);
                    int nextMissionId = missionConfig.GetNextMissionId(dataManager.missionId, true);
                    if (nextMissionId == -1)
                    {//如果已经到达此星球的最后一个关卡，则检查此星球是否为副本星球
                        WorldInfo worldInfo = this.missionConfig.GetWorldByStarID(this.dataManager.StarID);
                        if (worldInfo != null && worldInfo.Type == WorldInfo.WorldTypes.Duplicate)
                        {//副本星球的关卡是可以循环玩的，所以到达最后一个关卡后，再返回第一个关卡
                            missionRetry = true;
                            nextMissionId = this.missionConfig.GetFirstMissionOfScene(this.dataManager.StarID, sceneIndex);
                            if (nextMissionId != -1)
                                this.missionDataManager.ClearAllMissionOfScene(this.dataManager.StarID, sceneIndex);//清除此星球当前地图的所有关卡记录
                        }
                    }

                    if (nextMissionId != -1)
                    {
                        FlurryUtil.LogEventWithParam("Mission_Finish_Event", "MissionID", dataManager.missionId.ToString());
                        missionDataManager.AddMissionState(nextMissionId, MissionState.eUnderway);
                        dataManager.missionId = nextMissionId;//这里要先把nextMissionId的eUnderWay状态记录下来，再改变dataManager的missionId，否则(eLock状态)写不进去
                        uiState.PushNewState(UIStateEnum.eNewMissionSwitch, dataManager.missionId);
                        uiState.PushNewState(UIStateEnum.eRolePlayAction, "level_start", 0, false);
                        if (missionRetry) uiState.PushNewState(UIStateEnum.eMissionRetry, nextMissionId);
                    }
                }
                dataManager.TrySendPlayerData2Server();
                return true;
            }
            return false;
        }

        int GetMissionCompleteExp()
        {
            MissionInfo info = missionConfig.GetMissionInfoByMissionId(dataManager.missionId);
            if (info == null)
            {
                return 10;
            }
            TaskSetting task = taskConifg.GetTaskById(info.QuestId);
            if (task != null)
            {
                foreach (var reward in task.Rewards)
                {
                    if (reward.ItemID == 10001)
                    {
                        return (int)reward.Value;
                    }
                }
            }
            return 10;
        }

        int GetMissionCompleteCoin()
        {
            MissionInfo info = missionConfig.GetMissionInfoByMissionId(dataManager.missionId);
            return info != null ? info.AwardCoin : 10;
        }

        void RefreshLevelAfterExpChange()
        {
            if (dataManager.exp > dataManager.expForLevelUp)
            {
                dataManager.exp = dataManager.exp - dataManager.expForLevelUp;
                dataManager.playerLevel += 1;
                dataManager.TrySendPlayerData2Server();
                uiState.PushNewState(UIStateEnum.ePlayerLevelUp, dataManager.playerLevel);
                uiState.PushNewState(UIStateEnum.eExpIncrease, GetExpPercent());
                bTrigger = true;
                levelUpSignal.Dispatch();
            }
            else
            {
                uiState.PushNewState(UIStateEnum.eExpIncrease, GetExpPercent());
            }
            updateExpAndCoinSignal.Dispatch();
        }


        bool IsAchieveGoal()
        {
            return (dataManager.currIntake >= dataManager.GetDailyGoal());
        }

    }

}
