using Gululu;
using Gululu.Config;
using Gululu.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using strange.extensions.mediation.impl;
using UnityEngine.Assertions;

namespace Hank.MainScene
{
    public class MissionName : BaseView
    {
        [Inject]
        public IPlayerDataManager dataManager { get; set; }
        [Inject]
        public IMissionConfig missionConifg { get; set; }
        [Inject]
        public ILangManager mLangManager { get; set; }
        [Inject]
        public IPrefabRoot PrefabRoot { get; set; }

        [SerializeField]
        Text textName;
        [SerializeField]
        Animator aniMissionLimitImage;
        [SerializeField]
        Image imageBg;

        public override void TestSerializeField()
        {
            Assert.IsNotNull(textName);
            Assert.IsNotNull(aniMissionLimitImage);
            Assert.IsNotNull(imageBg);
        }


        // Use this for initialization
        override protected void Start()
        {
            base.Start();
            RefreshMissionName(dataManager.IsMissionLimit());
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void RefreshMissionName(bool isMissionLimit)
        {
            string titleBg = missionConifg.GetCurrentTitleBg(ShowMissionHelper.Instance.currShowMissionId);
            if (titleBg != string.Empty)
            {
                imageBg.sprite = this.PrefabRoot.GetObjectNoInstantiate<Sprite>("Texture/Title", titleBg);
            }
            if (dataManager.missionId == ShowMissionHelper.Instance.currShowMissionId
                && isMissionLimit
            )
            {
                aniMissionLimitImage.gameObject.SetActive(true);
                textName.gameObject.SetActive(false);
            }
            else
            {
                aniMissionLimitImage.gameObject.SetActive(false);
                textName.gameObject.SetActive(true);
                MissionInfo info = missionConifg.GetMissionInfoByMissionId(ShowMissionHelper.Instance.currShowMissionId);
                if (info != null)
                {
                    textName.text = mLangManager.GetLanguageString(info.MissionName);
                    textName.color = missionConifg.GetCurrentTitleTextColor(info.missionId);
                }
            }
        }

        public void ShowMissionLimitSpark()
        {
            RefreshMissionName(true);
            aniMissionLimitImage.Play("MissionLimitSpark");
        }

        public bool IsAniIdle()
        {
            AnimatorStateInfo info = (aniMissionLimitImage.GetCurrentAnimatorStateInfo(0));
            return info.IsName("MissionLimitNormal");
        }
    }
}
