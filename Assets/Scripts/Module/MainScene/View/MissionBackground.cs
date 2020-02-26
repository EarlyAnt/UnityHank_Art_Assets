using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hank;
using Gululu.Config;
using Gululu.Util;
using strange.extensions.mediation.impl;
using Cup.Utils.audio;
using Gululu;
using UnityEngine.Assertions;

namespace Hank.MainScene
{
    public class MissionBackground : BaseView
    {
        [Inject]
        public IPrefabRoot mPrefabRoot { get; set; }

        [SerializeField]
        SpriteRenderer currentBackgroud;
        [SerializeField]
        SpriteRenderer nextBackgroud;
        [Inject]
        public IPlayerDataManager dataManager { get; set; }
        [Inject]
        public IMissionDataManager missionDataManager { get; set; }
        [Inject]
        public IMissionConfig missionConifg { get; set; }

        [SerializeField]
        UnlockCover unlockCover;

        public override void TestSerializeField()
        {
            Assert.IsNotNull(currentBackgroud);
            Assert.IsNotNull(nextBackgroud);
            Assert.IsNotNull(unlockCover);

        }

        // Use this for initialization
        protected override void Start()
        {
            base.Start();
            ShowMissionHelper.Instance.currShowMissionId = dataManager.missionId;
            RefreshBackground(currentBackgroud, ref ShowMissionHelper.Instance.objCurrModel, ShowMissionHelper.Instance.currShowMissionId);
            nextBackgroud.gameObject.SetActive(false);
            currentBackgroud.gameObject.SetActive(true);
        }


        void setBackGroudSprite(SpriteRenderer sRenderer, ref MeshRenderer objModel, int desMissionId)
        {
            if (objModel != null)
            {
                Destroy(objModel.gameObject);
                objModel = null;
            }
#if (!ALLMISSIONOPEN)
            if (missionDataManager.GetMisionState(desMissionId) == MissionState.eLock)
            {
                unlockCover.ShowLock(true);
            }
            else
#endif
            {
                unlockCover.ShowLock(false);
            }

            if (desMissionId != PlayerData.sleepMission)
            {//普通关卡切换背景图片和小生物
                MissionInfo info = missionConifg.GetMissionInfoByMissionId(desMissionId);
                if (info != null)
                {
                    GameObject resource = mPrefabRoot.GetGameObject("Scenes", info.strBackground);
                    if (resource && resource.GetComponent<SpriteRenderer>() != null)
                    {
                        sRenderer.sprite = resource.GetComponent<SpriteRenderer>().sprite;
                        Destroy(resource);
                    }
                    resource = mPrefabRoot.GetGameObject("Scenes/Model", info.strModel);
                    if (resource != null)
                    {
                        objModel = resource.GetComponent<MeshRenderer>();
                        objModel.transform.SetParent(sRenderer.transform, false);
                    }
                }
            }
            else
            {//睡眠关卡只切换背景图片
                WorldInfo worldInfo = this.missionConifg.GetWorldByStarID(this.dataManager.StarID);
                if (worldInfo != null)
                {
                    GameObject resource = mPrefabRoot.GetGameObject("Scenes", worldInfo.SleepBackgournd);
                    if (resource && resource.GetComponent<SpriteRenderer>() != null)
                    {
                        sRenderer.sprite = resource.GetComponent<SpriteRenderer>().sprite;
                        Destroy(resource);
                    }
                }
            }
        }

        public void ChangeCurrBackGroudSprite(int desMissionId)
        {
            ShowMissionHelper.Instance.currShowMissionId = desMissionId;
            setBackGroudSprite(currentBackgroud, ref ShowMissionHelper.Instance.objCurrModel, desMissionId);
            PlayNimAnimation(-1, desMissionId);
        }

        public void RefreshCurrentBackground(int currMissionId)
        {
            if (currMissionId != ShowMissionHelper.Instance.currShowMissionId)
            {
                ShowMissionHelper.Instance.currShowMissionId = currMissionId;
                setBackGroudSprite(currentBackgroud, ref ShowMissionHelper.Instance.objCurrModel, currMissionId);
            }
            PlayNimAnimation(ShowMissionHelper.Instance.objCurrModel, currMissionId);
        }

        public void RefreshBackground(SpriteRenderer sRenderer, ref MeshRenderer objModel, int desMissionId)
        {
            setBackGroudSprite(sRenderer, ref objModel, desMissionId);
            PlayNimAnimation(objModel, desMissionId);
        }


        //用于uiPresent;
        public void PlayNimAnimation(int index, int desMissionId)
        {
            MeshRenderer objCurrModel = ShowMissionHelper.Instance.objCurrModel;
            if (objCurrModel != null)
            {
                Spine.Unity.SkeletonAnimation skeletonAnimation = objCurrModel.GetComponent<Spine.Unity.SkeletonAnimation>();
                if (index == 1)
                {
                    objCurrModel.gameObject.SetActive(true);
                    //                     skeletonAnimation.state.ClearTracks(); can`t use 
                    skeletonAnimation.state.SetAnimation(0, "animation02", true);
                }
                else if (index == 0)
                {
                    objCurrModel.gameObject.SetActive(true);
                    skeletonAnimation.state.ClearTracks();
                    skeletonAnimation.state.SetAnimation(0, "animation01", true);
                }
                else
                {
                    objCurrModel.gameObject.SetActive(false);
                }
            }

        }

        private void PlayNimAnimation(MeshRenderer objModel, int desMissionId)
        {
            if (objModel)
            {
                Spine.Unity.SkeletonAnimation skeletonAnimation = objModel.GetComponent<Spine.Unity.SkeletonAnimation>();
#if ALLMISSIONOPEN
                int voicePercent = 15;
#if (UNITY_ANDROID) && (!UNITY_EDITOR)
                voicePercent = AudioControl.getVolume();
#endif
                if (voicePercent < 7)
                {
                    objModel.gameObject.SetActive(true);
                    skeletonAnimation.state.SetAnimation(0, "animation01", true);
                }
                else
                {
                    objModel.gameObject.SetActive(true);
                    skeletonAnimation.state.SetAnimation(0, "animation02", true);
                }
#else
                if (missionDataManager.IsTreasureOpen(desMissionId, 1))
                {
                    objModel.gameObject.SetActive(true);
                    skeletonAnimation.state.ClearTracks();
                    skeletonAnimation.state.SetAnimation(0, "animation02", true);
                }
                else if (missionDataManager.IsTreasureOpen(desMissionId, 0))
                {
                    objModel.gameObject.SetActive(true);
                    skeletonAnimation.state.ClearTracks();
                    skeletonAnimation.state.SetAnimation(0, "animation01", true);
                }
                else
                {
                    objModel.gameObject.SetActive(false);
                }
#endif
            }
        }

    }
}

