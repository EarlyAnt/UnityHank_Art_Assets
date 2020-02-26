using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using strange.extensions.mediation.impl;
using Gululu.LocalData.Agents;
using Gululu;
using UnityEngine.Assertions;

namespace Hank.MainScene
{
    public class LevelUI : BaseView
    {
        [Inject]
        public IPlayerDataManager dataManager { get; set; }
        [Inject]
        public ILocalPetInfoAgent mLocalPetInfoAgent { get; set; }

        [SerializeField]
        Image imageExpProcess;

        [SerializeField]
        GameObject[] bgForRole;

        public override void TestSerializeField()
        {
            Assert.IsNotNull(imageExpProcess);
            Assert.AreEqual(bgForRole.Length,4);
            foreach(var obj in bgForRole)
            {
                Text testtextLevel = obj.transform.Find("TextLevel").GetComponent<Text>();
                Assert.IsNotNull(testtextLevel);
            }
        }


        Text textLevel;
        // Use this for initialization
        protected override void Start()
        {
            base.Start();
            ChangeRole();
            SetExp(dataManager.GetExpPercent());
            SetLevel(dataManager.playerLevel);

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetLevel(int level)
        {
            textLevel.text = level.ToString();
			//textLevel.text = "88";
//            Debug.LogError("主页Level:>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>"+imageExpProcess.fillAmount);
        }

        public void SetExp(float percent)
        {
            imageExpProcess.DOKill();
            imageExpProcess.fillAmount = percent;
//            Debug.LogError("主页经验值:>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>"+imageExpProcess.fillAmount);
            //imageExpProcess.DOFillAmount(percent, 0.5f);
        }

        public void ChangeRole()
        {
            string strType = mLocalPetInfoAgent.getCurrentPet();
            int type = 0;
            for (int i = 0; i < bgForRole.Length; i++)
            {
                if(strType == bgForRole[i].name)
                {
                    bgForRole[i].SetActive(true);
                    type = i;
                }
                else
                {
                    bgForRole[i].SetActive(false);
                }

            }
            if(type < bgForRole.Length)
            {
                textLevel = bgForRole[type].transform.Find("TextLevel").GetComponent<Text>();
            }
        }
    }
}
