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
    public static class Stars
    {
        public const int IMA = 10000;
        public const int LOGIC_LAND = 80000;
        public const int EARTH = 90000;
    }

    public class MissionTreasureBox
    {
        public int BoxId;
        public int Height;
    }
    public class MissionInfo
    {
        public string strBackground;
        public string strModel;
        public string NimIcon;
        public string MissionName;
        public int missionId;
        public int QuestId;
        public float dailyGoalPercent;
        public int AwardCoin;
        public string Sound;
        public string BGM;
        public string WaterDrop;
        public string WaterDropAudio;
        public List<MissionTreasureBox> listTreasureBox = new List<MissionTreasureBox>();
    }

    public class SceneInfo
    {
        public string strName;
        public string TitleBg;
        public string TitleColor;
        public string Icon;
        public string BoxType;//主页面右侧宝箱类型
        public string Preview;
        public Color TextColor
        {
            get
            {//任何异常都返回白色，没有异常按照配置返回颜色
                Color color = Color.white;
                try
                {
                    if (!string.IsNullOrEmpty(this.TitleColor))
                    {
                        string[] parts = this.TitleColor.Split(',');
                        if (parts != null && parts.Length == 3)
                        {
                            return new Color(float.Parse(parts[0]), float.Parse(parts[1]), float.Parse(parts[2]));
                        }
                    }
                    return color;
                }
                catch (Exception ex)
                {
                    GuLog.Error(string.Format("----SceneInfo.TextColor----Color config error: {0}", ex.Message));
                    return color;
                }
            }
        }
        public string Unlock;//地图解锁条件
        public bool Locked
        {
            get
            {
                if (string.IsNullOrEmpty(this.Unlock) || this.Unlock.ToUpper() == "NONE")
                    return false;

                bool locked = true;
                string[] parts = this.Unlock.Split('|');
                if (parts != null)
                {
                    switch (parts.Length)
                    {
                        case 2:
                            if (parts[0].ToUpper() == "DATE")
                            {
                                DateTime date = DateTime.MinValue;
                                if (DateTime.TryParse(parts[1], out date))
                                    locked = DateTime.Now < date;
                            }
                            break;
                    }
                    return locked;
                }
                else return locked;
            }
        }//地图是否解锁
        public DateTime OpenDate//地图开放日期
        {
            get
            {
                if (string.IsNullOrEmpty(this.Unlock) || this.Unlock.ToUpper() == "NONE")
                    return DateTime.MinValue;

                string[] parts = this.Unlock.Split('|');
                if (parts != null)
                {
                    switch (parts.Length)
                    {
                        case 2:
                            if (parts[0].ToUpper() == "DATE")
                            {
                                DateTime date = DateTime.MinValue;
                                if (DateTime.TryParse(parts[1], out date))
                                    return date;
                            }
                            break;
                    }
                    return DateTime.MinValue;
                }
                else return DateTime.MinValue;
            }
        }
        public int index;
        public List<MissionInfo> missionInfos = new List<MissionInfo>();
    }

    public class WorldInfo
    {
        public enum WorldTypes { Normal = 1, Duplicate = 9 }
        public string Name { get; set; }//星球名字
        public int ID { get; set; }//星球ID
        public WorldTypes Type { get; set; }//星球类型(此属性的作用之一是决定喝水开宝箱显示那种结算UI)
        public List<SceneInfo> SceneInfos { get; set; }//地图信息
        public string Title;//星球名字图片
        public string Photo;//星球照片
        public string Preview;//星球预览图
        public string Unlock;//星球解锁条件
        public bool Locked
        {
            get
            {
                if (string.IsNullOrEmpty(this.Unlock) || this.Unlock.ToUpper() == "NONE")
                    return false;

                bool locked = true;
                string[] parts = this.Unlock.Split('|');
                if (parts != null)
                {
                    switch (parts.Length)
                    {
                        case 2:
                            if (parts[0].ToUpper() == "DATE")
                            {
                                DateTime date = DateTime.MinValue;
                                if (DateTime.TryParse(parts[1], out date))
                                    locked = DateTime.Now < date;
                            }
                            break;
                    }
                    return locked;
                }
                else return locked;
            }
        }//星球是否解锁
        public DateTime OpenDate//星球开放日期
        {
            get
            {
                if (string.IsNullOrEmpty(this.Unlock) || this.Unlock.ToUpper() == "NONE")
                    return DateTime.MinValue;

                string[] parts = this.Unlock.Split('|');
                if (parts != null)
                {
                    switch (parts.Length)
                    {
                        case 2:
                            if (parts[0].ToUpper() == "DATE")
                            {
                                DateTime date = DateTime.MinValue;
                                if (DateTime.TryParse(parts[1], out date))
                                    return date;
                            }
                            break;
                    }
                    return DateTime.MinValue;
                }
                else return DateTime.MinValue;
            }
        }
        public string SleepBackgournd;//睡眠模式时的背景图
        public WorldInfo()
        {
            this.SceneInfos = new List<SceneInfo>();
        }
    }

    public class StarInfo
    {
        public int CompleteMissionCount { get; set; }
        public int TotalMissionCount { get; set; }
    }

    public interface IMissionConfig
    {
        List<WorldInfo> GetAllWorlds();
        WorldInfo GetWorldByStarID(int starID);
        SceneInfo GetScene(int starID, int sceneIndex);
        SceneInfo GetSceneByMissionID(int missionID);
        bool ExistStar(int starID);
        bool ExistMission(int missionID);
        int GetPreStarID(int starID, int defaultValue = PlayerData.defaultStar);
        int GetNextStarID(int starID, int defaultValue = PlayerData.defaultStar);
        int GetStarIDByMissionID(int missionID);
        int GetSceneIndexByMissionID(int missionID);
        void SetStarID(int index);
        int GetMissionCount(int starID);
        void LoadMissionConfig();
        int GetNextMissionId(int currentMission, bool withinStar = false);
        int GetLastMissionId(int currentMission);
        int GetFirstMissionOfStar(int starID);
        int GetFirstMissionOfScene(int starID, int sceneIndex);
        float GetDailyGoalPercent(int currentMission);

        MissionInfo GetMissionInfoByMissionId(int currentMission);
        MissionInfo GetMissionInfoByTreasureBoxId(int boxId, int starID);
        List<MissionInfo> GetAllMissionOfScene(int starID, int sceneIndex);
        string GetCurrentTitleBg(int currentMission);
        Color GetCurrentTitleTextColor(int currentMission);
        bool IsLoadedOK();
        int GetAwardCoin(int currentMission);
    }
}
