using BestHTTP;
using Cup.Utils.android;
using Gululu;
using Gululu.Config;
using Gululu.LocalData.Agents;
using Gululu.net;
using Gululu.Util;
using Hank.Api;
using Hank.MainScene;
using LitJson;
using strange.extensions.mediation.impl;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Hank
{
    public class MissionDataManager : IMissionDataManager
    {
        [Inject]
        public IJsonUtils mJsonUtils { set; get; }
        [Inject]
        public IGameDataHelper gameDataHelper { get; set; }
        [Inject]
        public ILocalChildInfoAgent mLocalChildInfoAgent { get; set; }
        [Inject]
        public IMissionConfig MissionConfig { get; set; }
        [Inject]
        public StartPostGameDataCommandSignal startPostGameDataCommand { get; set; }

        public const string missionKey = "MissionData";


        Dictionary<int, MissionState> missionInfo = new Dictionary<int, MissionState>();

        struct MisssionInfo
        {
            public int missionId;
            public MissionState eState;
        }

        [PostConstruct]
        public void PostConstruct()
        {
#if UNITY_EDITOR
            mLocalChildInfoAgent.saveChildSN(ServiceConfig.defaultChildSn);
#endif

            LoadMissionData();
        }

        public bool StarEntered(int starID)
        {
            if (this.missionInfo == null) return false;

            foreach (KeyValuePair<int, MissionState> kvp in this.missionInfo)
            {
                int starId = this.MissionConfig.GetStarIDByMissionID(kvp.Key);
                if (starId == starID) return true;
            }
            return false;
        }

        public MissionState GetMisionState(int missionId)
        {
            MissionState missionState;
            if (missionInfo.TryGetValue(missionId, out missionState))
            {
                return missionState;
            }
            return MissionState.eLock;

        }
        public bool IsMissionComplete(int missionId)
        {
            return GetMisionState(missionId) == MissionState.eComplete;
        }

        public void AddMissionState(int missionId, string strState)
        {
            MissionState state = (MissionState)Enum.Parse(typeof(MissionState), strState);
            AddMissionState(missionId, state);
        }
        public void SetMissionStateNot2Server(int missionId, string strState)
        {
            MissionState state = (MissionState)Enum.Parse(typeof(MissionState), strState);
            SetMissionStateNot2Server(missionId, state);
        }

        void SetMissionStateNot2Server(int missionId, MissionState state)
        {
            MissionState missionState;
            if (missionInfo.TryGetValue(missionId, out missionState))
            {
                missionInfo.Remove(missionId);
            }
            missionInfo.Add(missionId, state);
        }

        public void AddMissionState(int missionId, MissionState state = MissionState.eUnderway)
        {
            SetMissionStateNot2Server(missionId, state);
            SaveMissionData();
            SyncMissionData2Server(missionId, state);
        }

        public void RemoveMissionState(int missionId)
        {
            if (missionInfo.ContainsKey(missionId))
                missionInfo.Remove(missionId);
        }

        public void ClearAllMissionOfScene(int starID, int sceneIndex)
        {
            WorldInfo worldInfo = this.MissionConfig.GetWorldByStarID(starID);
            if (worldInfo != null && worldInfo.Type == WorldInfo.WorldTypes.Duplicate)
            {
                List<MissionsBean> allMissionOfStar = new List<MissionsBean>();
                foreach (var sceneInfo in worldInfo.SceneInfos)
                {
                    if (sceneInfo.index != sceneIndex) continue;
                    foreach (var missionInfo in sceneInfo.missionInfos)
                    {
                        this.RemoveMissionState(missionInfo.missionId);
                        allMissionOfStar.Add(new MissionsBean
                        {
                            mission_id = missionInfo.missionId,
                            mission_state = MissionState.eLock.ToString()
                        });
                    }
                }
                SendMissionData(allMissionOfStar);
            }
            else
            {
                Debug.LogErrorFormat("<><MissionDataManager.ClearAllMissionOfStar>worldInfo is null, StarID: {0}", starID);
            }
        }

        private void SendMissionData(List<MissionsBean> dataMissions)
        {
            GameData datas = GameData.getGameDataBuilder()
                .setMissions(dataMissions)
                .setTimestamp((int)DateUtil.GetTimeStamp())
                .build();
            string jsonData = mJsonUtils.Json2String(datas);
            startPostGameDataCommand.Dispatch(jsonData);

        }

        void SyncMissionData2Server(int missionId, MissionState state)
        {

            List<MissionsBean> missions = new List<MissionsBean>();
            missions.Add(new MissionsBean
            {
                mission_id = missionId,
                mission_state = state.ToString()
            });

            SendMissionData(missions);
        }

        public void SendAllMissionData(bool force)
        {
            if (force)
            {
                List<MissionsBean> missions = new List<MissionsBean>();

                foreach (var pair in missionInfo)
                {
                    missions.Add(new MissionsBean
                    {
                        mission_id = pair.Key,
                        mission_state = pair.Value.ToString()
                    });
                }
                SendMissionData(missions);
            }
        }
        public void SetValuesLong(GameData datas)
        {
            List<MissionsBean> listInfos = datas.getMissions();
            if (listInfos != null)
            {
                for (int i = 0; i < listInfos.Count; ++i)
                {
                    AddMissionState(listInfos[i].getMission_id(), listInfos[i].getMission_state());
                }
            }
        }

        private List<MisssionInfo> Convert2List()
        {
            List<MisssionInfo> listInfos = new List<MisssionInfo>();
            foreach (var pair in missionInfo)
            {
                listInfos.Add(new MisssionInfo
                {
                    missionId = pair.Key,
                    eState = pair.Value
                });
            }
            return listInfos;
        }

        public void SaveMissionData()
        {
            List<MisssionInfo> listInfos = Convert2List();

            gameDataHelper.SaveObject<List<MisssionInfo>>(missionKey, listInfos);

        }
        public void LoadMissionData()
        {
            missionInfo.Clear();
            List<MisssionInfo> listInfos = gameDataHelper.GetObject<List<MisssionInfo>>(missionKey);
            if (listInfos != null)
            {
                foreach (var info in listInfos)
                {
                    missionInfo.Add(info.missionId, info.eState);
                    GuLog.Debug("<><MissionData>missionId:" + info.missionId + "    State:" + info.eState.ToString());
                }
            }
            MissionState missionState;
            if (!missionInfo.TryGetValue(PlayerData.startMission, out missionState))
            {
                missionInfo.Add(PlayerData.startMission, MissionState.eUnderway);
            }
        }

        static public float[] treasurePersent =
        {
            0.3f,0.8f
        };

        public int GetTreasureBoxCount()
        {
            return treasurePersent.Length;
        }

        public bool CanTreasureOpen(double missionPercent, int index)
        {
            if (index >= 0 && index < GetTreasureBoxCount())
            {
                return missionPercent > treasurePersent[index];
            }
            return false;
        }

        public bool IsTreasureOpen(int missionId, int index)
        {
            MissionState missionState;
            if (missionInfo.TryGetValue(missionId, out missionState))
            {
                if (index == 0)
                {
                    return missionState >= MissionState.eTreasureBoxOpen1;
                }
                else if (index == 1)
                {
                    return missionState >= MissionState.eTreasureBoxOpen2;
                }
            }
            return false;
        }

        public void SetTreasureOpen(int missionId, int index)
        {
            if (index == 0)
            {
                AddMissionState(missionId, MissionState.eTreasureBoxOpen1);
            }
            else if (index == 1)
            {
                AddMissionState(missionId, MissionState.eTreasureBoxOpen2);
            }

        }

        public Dictionary<int, MissionState> GetAllMissionState()
        {
            return this.missionInfo;
        }

        //计算每个星球的关卡进度
        public StarInfo GetStarInfo(int starID, int completeMissionID)
        {
            StarInfo starInfo = new StarInfo() { CompleteMissionCount = 0, TotalMissionCount = 0 };
            List<WorldInfo> worldInfos = this.MissionConfig.GetAllWorlds();
            if (worldInfos == null || worldInfos.Count == 0 ||
                !worldInfos.Exists(t => t.ID == starID) || this.MissionConfig.GetStarIDByMissionID(completeMissionID) != starID)
            {
                Debug.LogWarningFormat("<><MissionDataManager.GetStarInfo>worldInfos is null or not exists star: {0}", starID);
                starInfo.TotalMissionCount = this.MissionConfig.GetMissionCount(starID);
                return starInfo;
            }

            Debug.LogFormat("<><MissionDataManager.GetStarInfo>StarID: {0}, CompleteMissionID: {1}", starID, completeMissionID);
            int completeCount = 0, totalCount = 0;
            WorldInfo worldInfo = worldInfos.Find(t => t.ID == starID);
            if (worldInfos == null) return starInfo;

            foreach (var sceneInfo in worldInfo.SceneInfos)
            {
                foreach (var missionInfo in sceneInfo.missionInfos)
                {
                    if (missionInfo.missionId == PlayerData.sleepMission)
                        continue;

                    totalCount += 1;
                    if (this.GetMisionState(missionInfo.missionId) != MissionState.eLock)
                        completeCount += 1;
                }
            }
            starInfo.CompleteMissionCount = completeCount;
            starInfo.TotalMissionCount = totalCount;
            Debug.LogFormat("<><MissionDataManager.GetStarInfo>MissionCompleteCount: {0}, MissionTotalCount: {1}", completeCount, totalCount);
            return starInfo;
        }
    }
}

