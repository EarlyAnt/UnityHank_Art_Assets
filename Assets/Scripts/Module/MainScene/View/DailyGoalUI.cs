using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Spine.Unity;

namespace Hank.MainScene
{
    public class DailyGoalUI : MonoBehaviour
    {
        
//         [SerializeField]
// 		Slider Slider1,Slider2,Slider3;
//         [SerializeField]
//         GameObject handle, handle1, handle2;
//         [SerializeField]
//         GameObject drinkAward1, drinkAward2, drinkAward3;


        [SerializeField]
        dailyProcessBar normalStatusBar, sleepBar, schoolBar;
        [SerializeField]
		SkeletonGraphic dailyGoalGp1,dailyGoalGp2,dailyGoalGp3;
        [SerializeField]
        SkeletonGraphic complete100CwGp;

        // Use this for initialization
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetDailyGoalPercent(float dailyGoalPercent, bool languageInitialized)
		{
            if (normalStatusBar != null)
            {
                normalStatusBar.setAwardAndHandle(dailyGoalPercent);
            }
            if (sleepBar != null)
            {
                sleepBar.setAwardAndHandle(dailyGoalPercent);
            }
            if (schoolBar != null)
            {
                schoolBar.setAwardAndHandle(dailyGoalPercent);
            }

            complete100CwGp.gameObject.SetActive(dailyGoalPercent >= 1);

            if (dailyGoalPercent < 0.3)
			{
                dailyGoalGp1.AnimationState.ClearTracks();
                dailyGoalGp1.AnimationState.SetAnimation(0, "fall_d", false);
				dailyGoalGp1.AnimationState.AddAnimation(0, "small crown_d", true,0);
                if (languageInitialized)//设置过语言后才能播放此声音
                    SoundPlayer.GetInstance().PlaySoundType("daily_goal_crown_jump_in");
            }
            else if(dailyGoalPercent >= 0.3 && dailyGoalPercent < 0.6)
			{
                dailyGoalGp2.AnimationState.ClearTracks();
                dailyGoalGp2.AnimationState.SetAnimation(0, "fall_c", false);
				dailyGoalGp2.AnimationState.AddAnimation(0, "small crown_c", true, 0);
                if (languageInitialized)//设置过语言后才能播放此声音
                    SoundPlayer.GetInstance().PlaySoundType("daily_goal_crown_jump_in");
            }
            else if (dailyGoalPercent >= 0.6 && dailyGoalPercent < 1.0)
			{
                dailyGoalGp3.AnimationState.ClearTracks();
                dailyGoalGp3.AnimationState.SetAnimation(0, "fall_s", false);
				dailyGoalGp3.AnimationState.AddAnimation(0, "small crown_s", true, 0);
                if (languageInitialized)//设置过语言后才能播放此声音
                    SoundPlayer.GetInstance().PlaySoundType("daily_goal_crown_jump_in");
            }
            else if(dailyGoalPercent >= 1.0)
			{
                complete100CwGp.AnimationState.ClearTracks();
                complete100CwGp.AnimationState.SetAnimation(0, "daily goal crown_drop", false);
                complete100CwGp.AnimationState.AddAnimation(0, "daily goal crown_idle", true, 0);
            }

        }

    }
}
