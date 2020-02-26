using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Spine.Unity;
using Spine;
using Cup.Utils.Touch;
using UnityEngine.Assertions;
using Gululu.Util;

namespace Hank.MainScene
{
    public class DailyGoalCrownComplete : MainPresentBase
    {
        [Inject]
        public IPrefabRoot PrefabRoot { get; set; }
        [Inject]
        public UIStateCompleteSignal uIStateCompleteSignal { get; set; }

        [SerializeField]
        SkeletonGraphic goalFinishGp1, goalFinishGp2, goalFinishGp3;
        [SerializeField]
        SkeletonGraphic dailyGoalGp1, dailyGoalGp2, dailyGoalGp3;
        [SerializeField]
        GameObject handle1, handle2, handle3;
        [SerializeField]
        Image indicate1, indicate2, indicate3;
        [SerializeField]
        GameObject imageDrinkAward1, imageDrinkAward2, imageDrinkAward3;
        [SerializeField]
        GameObject dailyGoalBlur;
        [SerializeField]
        SkeletonGraphic bigCrownGp;
        [SerializeField]
        SkeletonGraphic complete100CwGp;
        [SerializeField]
        RectTransform mcAward;
        [SerializeField]
        TextLabel expBox, coinBox, prop1Box, prop2Box;
        [SerializeField]
        DailyGoalUI dailyGoalUI;

        int crownIndex = -1;
        int dailyGoalExp = 0;
        AwardDatas awardDatas = null;
        float expOriginY = 0;
        bool isIdle = false;
        public override void TestSerializeField()
        {
            Assert.IsNotNull(goalFinishGp1);
            Assert.IsNotNull(goalFinishGp2);
            Assert.IsNotNull(goalFinishGp3);
            Assert.IsNotNull(dailyGoalGp1);
            Assert.IsNotNull(dailyGoalGp2);
            Assert.IsNotNull(dailyGoalGp3);
            Assert.IsNotNull(handle1);
            Assert.IsNotNull(handle2);
            Assert.IsNotNull(handle3);
            Assert.IsNotNull(indicate1);
            Assert.IsNotNull(indicate2);
            Assert.IsNotNull(indicate3);
            Assert.IsNotNull(imageDrinkAward1);
            Assert.IsNotNull(imageDrinkAward2);
            Assert.IsNotNull(imageDrinkAward3);
            Assert.IsNotNull(dailyGoalBlur);
            Assert.IsNotNull(bigCrownGp);
            Assert.IsNotNull(complete100CwGp);
            Assert.IsNotNull(mcAward);
            Assert.IsNotNull(coinBox);
            Assert.IsNotNull(expBox);
            Assert.IsNotNull(prop1Box);
            Assert.IsNotNull(prop2Box);
            Assert.IsNotNull(dailyGoalUI);
        }

        public override UIStateEnum GetStateEnum()
        {
            return UIStateEnum.eDailyGoalCrownComplete;
        }
        public override void ProcessState(UIState state)
        {
            this.isIdle = false;
            this.crownIndex = (int)state.value0;
            this.dailyGoalExp = (int)state.value1;
            this.awardDatas = (AwardDatas)state.value2;
            this.bFinish = false;
            this.HideLabels();
            this.goalFinishAni();
        }

        private void HideLabels()
        {
            this.expBox.SetStatus(false, true);
            this.coinBox.SetStatus(false, true);
            this.prop1Box.SetStatus(false, true);
            this.prop2Box.SetStatus(false, true);
        }

        override public void Stop()
        {
            base.Stop();
            mcAward.gameObject.SetActive(false);
            dailyGoalBlur.SetActive(false);
        }

        protected override void Start()
        {
            base.Start();

            dailyGoalBlur.SetActive(false);
            setSliderGpComplete();
            setBigCrownComplete();

        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
        }

        void setGoalFinishGpComplete()
        {
            goalFinishGp1.AnimationState.Complete += delegate
            {
                imageDrinkAward1.SetActive(true);
                dailyGoalGp1.AnimationState.ClearTracks();
                dailyGoalGp1.AnimationState.SetAnimation(1, "leave_d", false);
                goalFinishGp1.gameObject.SetActive(false);

            };

            goalFinishGp2.AnimationState.Complete += delegate
            {
                imageDrinkAward2.SetActive(true);
                dailyGoalGp2.AnimationState.ClearTracks();
                dailyGoalGp2.AnimationState.SetAnimation(2, "leave_c", false);
                goalFinishGp2.gameObject.SetActive(false);
            };

            goalFinishGp3.AnimationState.Complete += delegate
            {
                imageDrinkAward3.SetActive(true);
                dailyGoalGp3.AnimationState.ClearTracks();
                dailyGoalGp3.AnimationState.SetAnimation(3, "leave_s", false);
                goalFinishGp3.gameObject.SetActive(false);
            };
        }

        void setSliderGpComplete()
        {
            dailyGoalGp1.AnimationState.Complete += delegate (TrackEntry trackEntry)
            {
                if (trackEntry.trackIndex == 1)
                {
                    indicate1.DOFade(0, 0.5f);
                    dailyGoalBlur.SetActive(true);
                    bigCrownGp.AnimationState.ClearTracks();
                    bigCrownGp.AnimationState.SetAnimation(5, "update_d", false);
                    SoundPlayer.GetInstance().PlaySoundType("daily_goal_milestone_01_crown_shapeshift");
                    if (dailyGoalExp == 0)
                    {
                        bigCrownGp.AnimationState.AddAnimation(0, "idle_c", true, 0);
                    }
                }
            };

            dailyGoalGp2.AnimationState.Complete += delegate (TrackEntry trackEntry)
            {
                if (trackEntry.trackIndex == 2)
                {
                    indicate2.DOFade(0, 0.5f);
                    dailyGoalBlur.SetActive(true);
                    bigCrownGp.AnimationState.ClearTracks();
                    bigCrownGp.AnimationState.SetAnimation(5, "update_c", false);
                    SoundPlayer.GetInstance().PlaySoundType("daily_goal_milestone_02_crown_shapeshift");
                    if (dailyGoalExp == 1)
                    {
                        bigCrownGp.AnimationState.AddAnimation(0, "idle_s", true, 0);
                    }
                }
            };

            dailyGoalGp3.AnimationState.Complete += delegate (TrackEntry trackEntry)
            {
                if (trackEntry.trackIndex == 3)
                {
                    indicate3.DOFade(0, 0.5f);
                    dailyGoalBlur.SetActive(true);
                    bigCrownGp.AnimationState.ClearTracks();
                    bigCrownGp.AnimationState.SetAnimation(5, "update_s", false);
                    SoundPlayer.GetInstance().PlaySoundType("daily_goal_milestone_03_crown_shapeshift");
                    SoundPlayer.GetInstance().PlaySoundType("daily_goal_progress_done_bgm");
                    if (dailyGoalExp == 2)
                    {
                        bigCrownGp.AnimationState.AddAnimation(0, "idle_g", true, 0);
                    }
                }
            };
        }

        void setBigCrownComplete()
        {
            bigCrownGp.AnimationState.Complete += delegate (TrackEntry trackEntry)
            {
                if (trackEntry.trackIndex == 4)
                {
                    dailyGoalBlur.SetActive(false);
                    if (crownIndex == 0)
                    {
                        handle1.SetActive(false);
                        indicate1.color = new Color(indicate3.color.r, indicate3.color.g, indicate3.color.b, 1);
                        handle2.SetActive(true);
                        dailyGoalGp2.AnimationState.ClearTracks();
                        dailyGoalGp2.AnimationState.SetAnimation(0, "fall_c", false);
                        dailyGoalGp2.AnimationState.AddAnimation(0, "small crown_c", true, 0);
                        SoundPlayer.GetInstance().PlaySoundType("daily_goal_crown_jump_in");

                        //dailyGoalUI.indicateTnPlay(1);
                    }
                    else if (crownIndex == 1)
                    {
                        handle2.SetActive(false);
                        indicate2.color = new Color(indicate3.color.r, indicate3.color.g, indicate3.color.b, 1);
                        handle3.SetActive(true);
                        dailyGoalGp3.AnimationState.ClearTracks();
                        dailyGoalGp3.AnimationState.SetAnimation(0, "fall_s", false);
                        dailyGoalGp3.AnimationState.AddAnimation(0, "small crown_s", true, 0);
                        SoundPlayer.GetInstance().PlaySoundType("daily_goal_crown_jump_in");

                        //dailyGoalUI.indicateTnPlay(2);
                    }
                    else if (crownIndex == 2)
                    {
                        handle3.SetActive(false);
                        indicate3.color = new Color(indicate3.color.r, indicate3.color.g, indicate3.color.b, 1);
                        complete100CwGp.gameObject.SetActive(true);
                        complete100CwGp.AnimationState.ClearTracks();
                        complete100CwGp.AnimationState.SetAnimation(0, "daily goal crown_drop", false);
                        complete100CwGp.AnimationState.AddAnimation(0, "daily goal crown_idle", true, 0);
                    }
                    bFinish = true;
                    uIStateCompleteSignal.Dispatch(GetStateEnum());
                }
                else if (trackEntry.trackIndex == 5)
                {
                    if (dailyGoalExp > 0)
                    {
                        bigCrownOpenRewardAni(crownIndex);
                    }
                }
            };
        }


        void goalFinishAni()
        {
            if (crownIndex == 0)
            {
                goalFinishGp1.gameObject.SetActive(true);

                goalFinishGp1.AnimationState.ClearTracks();
                goalFinishGp1.AnimationState.SetAnimation(0, "daily goal_c", false).Complete += delegate
                {
                    imageDrinkAward1.SetActive(true);
                    dailyGoalGp1.AnimationState.SetAnimation(1, "leave_d", false);
                    goalFinishGp1.gameObject.SetActive(false);
                    SoundPlayer.GetInstance().PlaySoundType("daily_goal_crown_jump_out");
                };
                SoundPlayer.GetInstance().PlaySoundType("daily_goal_progress_node_active_1");

            }
            else if (crownIndex == 1)
            {
                goalFinishGp2.gameObject.SetActive(true);
                goalFinishGp2.AnimationState.ClearTracks();
                goalFinishGp2.AnimationState.SetAnimation(0, "daily goal_s", false).Complete += delegate
                {
                    imageDrinkAward2.SetActive(true);
                    dailyGoalGp2.AnimationState.SetAnimation(2, "leave_c", false);
                    goalFinishGp2.gameObject.SetActive(false);
                    SoundPlayer.GetInstance().PlaySoundType("daily_goal_crown_jump_out");
                };
                SoundPlayer.GetInstance().PlaySoundType("daily_goal_progress_node_active_2");
            }
            else if (crownIndex == 2)
            {
                goalFinishGp3.gameObject.SetActive(true);
                goalFinishGp3.AnimationState.ClearTracks();
                goalFinishGp3.AnimationState.SetAnimation(0, "daily goal_g", false).Complete += delegate
                {
                    imageDrinkAward3.SetActive(true);
                    dailyGoalGp3.AnimationState.SetAnimation(3, "leave_s", false);
                    goalFinishGp3.gameObject.SetActive(false);
                    SoundPlayer.GetInstance().PlaySoundType("daily_goal_crown_jump_out");
                };
                SoundPlayer.GetInstance().PlaySoundType("daily_goal_progress_node_active_3");
            }
        }

        void bigCrownOpenRewardAni(int rewardId)
        {
            string[] openReward = { "open_reward_c", "open_reward_s", "open_reward_g" };
            string[] rewardIdle = { "open_idle_c", "open_idle_s", "open_idle_g" };
            bigCrownGp.AnimationState.ClearTracks();
            bigCrownGp.AnimationState.SetAnimation(0, openReward[rewardId], false);
            bigCrownGp.AnimationState.AddAnimation(0, rewardIdle[rewardId], true, 0);
            SoundPlayer.GetInstance().PlaySoundType("daily_goal_crown_fly_in");

            addExpAni();
        }

        void Update()
        {

        }

        private void OnEnable()
        {
#if (UNITY_ANDROID) && (!UNITY_EDITOR)
            CupKeyEventHookStaticManager.addPowerKeyPressCallBack(powerKeyPressUp);
#endif
        }

        private void OnDisable()
        {
#if (UNITY_ANDROID) && (!UNITY_EDITOR)
            CupKeyEventHookStaticManager.removePowerKeyPressCallBack(powerKeyPressUp);
#endif
        }

        public void powerKeyPressUp()
        {
            if (TopViewHelper.Instance.HasModulePageOpened())
                return;

            switch (crownIndex)
            {
                case 0:
                    {
                        FlurryUtil.LogEvent("Dailygoal_30_Exp_Collect_Click");
                    }
                    break;
                case 1:
                    {
                        FlurryUtil.LogEvent("Dailygoal_60_Exp_Collect_Click");
                    }
                    break;
                case 2:
                    {
                        FlurryUtil.LogEvent("Dailygoal_100_Exp_Collect_Click");
                    }
                    break;
            }

            uIStateCompleteSignal.Dispatch(GetStateEnum());
            if (dailyGoalBlur.activeInHierarchy && isIdle)
            {
                ExpDisappearAni();
            }
        }

        void addExpAni()
        {
            expOriginY = mcAward.anchoredPosition3D.y;
            mcAward.gameObject.SetActive(true);
            mcAward.transform.DOLocalMoveY(expOriginY + crownIndex * 10, 1.0f);
            mcAward.GetComponent<CanvasGroup>().DOFade(1, 1.0f).onComplete = delegate ()
            {
                isIdle = true;
            };
            if (this.awardDatas != null)
            {
                int textLength = Mathf.Max(this.dailyGoalExp.ToString().Length, this.awardDatas.Coin.ToString().Length);
                this.expBox.SetStatus(true, true);
                this.expBox.SetText("+" + this.dailyGoalExp.ToString().PadLeft(textLength, ' '));
                this.coinBox.SetStatus(this.awardDatas.Coin > 0, true);
                this.coinBox.SetText("+" + this.awardDatas.Coin.ToString().PadLeft(textLength, ' '));
                if (this.awardDatas.PropDatas != null && this.awardDatas.PropDatas.Count > 0)
                {
                    for (int i = 0, index = 0; i < this.awardDatas.PropDatas.Count && index < 2; i++)
                    {
                        if (index == 0)
                        {
                            this.prop1Box.SetStatus(true, true);
                            this.prop1Box.SetIcon(this.PrefabRoot.GetObjectNoInstantiate<Sprite>("Texture", this.awardDatas.PropDatas[i].Icon));
                            this.prop1Box.SetText("+" + this.awardDatas.PropDatas[i].Count);
                            index += 1;
                        }
                        else if (index == 1)
                        {
                            this.prop2Box.SetStatus(true, true);
                            this.prop2Box.SetIcon(this.PrefabRoot.GetObjectNoInstantiate<Sprite>("Texture", this.awardDatas.PropDatas[i].Icon));
                            this.prop2Box.SetText("+" + this.awardDatas.PropDatas[i].Count);
                            index += 1;
                        }
                    }
                }
            }
        }

        void ExpDisappearAni()
        {
            SoundPlayer.GetInstance().PlaySoundType("daily_goal_claim_award");
            mcAward.transform.DOLocalMoveY(80, 1.0f);
            mcAward.GetComponent<CanvasGroup>().DOFade(0, 1.0f).onComplete = delegate ()
            {
                dailyGoalExp = 0;
                mcAward.gameObject.SetActive(false);
                mcAward.GetComponent<RectTransform>().DOLocalMoveY(expOriginY, 0f);
                if (crownIndex == 0)
                {
                    bigCrownGp.AnimationState.AddAnimation(4, "leave_c", false, 0);
                }
                else if (crownIndex == 1)
                {
                    bigCrownGp.AnimationState.AddAnimation(4, "leave_s", false, 0);
                }
                else if (crownIndex == 2)
                {
                    bigCrownGp.AnimationState.AddAnimation(4, "leave_g", false, 0);
                }
            };
        }
    }
}