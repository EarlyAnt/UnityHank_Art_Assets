using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Spine.Unity;
using UnityEngine.Events;
using Gululu.LocalData.Agents;
using UnityEngine.Assertions;

namespace Hank.MainScene
{


    public class StartDailyProcessBar : MainPresentBase
    {
        [Inject]
        public IPlayerDataManager dataManager { get; set; }
        [Inject]
        public ILocalPetInfoAgent mLocalPetInfoAgent { get; set; }
        [Inject]
        public IPlayerDataManager PlayerDataManager { get; set; }

        [SerializeField]
        GameObject objNormal;
        [SerializeField]
        SkeletonGraphic dailyGoalGp;
        [SerializeField]
        DailyGoalUI dailyGoalSc;
        [SerializeField]
        GameObject[] bgForRole;

        public override void TestSerializeField()
        {
            Assert.IsNotNull(objNormal);
            Assert.IsNotNull(dailyGoalGp);
            Assert.IsNotNull(dailyGoalSc);

            Assert.AreEqual(bgForRole.Length, 4);
            for (int i = 0; i < 4; ++i)
            {
                Assert.IsNotNull(bgForRole[i]);
            }

        }

        string[] ani_list = { "ani_d", "ani_n", "ani_p", "ani_s", "ani_y", "ani_xn" };
        int roleType = 0;
        public override UIStateEnum GetStateEnum()
        {
            return UIStateEnum.eStartDailyProcessBar;
        }
        public override void ProcessState(UIState state)
        {
            bFinish = false;
            ChangeRole();
            doHandBarAni(roleType);
            if (this.PlayerDataManager.LanguageInitialized())//设置过语言后才能播放此声音
                SoundPlayer.GetInstance().PlaySoundType("daily_goal_progress_init_bgm");
        }

        private void ResetUI(bool beforeStart)
        {
            objNormal.SetActive(!beforeStart);
            dailyGoalGp.gameObject.SetActive(beforeStart);
        }

        public void ChangeRole()
        {
            string strType = mLocalPetInfoAgent.getCurrentPet();
            for (int i = 0; i < bgForRole.Length; i++)
            {
                if (strType == bgForRole[i].name)
                {
                    bgForRole[i].SetActive(true);
                    roleType = i;
                }
                else
                {
                    bgForRole[i].SetActive(false);
                }
            }
            if (roleType < 0 || roleType >= ani_list.Length)
            {
                roleType = 0;
            }
        }

        // Use this for initialization
        protected override void Start()
        {
            base.Start();
            ResetUI(true);

            dailyGoalGp.AnimationState.Complete += delegate
            {
                ResetUI(false);
                dailyGoalSc.SetDailyGoalPercent(dataManager.percentOfDailyGoal, this.PlayerDataManager.LanguageInitialized());
                bFinish = true;
            };
        }

        // Update is called once per frame
        void Update()
        {

        }

        void doHandBarAni(int type)
        {
            ResetUI(true);
            dailyGoalGp.AnimationState.ClearTracks();
            dailyGoalGp.AnimationState.SetAnimation(0, ani_list[type], false);
        }

    }
}

