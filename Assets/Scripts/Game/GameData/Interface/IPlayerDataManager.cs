using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gululu.Util;
using Gululu;
using Gululu.Config;
using System;
using Gululu.Log;
using Cup.Utils.android;
using Gululu.LocalData.Agents;
using Gululu.net;
using BestHTTP;
using strange.extensions.mediation.impl;

namespace Hank
{

    public class PlayerTodayData
    {
        public int todayDate;//date
        public int todayIntake;
        public int todayCrownGotCount;
        public int todayDrinkTimes;
    }

    public class PlayerData
    {
        public const int sleepMission = -10000;
        public const int startMission = 11001;
        public const int defaultStar = 10000;
        public int starID = 10000;
        public int sceneIndex = 1;
        public int playerLevel = 1;
        public int playerExp;
        public Dictionary<int, int> playerMissionId = null;
        public Dictionary<string, float> playerMissionPercent;
        public int playerCrownCount = 0;
        public PlayerData()
        {
            this.playerMissionId = new Dictionary<int, int>();
            this.playerMissionId.Add(defaultStar, startMission);
            this.playerMissionPercent = new Dictionary<string, float>();
        }
    }

    public class AnimalData
    {
        public int ItemId { get; set; }
        public int PetType { get; set; }
        public string ImageUrl { get; set; }
        public Sprite Image { get; set; }
        public string AudioUrl { get; set; }
        public AudioClip Audio { get; set; }
        public string AnimationUrl { get; set; }
        public GameObject Animation { get; set; }
        public bool Lock { get; set; }
    }

    public interface IPlayerDataManager
    {
        PlayerTodayData GetPlayerTodayData();
        PlayerData GetPlayerData();
        bool IsMissionLimit();
        string Language { get; set; }

        bool DidCrownGet(int index);

        string CurrentPet { get; set; }
        DateTime Date { get; set; }
        int date { set; get; }

        int CrownCount { set; get; }

        float percentOfDailyGoal { set; get; }

        int currIntake { set; get; }

        int todayCrownGotCount { set; get; }

        int TodayDrinkTimes { get; set; }

        float missionPercent { set; get; }

        int playerLevel { set; get; }
        int exp { set; get; }

        int expForLevelUp { get; }
        //record voice percent before silence mode. used to recover voice percent after silence mode
        int voicePercentBeforeSilenceMode { set; get; }

        int StarID { get; set; }
        int SceneIndex { get; set; }

        int missionId { set; get; }

        int MissionIdOfOpenedBox { get; set; }

        int PetID { get; set; }

        string PurpieAccessories { get; }
        string DonnyAccessories { get; }
        string NinjiAccessories { get; }
        string SansaAccessories { get; }
        string YoyoAccessories { get; }
        string NuoAccessories { get; }

        bool Init();

        int GetMissionID(int starID, int defaultMissionID = -1);
        void SetMissionID(int starID, int missionID);

        float GetMissionPercent(int starID, int sceneIndex);
        void ClearMissionPercent();

        float GetExpPercent();

        int GetDailyGoal();
        void SetDailyGoal(int value);

        void TrySendPlayerData2Server();
        void SetValuesLong(Hank.Api.GameData datas);


        void NewDay(int currDay);
        void LoadPlayerDatas();

        int GetDailyGoalExp(int type);
        int GetDailyGoalExp(float percent);

        bool IsNovice();

        void RecoverVolume();
        void SetVolumeZero();

        bool LanguageInitialized();
        string GetLanguage();
        void SaveLanguage(string languageShortName);
        void ClearLanguage();
        float CurEnergy { get; set; }
        bool IsPetHunger { get; }
    }
}
