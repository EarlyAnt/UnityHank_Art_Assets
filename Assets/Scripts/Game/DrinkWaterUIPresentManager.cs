using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using strange.extensions.mediation.impl;
namespace Hank
{
    public class DrinkWaterUIPresentManager : IDrinkWaterUIPresentManager
    {
        //MainPresentBase[] allPresents;

        public MainPresentBase[] allIPresents { get; set; }

        bool IsSilenceMode(UIStateEnum eState)
        {
            return eState == UIStateEnum.eSchoolMode
                || eState == UIStateEnum.eSleepMode
                //|| eState == UIStateEnum.eOtherMode
                ;
        }

        public MainPresentBase CurrentPresent { get; set; }

        static UIState idleState = new UIState
        {
            state = UIStateEnum.eIdle,
            value0 = 0,
            value1 = 0,
            needWait = true
        };

        private List<UIState> _listState = new List<UIState>();

        public List<UIState> listState { get { return _listState; } }

        UIState GetNextState()
        {
            if (listState.Count > 0)
            {
                UIState ret = listState[0];
                listState.RemoveAt(0);
                return ret;
            }
            return idleState;
        }

        MainPresentBase GetPresentByEnum(UIStateEnum state)
        {
            if (allIPresents == null)
                return null;
            foreach (var ret in allIPresents)
            {
                if (state == ret.GetStateEnum())
                {
                    GuLog.Debug("<><DrinkWaterUIPresentManager> NextState:" + state.ToString());
                    return ret;
                }
            }
            return null;
        }

        public bool IsExistState(UIStateEnum eState)
        {
            if (CurrentPresent != null && CurrentPresent.GetStateEnum() == eState)
            {
                return true;
            }
            foreach (var state in listState)
            {
                if (state.state == eState)
                {
                    return true;
                }
            }
            return false;
        }
        public void PushNewState(UIStateEnum stateEnum, object valueIn = null, object valueIn1 = null, bool bNeedWait = true, object valueIn2 = null)
        {
            GuLog.Debug("<><DrinkWaterUIPresentManager> PushNewState:  " + stateEnum.ToString());

            PushNewState(new UIState
            {
                state = stateEnum,
                value0 = valueIn,
                value1 = valueIn1,
                value2 = valueIn2,
                needWait = bNeedWait
            });

        }

        private void RemoveSameState(UIState state)
        {
            for (int i = 0; i < listState.Count; ++i)
            {
                if (listState[i].state == state.state)
                {
                    listState.RemoveAt(i);
                    break;
                }
            }
        }

        public void PushNewState(UIState state)
        {
            if (state.state == UIStateEnum.eStartDailyProcessBar || state.state == UIStateEnum.eDrinkFail)
            {
                if (IsExistState(state.state))
                {
                    return;
                }
            }
            if (state.state == UIStateEnum.eRefreshMainByData)
            {
                RemoveSameState(state);
            }
            if (state.state == UIStateEnum.eMissionComplete)
            {
                FlurryUtil.LogEvent("Mission_Daily_count");
            }
            else if (state.state == UIStateEnum.eTreasureBoxOpen)
            {
                FlurryUtil.LogEvent("Nim_Chest_Open_daily_count");
            }
            else if (state.state == UIStateEnum.eDailyGoalCrownComplete)
            {
                switch ((int)state.value0)
                {
                    case 0:
                        {
                            FlurryUtil.LogEvent("Dailygoal_30_Archived_Event");
                        }
                        break;
                    case 1:
                        {
                            FlurryUtil.LogEvent("Dailygoal_60_Archived_Event");
                        }
                        break;
                    case 2:
                        {
                            FlurryUtil.LogEvent("Dailygoal_100_Archived_Event");
                        }
                        break;
                }
            }

            if (IsSilenceMode(state.state))
            {
                MainPresentBase present = GetPresentByEnum(state.state);
                if (present != null)
                    present.ProcessState(state);

                if (CurrentPresent == null)
                {
                    listState.Clear();
                    CurrentPresent = present;
                    return;
                }
                else
                {
                    if (!IsSilenceMode(CurrentPresent.GetStateEnum()))
                    {
                        ClearState();
                        CurrentPresent = present;
                        return;
                    }
                }
            }
            else if (CurrentPresent != null && IsSilenceMode(CurrentPresent.GetStateEnum()))
            {
                if (state.state != UIStateEnum.eRefreshMainByData && state.state != UIStateEnum.eStartDailyProcessBar)
                {
                    if (state.state == UIStateEnum.eDropWaterAni)
                    {
                        if (CurrentPresent.GetStateEnum() == UIStateEnum.eSchoolMode)
                        {
                            FlurryUtil.LogEvent("Drink_In_Schoolmode_Daily_Count");
                        }
                        else if (CurrentPresent.GetStateEnum() == UIStateEnum.eSleepMode)
                        {
                            FlurryUtil.LogEvent("Drink_In_Sleepmode_Daily_Count");
                        }
                    }
                    else if (state.state == UIStateEnum.eDailyProcessIncrease || state.state == UIStateEnum.eMissionFadeInOut)
                    {
                        MainPresentBase present = GetPresentByEnum(state.state);
                        if (present != null)
                            present.ProcessState(state);
                    }
                    return;
                }
            }
            if (state.state == UIStateEnum.eDropWaterAni)
            {
                FlurryUtil.LogEvent("Drink_Nomal_Daily_Count");
            }

            listState.Add(state);
        }
        public void ProcessStateRightNow()
        {
            GuLog.Debug("<><DrinkWaterUIPresentManager> ProcessStateRightNow");

            ProcessNextState();
        }
        private void ProcessNextState()
        {
            if (bHoldStatus)
            {
                return;
            }
            if (CurrentPresent == null || CurrentPresent.IsFinish())
            {
                UIState state = GetNextState();
                
                CurrentPresent = GetPresentByEnum(state.state);

                if (CurrentPresent != null)
                {
                    GuLog.Debug("<><DrinkWaterUIPresentManager> StateProcessing:" + state.state.ToString());

                    if (state.state != UIStateEnum.eSleepMode && state.state != UIStateEnum.eSchoolMode)
                    {
                        CurrentPresent.ProcessState(state);
                    }
                    if (!state.needWait)
                    {
                        CurrentPresent = null;
                    }
                    ProcessNextState();
                }

            }
        }

        public void ClearState()
        {
            if (CurrentPresent != null)
            {
                CurrentPresent.Stop();
                CurrentPresent = null;
                listState.Clear();
            }
        }

        // Update is called once per frame
        public void Update()
        {
            ProcessNextState();
        }

        public bool IsInRunning()
        {
            if (CurrentPresent != null)
            {
                GuLog.Debug("<><DrinkWaterUIPresentManager>currentPresent:" + CurrentPresent.GetStateEnum().ToString());
                return true;
            }
            if (bHoldStatus)
            {
                GuLog.Debug("<><DrinkWaterUIPresentManager>bHoldStatus:true");
                return true;
            }
            if (listState.Count != 0)
            {
                GuLog.Debug("<><DrinkWaterUIPresentManager>listState.Count" + listState.Count);
                return true;
            }
            return false;
        }
        public bool IsInSilenceMode()
        {
            if (CurrentPresent != null)
            {
                return IsSilenceMode(CurrentPresent.GetStateEnum());
            }
            return false;
        }

        bool bHoldStatus = false;
        public void SetHoldStatus(bool bHold)
        {
            bHoldStatus = bHold;
        }
    }


}
