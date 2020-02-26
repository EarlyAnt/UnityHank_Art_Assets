using Gululu;
using Gululu.Config;
using Gululu.Events;
using Gululu.LocalData.Agents;
using strange.extensions.signal.impl;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Hank.MainScene
{
    public class DebugView : BaseView
    {
        /************************************************属性与变量命名************************************************/
        [SerializeField]
        private int missionId;
        [Inject]
        public IMissionConfig MissionConfig { get; set; }
        [Inject]
        public IMissionDataManager MissionDataManager { get; set; }
        [Inject]
        public IPlayerDataManager PlayerDataManager { get; set; }
        [Inject]
        public ILocalPetInfoAgent LocalPetInfoAgent { get; set; }
        [Inject]
        public ILocalUnlockedPetsAgent LocalUnlockedPetsAgent { get; set; }
        [Inject]
        public IDrinkWaterUIPresentManager UIState { get; set; }
        public Signal<MainEvent> ChangeMissionIdSignal = new Signal<MainEvent>();
        /************************************************Unity方法与事件***********************************************/
        protected override void Start()
        {
            base.Start();
#if UNITY_EDITOR
            this.CreateIncorrectPetName();
#endif
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
        }
        /************************************************自 定 义 方 法************************************************/
        private void CreateIncorrectPetName()
        {
            List<string> unlockedPetNames = this.LocalUnlockedPetsAgent.GetUnlockedPets();
            List<string> incorrectPetNames = new List<string>();
            StringBuilder strbNames = new StringBuilder("====DebugView.CreateIncorrectPetName.UnlockedPetNames====\n");
            unlockedPetNames.ForEach(t =>
            {
                t = t + "2";
                incorrectPetNames.Add(t);
                strbNames.AppendFormat("{0}, ", t);
            });
            strbNames.Remove(strbNames.Length - 2, 2);
            GuLog.Debug(strbNames.ToString());
            this.LocalUnlockedPetsAgent.SaveUnlockedPets(incorrectPetNames);

            string currentPetName = this.LocalPetInfoAgent.getCurrentPet() + "2";
            this.LocalPetInfoAgent.saveCurrentPet(currentPetName);
            GuLog.Debug(string.Format("----DebugView.CreateIncorrectPetName.CurrentPetName----{0}", currentPetName));
        }
        [ContextMenu("跳转到指定关卡")]
        private void SetMission()
        {
            try
            {
                int starID = this.MissionConfig.GetStarIDByMissionID(this.missionId);
                WorldInfo worldInfo = this.MissionConfig.GetWorldByStarID(starID);
                if (worldInfo == null) return;
                int sceneIndex = this.MissionConfig.GetSceneIndexByMissionID(this.missionId);
                if (sceneIndex < 0) return;
                StringBuilder strbContent = new StringBuilder();
                bool found = false;
                for (int i = 0; i < worldInfo.SceneInfos.Count; i++)
                {
                    if (worldInfo.Type == WorldInfo.WorldTypes.Duplicate && worldInfo.SceneInfos[i].index != sceneIndex)
                        continue;

                    for (int j = 0; j < worldInfo.SceneInfos[i].missionInfos.Count; j++)
                    {
                        int id = worldInfo.SceneInfos[i].missionInfos[j].missionId;
                        this.MissionDataManager.AddMissionState(id, id < this.missionId ? MissionState.eComplete : MissionState.eUnderway);
                        strbContent.AppendFormat("ID: {0}, State: {1}\n", id, id < this.missionId ? MissionState.eComplete : MissionState.eUnderway);
                        if (id >= this.missionId) { found = true; break; }
                    }
                    if (found) break;
                }
                this.PlayerDataManager.StarID = starID;
                this.PlayerDataManager.missionId = this.missionId;
                this.UIState.PushNewState(UIStateEnum.eNewMissionSwitch, this.PlayerDataManager.missionId);
                this.UIState.PushNewState(UIStateEnum.eRolePlayAction, "level_start", 0, false);
                this.UIState.PushNewState(UIStateEnum.eStartDailyProcessBar);
                Debug.LogFormat("<><DebugView.SetMission>Datas: \n{0}", strbContent.ToString());
            }
            catch (Exception ex)
            {
                Debug.LogErrorFormat("<><DebugView.SetMission>Unknown error: {0}", ex.Message);
            }
        }
    }
}