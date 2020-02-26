using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using strange.extensions.mediation.impl;
namespace Hank
{
    public enum UIStateEnum
    {
        eIdle,
        eStartDailyProcessBar,
        eDropWaterAni,
        eRolePlayAction,
        eExpIncrease,
        ePlayerLevelUp,
        eTreasureBoxOpen,
        eShowPetPlayer,
        eDailyProcessIncrease,
        eDailyGoalCrownComplete,
        eDailyGoal100CompleteNotify,
        eMissionComplete,
        eMissionUpperLimitNotify,
        eNewMissionSwitch,
        eMissionRetry,
        eSchoolMode,
        eSleepMode,
        //eOtherMode,
        eRefreshMainByData,
        eMissionFadeInOut,
        eDrinkFail,
        eTotalUiState
    }

    public struct UIState
    {
        public UIStateEnum state;
        public object value0;
        public object value1;
        public object value2;
        public bool needWait;
    };

    public interface IDrinkWaterUIPresentManager
    {
        [SerializeField]
        MainPresentBase[] allIPresents { get; set; }

        MainPresentBase CurrentPresent { get; set; }

        List<UIState> listState { get; }

        void PushNewState(UIStateEnum stateEnum, object valueIn = null, object valueIn1 = null, bool bNeedWait = true, object valueIn2 = null);

        void ProcessStateRightNow();

        void SetHoldStatus(bool bHold);

        bool IsInRunning();
        bool IsInSilenceMode();

        bool IsExistState(UIStateEnum eState);

        void Update();

        void ClearState();
    }


}
