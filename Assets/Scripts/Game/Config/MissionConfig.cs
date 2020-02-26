using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gululu.Util;
using System.Security;
using Mono.Xml;
using System;
using Hank;

namespace Gululu.Config
{
    public class MissionConfig : AbsConfigBase, IMissionConfig
    {
        public List<WorldInfo> worldInfos = new List<WorldInfo>();
        private int starID = 10000;
        public bool ExistStar(int starID)
        {
            if (this.worldInfos != null && this.worldInfos.Count > 0 && this.worldInfos.Exists(t => t.ID == starID))
                return true;
            else
                return false;
        }
        public bool ExistMission(int missionID)
        {
            if (this.worldInfos == null || this.worldInfos.Count == 0)
                return true;

            for (int i = 0; i < this.worldInfos.Count; i++)
            {
                for (int j = 0; j < this.worldInfos[i].SceneInfos.Count; j++)
                {
                    if (this.worldInfos[i].SceneInfos[j].missionInfos.Exists(t => t.missionId == missionID))
                        return true;
                }
            }
            return false;
        }
        public int GetPreStarID(int starID, int defaultValue = PlayerData.defaultStar)
        {
            if (starID <= PlayerData.defaultStar || starID.ToString().Length != 5 || starID % 10000 != 0 || this.worldInfos == null || this.worldInfos.Count == 0)
                return defaultValue;

            bool existed = false;
            int nextStarID = defaultValue;
            for (int i = this.worldInfos.Count; i > 0; i--)
            {
                if (existed)
                {
                    nextStarID = this.worldInfos[i].ID;
                    break;
                }

                if (this.worldInfos[i].ID == starID)
                    existed = true;
            }
            return nextStarID;
        }
        public int GetNextStarID(int starID, int defaultValue = PlayerData.defaultStar)
        {
            if (starID < PlayerData.defaultStar || starID.ToString().Length != 5 || starID % 10000 != 0 || this.worldInfos == null || this.worldInfos.Count == 0)
                return defaultValue;

            bool existed = false;
            int nextStarID = defaultValue;
            for (int i = 0; i < this.worldInfos.Count; i++)
            {
                if (existed)
                {
                    nextStarID = this.worldInfos[i].ID;
                    break;
                }

                if (this.worldInfos[i].ID == starID)
                    existed = true;
            }
            return nextStarID;
        }
        public int GetStarIDByMissionID(int missionID)
        {
            if (missionID < PlayerData.startMission || missionID.ToString().Length != 5)
                return PlayerData.defaultStar;

            //Debug.LogFormat("<><MissionConfig.GetStarIDByMissionID>StarID: {0}, MissionID: {1}",
            //                int.Parse(missionID.ToString().Substring(0, 1)) * 10000, missionID);
            return int.Parse(missionID.ToString().Substring(0, 1)) * 10000;
        }
        public int GetSceneIndexByMissionID(int missionID)
        {
            WorldInfo worldInfo = this.GetWorldByStarID(this.GetStarIDByMissionID(missionID));
            if (worldInfo == null || worldInfo.SceneInfos == null || worldInfo.SceneInfos.Count == 0)
            {
                Debug.LogErrorFormat("<><MissionConfig.GetSceneIndexByMissionID>Can't find the star by missionID[{0}]", missionID);
                return -1;
            }

            bool found = false;
            int sceneIndex = -1;
            foreach (var sceneInfo in worldInfo.SceneInfos)
            {
                foreach (var missionInfo in sceneInfo.missionInfos)
                {
                    if (missionInfo.missionId == missionID)
                    {
                        sceneIndex = sceneInfo.index;
                        found = true;
                        break;
                    }
                }
                if (found) break;
            }
            Debug.LogFormat("<><MissionConfig.GetSceneIndexByMissionID>SceneIndex: {0}", sceneIndex);
            return sceneIndex;
        }
        public void SetStarID(int starID)
        {
            this.starID = starID;
        }
        public int GetMissionCount(int starID)
        {
            int missionCount = 0;
            WorldInfo worldInfo = this.worldInfos.Find(t => t.ID == starID);
            if (worldInfo == null) return missionCount;

            for (int i = 0; i < worldInfo.SceneInfos.Count; i++)
            {
                for (int j = 0; j < worldInfo.SceneInfos[i].missionInfos.Count; j++)
                {
                    if (worldInfo.SceneInfos[i].missionInfos[j].missionId == PlayerData.sleepMission)
                        continue;

                    missionCount += 1;
                }
            }
            return missionCount;
        }
        public WorldInfo GetWorldByStarID(int starID)
        {
            if (this.worldInfos != null)
                return this.worldInfos.Find(t => t.ID == starID);
            else
                return null;
        }
        public SceneInfo GetScene(int starID, int sceneIndex)
        {
            if (this.worldInfos != null)
            {
                WorldInfo worldInfo = this.GetWorldByStarID(starID);
                if (worldInfo != null && worldInfo.SceneInfos != null)
                    return worldInfo.SceneInfos.Find(t => t.index == sceneIndex);
                else
                    return null;
            }
            else
            {
                return null;
            }
        }
        public SceneInfo GetSceneByMissionID(int missionID)
        {
            if (this.worldInfos != null)
            {
                WorldInfo worldInfo = this.GetWorldByStarID(this.GetStarIDByMissionID(missionID));
                if (worldInfo != null && worldInfo.SceneInfos != null)
                    return worldInfo.SceneInfos.Find(t => t.index == this.GetSceneIndexByMissionID(missionID));
                else
                    return null;
            }
            else
            {
                return null;
            }
        }
        public List<WorldInfo> GetAllWorlds()
        {
            return this.worldInfos;
        }

        public void LoadMissionConfig()
        {
            if (m_LoadOk)
                return;
            worldInfos.Clear();
            ConfigLoader.Instance.LoadConfig("Config/MissionConfig.xml", Parser);
            //             StartCoroutine(_LoadMissionConfig());
        }
        private void Parser(WWW www)
        {
            m_LoadOk = true;
            GuLog.Debug("MissionConfigPath WWW::" + www.error);

            if (www.isDone && (www.error == null || www.error.Length == 0))
            {
                SecurityParser xmlDoc = new SecurityParser();
                GuLog.Debug("MissionConfigPath" + www.text);

                xmlDoc.LoadXml(www.text);
                ArrayList worldXmlList = xmlDoc.ToXml().Children;

                foreach (SecurityElement xeWorld in worldXmlList)
                {
                    if (xeWorld.Tag == "World")
                    {
                        WorldInfo worldInfo = new WorldInfo();
                        worldInfo.ID = Convert.ToInt32(xeWorld.Attribute("ID"));
                        worldInfo.Type = WorldInfo.WorldTypes.Normal;
                        int type = Convert.ToInt32(xeWorld.Attribute("Type"));
                        if (Enum.IsDefined(typeof(WorldInfo.WorldTypes), type))
                            worldInfo.Type = (WorldInfo.WorldTypes)type;
                        worldInfo.Name = xeWorld.Attribute("Name");
                        worldInfo.Title = xeWorld.Attribute("Title");
                        worldInfo.Photo = xeWorld.Attribute("Photo");
                        worldInfo.Preview = xeWorld.Attribute("Preview");
                        worldInfo.Unlock = xeWorld.Attribute("Unlock");
                        worldInfo.SleepBackgournd = xeWorld.Attribute("SleepBackgournd");

                        ArrayList sceneXmlList = xeWorld.Children;
                        foreach (SecurityElement xeScene in sceneXmlList)
                        {
                            if (xeScene.Tag == "Scene")
                            {
                                SceneInfo sceneInfo = new SceneInfo();
                                sceneInfo.index = Convert.ToInt32(xeScene.Attribute("Index"));
                                sceneInfo.strName = xeScene.Attribute("Name");
                                sceneInfo.TitleBg = xeScene.Attribute("TitleBg");
                                sceneInfo.TitleColor = xeScene.Attribute("TitleColor");
                                sceneInfo.Icon = xeScene.Attribute("Icon");
                                sceneInfo.BoxType = xeScene.Attribute("BoxType");
                                sceneInfo.Preview = xeScene.Attribute("Preview");
                                sceneInfo.Unlock = xeScene.Attribute("Unlock");

                                ArrayList storyXmlList = xeScene.Children;

                                foreach (SecurityElement xeMission in storyXmlList)
                                {
                                    if (xeMission.Tag == "Mission")
                                    {
                                        MissionInfo story = new MissionInfo();
                                        story.missionId = Convert.ToInt32(xeMission.Attribute("Id"));
                                        story.QuestId = Convert.ToInt32(xeMission.Attribute("QuestId"));
                                        story.strBackground = xeMission.Attribute("Background");
                                        story.strModel = xeMission.Attribute("Model");
                                        story.NimIcon = xeMission.Attribute("NimIcon");
                                        story.MissionName = xeMission.Attribute("Name");
                                        story.dailyGoalPercent = Convert.ToSingle(xeMission.Attribute("dailyGoalPercent"));
                                        story.AwardCoin = Convert.ToInt32(xeMission.Attribute("AwardCoin"));
                                        story.Sound = xeMission.Attribute("Sound");
                                        story.BGM = xeMission.Attribute("BGM");
                                        story.WaterDrop = xeMission.Attribute("WaterDrop");
                                        story.WaterDropAudio = xeMission.Attribute("WaterDropAudio");

                                        ArrayList treasureBoxXmlList = xeMission.Children;

                                        foreach (SecurityElement xeTreasure in treasureBoxXmlList)
                                        {
                                            if (xeTreasure.Tag == "TreasureBox")
                                            {
                                                MissionTreasureBox treasure = new MissionTreasureBox();
                                                treasure.BoxId = Convert.ToInt32(xeTreasure.Attribute("BoxId"));
                                                treasure.Height = Convert.ToInt32(xeTreasure.Attribute("Height"));
                                                story.listTreasureBox.Add(treasure);
                                            }
                                        }

                                        sceneInfo.missionInfos.Add(story);
                                    }
                                }
                                worldInfo.SceneInfos.Add(sceneInfo);
                            }
                        }
                        worldInfos.Add(worldInfo);
                    }
                }

            }

            GuLog.Debug("LoadMissionConfig OK! World:" + worldInfos.Count);
            GuLog.Debug("SceneCount" + worldInfos[0].SceneInfos.Count);
            GuLog.Debug("MissionCount" + worldInfos[0].SceneInfos[0].missionInfos.Count);
        }
        private IEnumerator LoadMissionConfigData()
        {
            string strImagePath = ResourceLoadBase.GetResourceBasePath() + "Config/MissionConfig.xml";
            GuLog.Debug("MissionConfigPath" + strImagePath);
            WWW www = new WWW(strImagePath);
            yield return www;

            Parser(www);
        }
        public int GetNextMissionId(int currentMission, bool withinStar = false)
        {
            int nextMissionId = this.FindSiblingMission(this.GetStarIDByMissionID(currentMission), currentMission, false, withinStar);
            return nextMissionId;
        }
        public int GetLastMissionId(int currentMission)
        {
            int preMissionId = this.FindSiblingMission(this.GetStarIDByMissionID(currentMission), currentMission, true);
            return preMissionId;
        }
        public int GetFirstMissionOfStar(int starID)
        {
            if (this.worldInfos != null && this.worldInfos.Exists(t => t.ID == starID))
            {
                WorldInfo worldInfo = this.worldInfos.Find(t => t.ID == starID);
                if (worldInfo != null && worldInfo.SceneInfos != null && worldInfo.SceneInfos.Count > 0 &&
                    worldInfo.SceneInfos[0].missionInfos != null && worldInfo.SceneInfos[0].missionInfos.Count > 0)
                    return worldInfo.SceneInfos[0].missionInfos[0].missionId;
                else return -1;
            }
            else
                return -1;
        }
        public int GetFirstMissionOfScene(int starID, int sceneIndex)
        {
            if (this.worldInfos == null) return -1;

            WorldInfo worldInfo = this.worldInfos.Find(t => t.ID == starID);
            if (worldInfo == null || worldInfo.SceneInfos == null) return -1;

            SceneInfo sceneInfo = worldInfo.SceneInfos.Find(t => t.index == sceneIndex);
            if (sceneInfo == null || sceneInfo.missionInfos == null || sceneInfo.missionInfos.Count == 0) return -1;

            return sceneInfo.missionInfos[0].missionId;
        }
        public MissionInfo GetMissionInfoByMissionId(int currentMission)
        {
            int starID = this.GetStarIDByMissionID(currentMission);
            if (!this.worldInfos.Exists(t => t.ID == starID))
            {
                Debug.LogErrorFormat("<><MissionConfig.GetMissionInfoByMissionId>Can't find star[{0}], mission[{1}]", starID, currentMission);
                return null;
            }

            WorldInfo worldInfo = this.worldInfos.Find(t => t.ID == starID);
            if (worldInfo == null) return null;
            foreach (var world in worldInfo.SceneInfos)
            {
                foreach (var mission in world.missionInfos)
                {
                    if (mission.missionId == currentMission)
                    {
                        return mission;
                    }
                }
            }
            return null;
        }
        public MissionInfo GetMissionInfoByTreasureBoxId(int boxId, int starID)
        {
            if (!this.worldInfos.Exists(t => t.ID == starID))
            {
                Debug.LogErrorFormat("<><MissionConfig.GetCurrentTitleBg>Can't find star[{0}], boxId[{1}]", starID, boxId);
                return null;
            }

            WorldInfo worldInfo = this.worldInfos.Find(t => t.ID == starID);
            if (worldInfo == null) return null;
            foreach (var world in worldInfo.SceneInfos)
            {
                foreach (var mission in world.missionInfos)
                {
                    if (mission.listTreasureBox.Exists(t => t.BoxId == boxId))
                    {
                        return mission;
                    }
                }
            }
            return null;
        }
        public List<MissionInfo> GetAllMissionOfScene(int starID, int sceneIndex)
        {
            WorldInfo worldInfo = this.GetWorldByStarID(starID);
            if (worldInfo == null || worldInfo.SceneInfos == null || worldInfo.SceneInfos.Count == 0)
                return null;

            SceneInfo sceneInfo = worldInfo.SceneInfos.Find(t => t.index == sceneIndex);
            if (sceneInfo == null)
                return null;
            else
                return sceneInfo.missionInfos;
        }
        public float GetDailyGoalPercent(int currentMission)
        {
            MissionInfo levelInfo = GetMissionInfoByMissionId(currentMission);
            if (levelInfo != null)
            {
                return levelInfo.dailyGoalPercent;
            }
            return 0.34f;
        }
        public string GetCurrentTitleBg(int currentMission)
        {
            if (!this.worldInfos.Exists(t => t.ID == this.starID))
            {
                Debug.LogErrorFormat("<><MissionConfig.GetCurrentTitleBg>Can't find star[{0}], mission[{1}]", this.starID, currentMission);
                return string.Empty;
            }

            WorldInfo worldInfo = this.worldInfos.Find(t => t.ID == this.starID);
            if (worldInfo == null) return string.Empty;
            foreach (var world in worldInfo.SceneInfos)
            {
                foreach (var mission in world.missionInfos)
                {
                    if (mission.missionId == currentMission)
                    {
                        return world.TitleBg;
                    }
                }
            }
            return string.Empty;
        }
        public Color GetCurrentTitleTextColor(int currentMission)
        {
            if (!this.worldInfos.Exists(t => t.ID == this.starID))
            {
                Debug.LogErrorFormat("<><MissionConfig.GetCurrentTitleTextColor>Can't find star[{0}], mission[{1}]", this.starID, currentMission);
                return Color.white;
            }

            WorldInfo worldInfo = this.worldInfos.Find(t => t.ID == this.starID);
            if (worldInfo == null) return Color.white;
            foreach (var world in worldInfo.SceneInfos)
            {
                foreach (var mission in world.missionInfos)
                {
                    if (mission.missionId == currentMission)
                    {
                        return world.TextColor;
                    }
                }
            }
            return Color.white;
        }
        public int GetAwardCoin(int currentMission)
        {
            MissionInfo levelInfo = GetMissionInfoByMissionId(currentMission);
            if (levelInfo != null)
            {
                return levelInfo.AwardCoin;
            }
            return 0;
        }
        private int FindSiblingMission(int starID, int missionId, bool preMission, bool withinStar = false)
        {
            WorldInfo star = worldInfos.Find(t => t.ID == starID);
            if (star == null) return -1;

            bool bFind = false;
            bool bSet = false;
            int preMissionId = -1;
            int result = -1;
            int sceneIndex = this.GetSceneIndexByMissionID(missionId);
            foreach (var scene in star.SceneInfos)
            {
                if ((star.Type == WorldInfo.WorldTypes.Normal && !withinStar || star.Type == WorldInfo.WorldTypes.Duplicate) &&
                    scene.index != sceneIndex) continue;//主线星球可以选择是搜索整个星球的关卡还是搜索本地图的关卡，副本星球只能搜索本地图的关卡

                foreach (var mission in scene.missionInfos)
                {
                    if (mission.missionId == PlayerData.sleepMission)
                        continue;//睡眠模式的Mission不参与计算

                    if (preMissionId == -1)
                        preMissionId = mission.missionId;//第一个地图的第一个关卡一定要记录

                    if (bFind && !bSet && !preMission)
                    {//找到下一个关卡的ID
                        result = mission.missionId;
                        bSet = true;
                        break;
                    }
                    if (mission.missionId == missionId)
                    {
                        bFind = true;
                    }
                    if (bFind && !bSet && preMission)
                    {//找到上一个关卡的ID
                        result = preMissionId;
                        bSet = true;
                        break;
                    }
                    preMissionId = mission.missionId;
                }
                if (bFind && bSet) break;
            }
            return result;
        }
    }
}
