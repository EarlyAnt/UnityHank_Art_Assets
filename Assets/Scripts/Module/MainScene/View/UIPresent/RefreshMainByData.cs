using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Assertions;

namespace Hank.MainScene
{
    public class RefreshMainByData : MainPresentBase
    {
        [Inject]
        public IPlayerDataManager dataManager { get; set; }

        [SerializeField]
        LevelUI levelSc;
        [SerializeField]
        DailyGoalUI dailyGoalSc;
        [SerializeField]
        WaterSlider sliderWave;
        [SerializeField]
        MissionName missionNameSC;
        [SerializeField]
        MissionBackground missionBackgroundSC;
        //[SerializeField]
        //RightTreasureBoxSpecialIdleController rightTreasure;
        float missionProcess;
        [SerializeField]
        RolePlayer rolePlayerSC;
        public override void TestSerializeField()
        {
            Assert.IsNotNull(levelSc);
            Assert.IsNotNull(dailyGoalSc);
            Assert.IsNotNull(sliderWave);
            Assert.IsNotNull(missionNameSC);
            Assert.IsNotNull(missionBackgroundSC);
            //Assert.IsNotNull(rightTreasure);
            Assert.IsNotNull(rolePlayerSC);
        }

        public override UIStateEnum GetStateEnum()
        {
            return UIStateEnum.eRefreshMainByData;
        }
        public override void ProcessState(UIState state)
        {
            Debug.LogFormat("<><RefreshMainByData.ProcessState>StarID: {0}, MissionID: {1}", this.dataManager.StarID, this.dataManager.missionId);
            missionProcess = (float)dataManager.missionPercent;
            sliderWave.RefreshSlideValue();
            levelSc.ChangeRole();
            levelSc.SetExp(dataManager.GetExpPercent());
            levelSc.SetLevel(dataManager.playerLevel);
            dailyGoalSc.SetDailyGoalPercent(dataManager.percentOfDailyGoal, this.dataManager.LanguageInitialized());
            missionBackgroundSC.RefreshCurrentBackground(dataManager.missionId);
            missionNameSC.RefreshMissionName(dataManager.IsMissionLimit());
            //rightTreasure.RefreshRightTreasureBoxByMission(this.dataManager.missionId);
            rolePlayerSC.RefreshRole(true);

            gameObject.SetActive(false);
            bFinish = true;
        }


        // Update is called once per frame
        void Update()
        {

        }
    }
}

