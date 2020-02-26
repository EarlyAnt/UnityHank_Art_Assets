using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Spine.Unity;
using UnityEngine.Assertions;

namespace Hank.MainScene
{
    public class DailyProcessIncrease : MainPresentBase
    {
		[SerializeField]
		dailyProcessBar normalStatusBar, sleepBar, schoolBar;
        public override void TestSerializeField()
        {
            Assert.IsNotNull(normalStatusBar);
            Assert.IsNotNull(sleepBar);
            Assert.IsNotNull(schoolBar);
        }

        public override UIStateEnum GetStateEnum()
        {
            return UIStateEnum.eDailyProcessIncrease;
        }
        public override void ProcessState(UIState state)
        {
            bFinish = false;
			if(normalStatusBar != null)
			{
				normalStatusBar.dailyGoalAni((float)state.value0,"normal", setFinish);
			}
			if (sleepBar != null)
			{
				sleepBar.dailyGoalAni((float)state.value0,"sleep", setFinish);
			}
			if (schoolBar != null)
            {
				schoolBar.dailyGoalAni((float)state.value0,"school", setFinish);
            }  

        }


        // Update is called once per frame
        void Update()
        {

        }

        void setFinish()
		{
			bFinish = true;
		}
    }
}

