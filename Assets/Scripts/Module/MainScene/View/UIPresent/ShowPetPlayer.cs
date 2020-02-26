using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Gululu.Config;
using Gululu.Util;
using Cup.Utils.Touch;
using Gululu.LocalData.Agents;
using UnityEngine.Assertions;

namespace Hank.MainScene
{
    public class ShowPetPlayer : MainPresentBase
    {
        [Inject]
        public ShowPetPlayerSignal ShowPetPlayerSignal { get; set; }
        [Inject]
        public IPlayerDataManager DataManager { get; set; }//数据管理
        [Inject]
        public IMissionConfig MissionConfig { get; set; }//Mission任务配置
        [Inject]
        public IMissionDataManager MissionDataManager { get; set; }//任务数据管理

        protected override void Start()
        {
            base.Start();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }

        private void Update()
        {
            if (!TopViewHelper.Instance.IsTopView(TopViewHelper.ETopView.eAnimalPlayer))
                this.bFinish = true;
        }

        public override UIStateEnum GetStateEnum()
        {
            return UIStateEnum.eShowPetPlayer;
        }

        public override void ProcessState(UIState state)
        {
            if (!this.IsFirstPet())
                this.ShowPetPlayerSignal.Dispatch();
            else
                this.bFinish = true;
        }

        private bool IsFirstPet()
        {
            if (this.DataManager.missionId == 11001 &&
                this.MissionDataManager.IsTreasureOpen(this.DataManager.missionId, 0) &&
                !this.MissionDataManager.IsTreasureOpen(this.DataManager.missionId, 1))
                return true;
            else
                return false;
        }
    }
}

