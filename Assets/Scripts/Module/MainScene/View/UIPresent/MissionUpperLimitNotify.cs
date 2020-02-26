using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Hank.MainScene
{
    public class MissionUpperLimitNotify : MainPresentBase
    {
        [SerializeField]
        MissionName missionNameSC;
        public override void TestSerializeField()
        {
            Assert.IsNotNull(missionNameSC);
        }


        public override UIStateEnum GetStateEnum()
        {
            return UIStateEnum.eMissionUpperLimitNotify;
        }

        Coroutine myCoroutine = null;

        IEnumerator aniOver()
        {
            sleepTimeManager.AddSleepStatus(SleepTimeout.NeverSleep);

            yield return new WaitForSeconds(1);
            bFinish = true;
            myCoroutine = null;
            sleepTimeManager.PopSleepStatus();
        }
        public override void ProcessState(UIState state)
        {
            bFinish = false;
            missionNameSC.ShowMissionLimitSpark();
            if (myCoroutine != null)
            {
                StopCoroutine(myCoroutine);
            }
            myCoroutine = StartCoroutine(aniOver());
            Debug.LogFormat("<><BasePlayer.PlayActionSound>Sound: daily_goal_mission_complete_tick, HasModulePageOpened: {0}, InGuideView: {1}",
                            TopViewHelper.Instance.HasModulePageOpened(), TopViewHelper.Instance.IsInGuideView());
            if (!TopViewHelper.Instance.HasModulePageOpened() || TopViewHelper.Instance.IsInGuideView())
            {//没有打开任何功能模块或正在引导才能播放此声音
                Debug.LogFormat("<><BasePlayer.PlayActionSound>Sound: daily_goal_mission_complete_tick ==== Playing");
                SoundPlayer.GetInstance().PlaySoundType("daily_goal_mission_complete_tick");
            }
        }


        // Update is called once per frame
        void Update()
        {

        }
    }
}

