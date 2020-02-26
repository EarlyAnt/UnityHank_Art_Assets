using Cup.Utils.android;
using Gululu;
using Gululu.Config;
using Gululu.LocalData.Agents;
using Hank.Api;
using Hank.MainScene;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Hank.Action
{
    public class SetValuesAction : ActionBase
    {
        #region 注入接口
        [Inject]
        public IDrinkWaterUIPresentManager uiState { get; set; }
        [Inject]
        public ITimeManager timeManager { get; set; }
        [Inject]
        public ILanguageUtils languageUtils { get; set; }
        [Inject]
        public ILocalPetInfoAgent mLocalPetInfoAgent { get; set; }
        [Inject]
        public IInteractiveCountManager interactiveCountManager { get; set; }
        [Inject]
        public IFundDataManager FundDataManager { get; set; }
        public IPropertyConfig PropertyConfig { get; set; }
        [Inject]
        public IMissionConfig MissionConfig { get; set; }
        [Inject]
        public ILanConfig LanConfig { get; set; }
        [Inject]
        public ILocalPetAccessoryAgent LocalPetAccessoryAgent { get; set; }
        [Inject]
        public SilenceTimeProcessSignal silenceTimeProcessSignal { get; set; }
        [Inject]
        public ShowLanguageSignal ShowLanguageSignal { get; set; }
        #endregion
        #region 其他变量
        private HeartBeatSetValues value;
        #endregion

        //初始化
        public override void Init(HeartBeatActionBase actionModel)
        {
            value = actionModel as HeartBeatSetValues;
        }
        //执行后台指令
        public override void DoAction()
        {
            string ErrMsg = "";
            bool error = false;
            bool needSync = false;
            foreach (var val in value.task)
            {
                GuLog.Debug("<><SetValuesAction> key:" + val.key + "   value:" + val.value);
                switch (val.key)
                {
                    case "todayDate":
                        {
                            dataManager.date = Convert.ToInt32(val.value);
                        }
                        break;
                    case "todayIntake":
                        {
                            dataManager.currIntake = Convert.ToInt32(val.value);
                        }
                        break;
                    case "todayCrownGotCount":
                        {
                            dataManager.todayCrownGotCount = Convert.ToInt32(val.value);
                        }
                        break;
                    case "Level":
                        {
                            dataManager.playerLevel = Convert.ToInt32(val.value);
                            needSync = true;
                        }
                        break;
                    case "Exp":
                        {
                            dataManager.exp = Convert.ToInt32(val.value);
                            needSync = true;
                        }
                        break;
                    case "CrownCount":
                        {
                            dataManager.CrownCount = Convert.ToInt32(val.value);
                            needSync = true;
                        }
                        break;
                    case "PlayerMissionId":
                        {
                            dataManager.missionId = Convert.ToInt32(val.value);
                            needSync = true;
                        }
                        break;
                    case "PlayerMissionPercent":
                        {
                            dataManager.missionPercent = Convert.ToInt32(val.value) / (float)100;
                            needSync = true;
                        }
                        break;
                    case "timestamp":
                        {
                        }
                        break;
                    case "timezone":
                        {
                            timeManager.setTimeZone(val.value);
                            Loom.QueueOnMainThread(() =>
                            {
                                silenceTimeProcessSignal.Dispatch();
                            });
                        }
                        break;
                    case "language":
                        {
                            languageUtils.setLanguageByAgentKey(val.value);
                        }
                        break;
                    //case "recommend_water":
                    //    {
                    //        dataManager.SetDailyGoal(Convert.ToInt32(val.value));
                    //        Loom.QueueOnMainThread(() =>
                    //        {
                    //            Debug.LogFormat("----{0}----SetValuesLongAction.ShowAlertBoard: {1}",
                    //                            System.DateTime.Now.ToString("HH:mm:ss:fff"), AlertBoard.AlertType.AlertType_DailyGoal);
                    //            alertBoardSignal.Dispatch(AlertBoard.AlertType.AlertType_DailyGoal);
                    //        });
                    //    }
                    //    break;
                    case "reset_guide":
                        {
                            interactiveCountManager.Reset();
                        }
                        break;
                    case "next_pet":
                        {
                            mLocalPetInfoAgent.saveCurrentPet(val.value);
                            Loom.QueueOnMainThread(() =>
                            {
                                uiState.PushNewState(UIStateEnum.eRefreshMainByData);
                                uiState.PushNewState(UIStateEnum.eRolePlayAction, "hello", 0, false);
                                uiState.PushNewState(UIStateEnum.eStartDailyProcessBar);
                                uiState.ProcessStateRightNow();
                            });
                        }
                        break;
                    case "cup_language":
                        {
                            if (this.LanConfig.IsValid(val.value))
                            {
                                Debug.LogFormat("<><SetValuesAction.DoAction>Value: {0}, Current language: {1}", val.value, this.dataManager.GetLanguage());
                                this.dataManager.SaveLanguage(val.value);
                                Debug.LogFormat("<><SetValuesAction.DoAction>Current language: {0}", this.dataManager.GetLanguage());
                            }
                            else if (!string.IsNullOrEmpty(val.value) && val.value.ToUpper() == "RESET")
                            {
                                Debug.LogFormat("<><SetValuesAction.DoAction>Reset language, current language: {0}", this.dataManager.GetLanguage());
                                Loom.QueueOnMainThread(() =>
                                {
                                    this.ShowLanguageSignal.Dispatch();
                                });
                            }
                        }
                        break;
                    case "set_coin":
                        {
                            int count = -1;
                            if (string.IsNullOrEmpty(val.value) || !int.TryParse(val.value, out count))
                            {
                                error = true;
                                ErrMsg = string.Format("<><SetValuesAction.DoAction>Add or expend coin, value format error: {0}", val.value);
                                break;
                            }
                            int currentCoin = this.FundDataManager.GetItemCount(this.FundDataManager.GetCoinId());
                            if (count > 0)
                            {
                                this.FundDataManager.AddCoin(this.FundDataManager.GetCoinId(), count);
                                Debug.LogFormat("<><SetValuesAction.DoAction>Add coin, current: {0}, add: {1}, total: {2}", currentCoin, count, this.FundDataManager.GetItemCount(this.FundDataManager.GetCoinId()));
                            }
                            else if (count < 0)
                            {
                                this.FundDataManager.ExpendCoin(this.FundDataManager.GetCoinId(), -count);
                                Debug.LogFormat("<><SetValuesAction.DoAction>Expend coin, current: {0}, expend: {1}, total: {2}", currentCoin, -count, this.FundDataManager.GetItemCount(this.FundDataManager.GetCoinId()));
                            }
                        }
                        break;
                    case "set_prop":
                        {
                            string value = val.value != null ? val.value.ToString() : "";
                            int id = -1;
                            int count = -1;
                            string[] parts = value.Split('|');
                            if (parts == null || parts.Length != 2 || string.IsNullOrEmpty(parts[0]) || string.IsNullOrEmpty(parts[1]) ||
                                !int.TryParse(parts[0], out id) || !int.TryParse(parts[1], out count))
                            {
                                error = true;
                                ErrMsg = string.Format("<><SetValuesAction.DoAction>Add or expend props, value format error: {0}", val.value);
                                break;
                            }
                            int currentProps = this.FundDataManager.GetItemCount(id);
                            if (count > 0)
                            {
                                this.FundDataManager.AddProperty(id, count);
                                Debug.LogFormat("<><SetValuesAction.DoAction>Add props, current: {0}, add: {1}, total: {2}", currentProps, count, this.FundDataManager.GetItemCount(id));
                            }
                            else if (count < 0)
                            {
                                this.FundDataManager.ExpendProperty(id, -count);
                                Debug.LogFormat("<><SetValuesAction.DoAction>Expend props, current: {0}, expend: {1}, total: {2}", currentProps, -count, this.FundDataManager.GetItemCount(id));
                            }
                        }
                        break;
                    case "goto_mission":
                        {
                            string errorText = "";
                            if (!this.SetMissionValue(val.value, ref errorText))
                            {
                                error = true;
                                ErrMsg += string.Format("<><SetValuesAction.DoAction>{0}", errorText);
                            }
                        }
                        break;
                    case "clear_accessory":
                        {
                            string errorText = "";
                            if (!this.ClearPetAccessories(val.value, ref errorText))
                            {
                                error = true;
                                ErrMsg += string.Format("<><SetValuesAction.DoAction>{0}", errorText);
                            }
                        }
                        break;
                    default:
                        {
                            if (val.key.StartsWith(MissionDataManager.missionKey))
                            {
                                string strMissionId = val.key.Substring(MissionDataManager.missionKey.Length);
                                int missionId = Convert.ToInt32(strMissionId);
                                missionDataManager.SetMissionStateNot2Server(missionId, val.value);
                                needSync = true;
                            }
                            else if (val.key.StartsWith(ItemsDataManager.itemsKey))
                            {
                                string strItemsId = val.key.Substring(ItemsDataManager.itemsKey.Length);
                                int itemId = Convert.ToInt32(strItemsId);
                                itemsDataManager.SetItem(itemId, Convert.ToInt32(val.value));
                                needSync = true;
                            }
                            else
                            {
                                error = true;
                                ErrMsg += val.key + "";
                            }
                        }
                        break;

                }
            }
            if (needSync)
            {
                missionDataManager.SendAllMissionData(true);
                dataManager.TrySendPlayerData2Server();
                itemsDataManager.SendAllItemsData(true);
            }
            if (error)
            {
                BuildBackAckBody(value.ack, ErrMsg, "ERROR", value.todoid);
                Debug.LogErrorFormat(ErrMsg);
            }
            else
            {
                BuildBackAckBody(value.ack, "SetValues", "OK", value.todoid);
            }
            Loom.QueueOnMainThread(() =>
            {
                uiState.PushNewState(UIStateEnum.eRefreshMainByData);
            });
        }
        //跳转关卡
        private bool SetMissionValue(string missionValue, ref string errorText)
        {
            int missionID = -1;
            if (!int.TryParse(missionValue, out missionID))
            {
                errorText = string.Format("Invalid net value, it's not numeric: {0}", missionValue);
                Debug.LogErrorFormat("<><SetValuesAction.SetMissionValue>{0}", errorText);
                return false;
            }
            else if (!this.MissionConfig.ExistMission(missionID))
            {
                errorText = string.Format("MissionID is not existed: {0}", missionValue);
                Debug.LogErrorFormat("<><SetValuesAction.SetMissionValue>{0}", errorText);
                return false;
            }

            try
            {
                int starID = this.MissionConfig.GetStarIDByMissionID(missionID);
                WorldInfo worldInfo = this.MissionConfig.GetWorldByStarID(starID);
                if (worldInfo == null || worldInfo.SceneInfos == null)
                {
                    errorText = string.Format("Star is not existed: {0}", starID);
                    Debug.LogErrorFormat("<><SetValuesAction.SetMissionValue>{0}", errorText);
                    return false;
                }
                else if (!this.MissionConfig.ExistMission(missionID))
                {
                    errorText = string.Format("Mission is not existed: {0}", missionID);
                    Debug.LogErrorFormat("<><SetValuesAction.SetMissionValue>{0}", errorText);
                    return false;
                }

                int sceneIndex = this.MissionConfig.GetSceneIndexByMissionID(missionID);
                sceneIndex = Mathf.Clamp(sceneIndex, 1, worldInfo.SceneInfos.Count);
                StringBuilder strbContent = new StringBuilder();
                bool found = false;
                for (int i = 0; i < worldInfo.SceneInfos.Count; i++)
                {
                    if (worldInfo.Type == WorldInfo.WorldTypes.Duplicate && worldInfo.SceneInfos[i].index != sceneIndex)
                        continue;

                    for (int j = 0; j < worldInfo.SceneInfos[i].missionInfos.Count; j++)
                    {
                        int id = worldInfo.SceneInfos[i].missionInfos[j].missionId;
                        this.missionDataManager.AddMissionState(id, id < missionID ? MissionState.eComplete : MissionState.eUnderway);
                        strbContent.AppendFormat("ID: {0}, State: {1}\n", id, id < missionID ? MissionState.eComplete : MissionState.eUnderway);
                        if (id >= missionID) { found = true; break; }
                    }
                    if (found) break;
                }
                this.dataManager.StarID = starID;
                this.dataManager.missionId = missionID;
                uiState.PushNewState(UIStateEnum.eNewMissionSwitch, dataManager.missionId);
                uiState.PushNewState(UIStateEnum.eRolePlayAction, "level_start", 0, false);
                uiState.PushNewState(UIStateEnum.eStartDailyProcessBar);
                Debug.LogFormat("<><SetValuesAction.SetMissionValue>Datas: \n{0}", strbContent.ToString());
            }
            catch (Exception ex)
            {
                errorText = string.Format("Unknown error: {0}", ex.Message);
                Debug.LogErrorFormat("<><SetValuesAction.SetMissionValue>{0}", errorText);
                return false;
            }
            return true;
        }
        //清除小宠物配饰
        private bool ClearPetAccessories(string petName, ref string errorText)
        {
            if (string.IsNullOrEmpty(petName))
            {
                errorText = "Invalid net value, parameter[petName] is null or empty";
                Debug.LogErrorFormat("<><SetValuesAction.ClearPetAccessories>{0}", errorText);
                return false;
            }

            try
            {
                bool matched = false;
                switch (petName.ToUpper())
                {
                    case Roles.Purpie:
                        this.LocalPetAccessoryAgent.SavePetAccessories(Roles.Purpie, new List<string>());
                        matched = true;
                        break;
                    case Roles.Donny:
                        this.LocalPetAccessoryAgent.SavePetAccessories(Roles.Donny, new List<string>());
                        matched = true;
                        break;
                    case Roles.Ninji:
                        this.LocalPetAccessoryAgent.SavePetAccessories(Roles.Ninji, new List<string>());
                        matched = true;
                        break;
                    case Roles.Sansa:
                        this.LocalPetAccessoryAgent.SavePetAccessories(Roles.Sansa, new List<string>());
                        matched = true;
                        break;
                    case Roles.Yoyo:
                        this.LocalPetAccessoryAgent.SavePetAccessories(Roles.Yoyo, new List<string>());
                        matched = true;
                        break;
                    case Roles.Nuo:
                        this.LocalPetAccessoryAgent.SavePetAccessories(Roles.Nuo, new List<string>());
                        matched = true;
                        break;
                    case "ALL":
                        this.LocalPetAccessoryAgent.SavePetAccessories(Roles.Purpie, new List<string>());
                        this.LocalPetAccessoryAgent.SavePetAccessories(Roles.Donny, new List<string>());
                        this.LocalPetAccessoryAgent.SavePetAccessories(Roles.Ninji, new List<string>());
                        this.LocalPetAccessoryAgent.SavePetAccessories(Roles.Sansa, new List<string>());
                        this.LocalPetAccessoryAgent.SavePetAccessories(Roles.Yoyo, new List<string>());
                        this.LocalPetAccessoryAgent.SavePetAccessories(Roles.Nuo, new List<string>());
                        matched = true;
                        break;
                }

                if (matched)
                {
                    this.dataManager.TrySendPlayerData2Server();
                    RoleManager.Instance.Reset(false);
                    return true;
                }
                else
                {
                    errorText = string.Format("Invalid net value, pet[{0}] is not existed", petName);
                    Debug.LogErrorFormat("<><SetValuesAction.ClearPetAccessories>{0}", errorText);
                    return false;
                }
            }
            catch (Exception ex)
            {
                errorText = string.Format("Unknown error: {0}", ex.Message);
                Debug.LogErrorFormat("<><SetValuesAction.ClearPetAccessories>{0}", errorText);
                return false;
            }
        }
    }
}
