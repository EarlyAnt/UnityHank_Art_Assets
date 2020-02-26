using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Spine.Unity;

namespace Hank.MainScene
{
	public class dailyProcessBar : MonoBehaviour
    {
		[SerializeField]
		Slider Slider1, Slider2, Slider3;
		[SerializeField]
		GameObject handle, handle1, handle2;
		[SerializeField]
        GameObject drinkAward1, drinkAward2, drinkAward3;
		[SerializeField]
		GameObject goldIcon;

//         // Use this for initialization
//         void Awake()
// 		{
//             setHandleStatus(false, false, false);
//         }

        // Update is called once per frame
        void Update()
        {

        }

		public delegate void FinishDelegate();

		public void dailyGoalAni(float percentOfDailyGoal, string barType, FinishDelegate callBack)
        {         
			if(handle.activeInHierarchy)
			{
				float process = (float)(percentOfDailyGoal / 0.3);
				Slider1.DOValue(process, 1.0f).onComplete = delegate() {
					if (barType != "normal")
					{
						setAwardAndHandle(percentOfDailyGoal);
					}
                    callBack();
                };
			}else if(handle1.activeInHierarchy)
			{
				Slider1.value = 1.0f;
				float process = (float)((percentOfDailyGoal - 0.3) / 0.3);
                Slider2.DOValue(process, 1.0f).onComplete = delegate () {
					if (barType != "normal")
                    {
                        setAwardAndHandle(percentOfDailyGoal);
                    }
					callBack();
                };
			}else if (handle2.activeInHierarchy)
            {
				Slider1.value = 1.0f;
				Slider2.value = 1.0f;
                float process = (float)((percentOfDailyGoal - 0.6) / 0.4);
                Slider3.DOValue(process, 1.0f).onComplete = delegate () {
					if (barType != "normal")
                    {
                        setAwardAndHandle(percentOfDailyGoal);
                    }
                    callBack();
                };
            }
        }

        void setHandleStatus(bool status, bool status1, bool status2)
        {
            handle.SetActive(status);
            handle1.SetActive(status1);
            handle2.SetActive(status2);
        }

        public void setAwardAndHandle(float dailyGoal)
		{
            drinkAward1.SetActive(dailyGoal > 0.3);
            drinkAward2.SetActive(dailyGoal > 0.6);
            drinkAward3.SetActive(dailyGoal >= 1);

            if (dailyGoal < 0.3)
            {
                setHandleStatus(true, false, false);
                Slider1.value = (float)(dailyGoal / 0.3);
                Slider2.value = 0;
                Slider3.value = 0;
            }
            else if (dailyGoal >= 0.3 && dailyGoal < 0.6)
            {
                setHandleStatus(false, true, false);
                Slider1.value = 1.0f;
                Slider2.value = (float)((dailyGoal - 0.3) / 0.3);
                Slider3.value = 0;
            }
            else if (dailyGoal >= 0.6 && dailyGoal < 1.0)
            {
                setHandleStatus(false, false, true);
                Slider1.value = 1.0f;
                Slider2.value = 1.0f;
                Slider3.value = (float)((dailyGoal - 0.6) / 0.4);
            }
            else if (dailyGoal >= 1.0)
            {
                setHandleStatus(false, false, false);
                Slider1.value = 1.0f;
                Slider2.value = 1.0f;
                Slider3.value = 1.0f;
            }

//             drinkAward1.SetActive(dailyGoal >= 0.3);
//             drinkAward2.SetActive(dailyGoal >= 0.6);
//             drinkAward3.SetActive(dailyGoal >= 1);
//             handle.SetActive(dailyGoal <= 0.3);
//             handle1.SetActive(dailyGoal > 0.3 && dailyGoal <= 0.6);
//             handle2.SetActive(dailyGoal > 0.6 && dailyGoal <= 1);
            if(goldIcon != null)
            {
                goldIcon.SetActive(dailyGoal >= 1);
            }
        }
    }
}

