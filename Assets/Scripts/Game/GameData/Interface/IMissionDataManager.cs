using BestHTTP;
using Gululu;
using Gululu.Config;
using Gululu.LocalData.Agents;
using Gululu.net;
using Gululu.Util;
using LitJson;
using strange.extensions.mediation.impl;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Hank
{
    public enum MissionState
    {
        eLock = 0,
        eUnderway,
        eTreasureBoxOpen1,
        eTreasureBoxOpen2,
        eComplete
    }

    public interface IMissionDataManager
    {


        MissionState GetMisionState(int missionId);
        bool IsMissionComplete(int missionId);

        void AddMissionState(int missionId, string strState);

        void AddMissionState(int missionId, MissionState state = MissionState.eUnderway);

        void RemoveMissionState(int missionId);

        void ClearAllMissionOfScene(int starID, int sceneIndex);

        void SetMissionStateNot2Server(int missionId, string strState);

        bool StarEntered(int starID);

        void SendAllMissionData(bool force);
        void SetValuesLong(Hank.Api.GameData datas);


        void SaveMissionData();
        void LoadMissionData();

        int GetTreasureBoxCount();

        bool CanTreasureOpen(double missionPercent, int index);

        bool IsTreasureOpen(int missionId, int index);

        void SetTreasureOpen(int missionId, int index);

        Dictionary<int, MissionState> GetAllMissionState();

        StarInfo GetStarInfo(int starID, int completeMissionID);
    }
}

