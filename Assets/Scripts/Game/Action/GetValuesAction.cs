using System;
using System.Collections;
using System.Collections.Generic;
using Gululu;
using Hank.Api;
using UnityEngine;

namespace Hank.Action
{
    public class GetValuesAction : ActionBase
    {
        [Inject]
        public IDrinkWaterUIPresentManager uiState { get; set; }
        [Inject]
        public ILanguageUtils languageUtils { get; set; }

        HeartBeatGetValues value;
        public override void Init(HeartBeatActionBase actionModel)
        {
            value = actionModel as HeartBeatGetValues;
        }


        public override void DoAction()
        {
            string ErrMsg = "";
            bool error = false;
            List<ActionPostBodyValue> values = new List<ActionPostBodyValue>();
            foreach (var val in value.task)
            {
                switch (val.key)
                {
                    case "todayDate":
                        {
                            values.Add(ActionPostBodyValue.getBuilder().setKey(val.key).setValue(dataManager.date.ToString()).build());
                        }
                        break;
                    case "todayIntake":
                        {
                            values.Add(ActionPostBodyValue.getBuilder().setKey(val.key).setValue(dataManager.currIntake.ToString()).build());
                        }
                        break;
                    case "Level":
                        {
                            values.Add(ActionPostBodyValue.getBuilder().setKey(val.key).setValue(dataManager.playerLevel.ToString()).build());
                        }
                        break;
                    case "Exp":
                        {
                            values.Add(ActionPostBodyValue.getBuilder().setKey(val.key).setValue(dataManager.exp.ToString()).build());
                        }
                        break;
                    case "crownTodayGotCount":
                        {
                            values.Add(ActionPostBodyValue.getBuilder().setKey(val.key).setValue(dataManager.todayCrownGotCount.ToString()).build());
                        }
                        break;
                    case "PlayerMissionId":
                        {
                            values.Add(ActionPostBodyValue.getBuilder().setKey(val.key).setValue(dataManager.missionId.ToString()).build());
                        }
                        break;
                    case "PlayerMissionPercent":
                        {
                            int percent = dataManager.missionPercent > 1 ? 100 : (int)dataManager.missionPercent * 100;
                            values.Add(ActionPostBodyValue.getBuilder().setKey(val.key).setValue(percent.ToString()).build());
                        }
                        break;
                    case "CrownCount":
                        {
                            values.Add(ActionPostBodyValue.getBuilder().setKey(val.key).setValue(dataManager.CrownCount.ToString()).build());
                        }
                        break;
                    case "language":
                        {
                            values.Add(ActionPostBodyValue.getBuilder().setKey(val.key).setValue(languageUtils.getLanguageToAgentKey()).build());
                        }
                        break;
                    case "CurStarID":
                        {
                            values.Add(ActionPostBodyValue.getBuilder().setKey(val.key).setValue(dataManager.StarID.ToString()).build());
                            Debug.LogFormat("<><GetValuesAction.DoAction>StarID: {0}", dataManager.StarID);
                        }
                        break;
                    default:
                        {
                            if (val.key.StartsWith(MissionDataManager.missionKey))
                            {
                                string strMissionId = val.key.Substring(MissionDataManager.missionKey.Length);
                                int missionId = Convert.ToInt32(strMissionId);
                                values.Add(ActionPostBodyValue.getBuilder().setKey(val.key).setValue(missionDataManager.GetMisionState(missionId).ToString()).build());
                            }
                            else if (val.key.StartsWith(ItemsDataManager.itemsKey))
                            {
                                string strItemsId = val.key.Substring(ItemsDataManager.itemsKey.Length);
                                int itemId = Convert.ToInt32(strItemsId);
                                values.Add(ActionPostBodyValue.getBuilder().setKey(val.key).setValue(itemsDataManager.GetItemCount(itemId).ToString()).build());
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
            if (error)
            {
                BuildBackAckBody(value.ack, ErrMsg, "ERROR", value.todoid, values);
            }
            else
            {
                BuildBackAckBody(value.ack, "GetValues", "OK", value.todoid, values);
            }
        }
    }
}
