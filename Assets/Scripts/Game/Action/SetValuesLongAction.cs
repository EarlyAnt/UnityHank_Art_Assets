using Gululu;
using Gululu.Config;
using Gululu.LocalData.Agents;
using Gululu.LocalData.DataManager;
//using Hank.AlertBoard;
using Hank.Api;
using Hank.MainScene;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hank.Action
{
    public class SetValuesLongAction : ActionBase
    {
        [Inject]
        public IJsonUtils JsonUtils { set; get; }
        [Inject]
        public IMissionConfig MissionConfig { get; set; }
        [Inject]
        public IDrinkWaterUIPresentManager uiState { get; set; }
        [Inject]
        public INewsEventManager newsEventManager { get; set; }
        [Inject]
        public ICupClockManager mCupClockManager { get; set; }
        [Inject]
        public ILocalChildInfoAgent mLocalChildInfoAgent { get; set; }
        //[Inject]
        //public AlertBoardSignal alertBoardSignal { get; set; }
        [Inject]
        public SilenceTimeProcessSignal silenceTimeProcessSignal { get; set; }

        HeartBeatSetValuesLong value;
        public override void Init(HeartBeatActionBase actionModel)
        {
            value = actionModel as HeartBeatSetValuesLong;
        }

        public override void DoAction()
        {
            bool error = false;

            Debug.Log("<><SetValuesLongAction> key:" + value.task.key + "    value:" + value.task.value);

            switch (value.task.key)
            {
                case "game_data":
                    {
                        GameData datas = JsonUtils.String2Json<GameData>(value.task.value);

                        fundDataManager.SetValuesLong(datas);
                        accessoryDataManager.SetValuesLong(datas);
                        if (datas.playerData != null)
                        {
                            if (datas.playerData.level == 0)
                            {

                            }
                            else if (datas.playerData.level >= dataManager.playerLevel)
                            {
                                dataManager.SetValuesLong(datas);
                                missionDataManager.SetValuesLong(datas);
                                itemsDataManager.SetValuesLong(datas);
                                Loom.QueueOnMainThread(() =>
                                {
                                    uiState.PushNewState(UIStateEnum.eRefreshMainByData);
                                });
                            }
                            else
                            {
                                error = true;
                            }
                        }
                    }
                    break;
                //case "config_times":
                //    {
                //        //先按照旧格式记录数据(一些“跨天”，是否防打扰模式的判断，等逻辑还在此脚本中)
                //        AllSilenceTimeSectionFromServer listSection = JsonUtils.String2Json<AllSilenceTimeSectionFromServer>(value.task.value);
                //        silenceTimeDataManager.InitialSilenceTimeSection(listSection.configs);
                //        //再按照新格式记录数据
                //        mCupClockManager.setOtherClock(mLocalChildInfoAgent.getChildSN(), value.task.value, () =>
                //        {
                //            GuLog.Info("set silence data success: " + value.task.value);
                //            Loom.QueueOnMainThread(() =>
                //            {
                //                Debug.LogFormat("----{0}----SetValuesLongAction.ShowAlertBoard: {1}",
                //                                System.DateTime.Now.ToString("HH:mm:ss:fff"), AlertType.AlertType_SilenceMode);
                //                this.alertBoardSignal.Dispatch(AlertType.AlertType_SilenceMode);

                //            });
                //        }, () =>
                //        {
                //            GuLog.Error("set silence data error: " + value.task.value);
                //        });
                //        //触发信号，检查当前时间是否处于防打扰模式
                //        Loom.QueueOnMainThread(() => { this.silenceTimeProcessSignal.Dispatch(); });
                //    }
                //    break;
                case "friend_list":
                    {
                    }
                    break;
                case "event_message":
                    {
                        GuEvents datas = JsonUtils.String2Json<GuEvents>(value.task.value);
                        newsEventManager.AddNewsEvent(datas);

                    }
                    break;
                //case "alarm_clock":
                //    {
                //        GuLog.Debug("<><SetValuesLongAction> alarm_clock");
                //        // GuEvents datas = mJsonUtils.String2Json<GuEvents>(value.task.value);
                //        // newsEventManager.AddNewsEvent(datas);
                //        mCupClockManager.setNetClock(mLocalChildInfoAgent.getChildSN(), value.task.value, () =>
                //        {
                //            GuLog.Info("set alarm success info: " + value.task.value);
                //            Loom.QueueOnMainThread(() =>
                //            {
                //                Debug.LogFormat("----{0}----SetValuesLongAction.ShowAlertBoard: {1}",
                //                                System.DateTime.Now.ToString("HH:mm:ss:fff"), AlertType.AlertType_ClockAlarm);
                //                alertBoardSignal.Dispatch(AlertType.AlertType_ClockAlarm);
                //            });
                //        }, () =>
                //        {
                //            GuLog.Error("set alarm error info: " + value.task.value);
                //        });
                //    }
                //    break;
                case "mission_data":
                    {
                        this.SetMissionData(value.task.value);
                    }
                    break;
                default:
                    {
                        error = true;
                    }
                    break;

            }
            if (error)
            {
                BuildBackAckBody(value.ack, value.task.key, "ERROR", value.todoid);
            }
            else
            {
                BuildBackAckBody(value.ack, "SetValuesLong", "OK", value.todoid);
            }

        }

        private void SetMissionData(string jsonData)
        {
            if (string.IsNullOrEmpty(jsonData)) return;

            try
            {
                Debug.LogFormat("<><SetValuesLongAction.SetMissionData>Set missionData: {0}", jsonData);
                MissionData missionData = this.JsonUtils.String2Json<MissionData>(jsonData);
                if (missionData != null)
                {
                    List<WorldInfo> worldInfos = this.MissionConfig.GetAllWorlds();
                    foreach (var worldInfo in worldInfos)
                    {
                        Star star = missionData.Stars.Find(t => t.ID == worldInfo.ID);
                        if (star != null)
                        {
                            Debug.LogFormat("<><SetValuesLongAction.SetMissionData>Star unlock 1, config: {0}, net: {1}", worldInfo.Unlock, star.Unlock);
                            worldInfo.Unlock = star.Unlock;
                            Debug.LogFormat("<><SetValuesLongAction.SetMissionData>Star unlock 2, openDate: {0}", worldInfo.OpenDate);
                            foreach (var sceneInfo in worldInfo.SceneInfos)
                            {
                                Scene scene = star.Scenes.Find(t => t.Index == sceneInfo.index);
                                if (scene != null)
                                {
                                    Debug.LogFormat("<><SetValuesLongAction.SetMissionData>Scene unlock 1, config: {0}, net: {1}", sceneInfo.Unlock, scene.Unlock);
                                    sceneInfo.Unlock = scene.Unlock;
                                    Debug.LogFormat("<><SetValuesLongAction.SetMissionData>Scene unlock 2, openDate: {0}", sceneInfo.OpenDate);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogErrorFormat("<><SetValuesLongAction.SetMissionData>Error: {0}", ex.Message);
            }
        }
    }
}
