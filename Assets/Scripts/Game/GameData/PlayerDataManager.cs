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
using Hank.Api;
using Hank.MainScene;
using Cup.Utils.audio;

namespace Hank
{
    public class PlayerDataManager : IPlayerDataManager
    {
        [Inject]
        public IJsonUtils mJsonUtils { set; get; }
        [Inject]
        public IGameDataHelper gameDataHelper { get; set; }
        [Inject]
        public IMissionDataManager missionDataManager { get; set; }
        [Inject]
        public ILocalChildInfoAgent mLocalChildInfoAgent { get; set; }
        [Inject]
        public StartPostGameDataCommandSignal startPostGameDataCommand { get; set; }
        [Inject]
        public IMissionConfig missionConfig { get; set; }
        [Inject]
        public ILevelConfig levelConifg { get; set; }
        [Inject]
        public ILanConfig LanConfig { get; set; }
        [Inject]
        public ILocalPetInfoAgent LocalPetInfoAgent { get; set; }
        [Inject]
        public ILocalPetAccessoryAgent LocalPetAccessoryAgent { get; set; }
        public string Language { get; set; }
        public static bool ResourceDownloaded { get; set; }
        PlayerTodayData playerTodayData = new PlayerTodayData();
        bool bDirty = false;
        int dailyGoal = 2000;
        PlayerData playerData = new PlayerData();

        public bool Init()
        {
            LoadPlayerDatas();
            this.GetLanguage();
            return true;
        }

        public PlayerTodayData GetPlayerTodayData()
        {
            return playerTodayData;
        }

        public PlayerData GetPlayerData()
        {
            return playerData;
        }

        public bool IsMissionLimit()
        {
            return /*missionPercent >= 1 && */currIntake >= GetDailyGoal();
        }

        public bool DidCrownGet(int index)
        {
            return todayCrownGotCount > index;
        }

        public DateTime Date
        {
            get
            {
                string dateString = getObject<string>("TodayDate");
                DateTime result = DateTime.MinValue;
                if (DateTime.TryParse(dateString, out result))
                {
                    return result;
                }
                else
                {
                    saveObject<string>("TodayDate", DateTime.Today.ToString());
                    return DateTime.Today;
                }
            }

            set
            {
                saveObject<string>("TodayDate", value.ToString());
            }
        }

        public int date
        {
            get { return playerTodayData.todayDate; }
            set
            {
                GuLog.Debug("<><PlayerData><>playerData_date:" + date.ToString());
                playerTodayData.todayDate = value;
                saveObject<int>("Date", playerTodayData.todayDate);
            }
        }

        public int CrownCount
        {
            get { return playerData.playerCrownCount; }
            set
            {
                GuLog.Debug("<><PlayerData>playerData_CrownCount:" + value.ToString());
                playerData.playerCrownCount = value;
                saveObject<int>("CrownCount", playerData.playerCrownCount);
            }

        }

        public float percentOfDailyGoal
        {
            get
            {
                return currIntake / (float)dailyGoal;
            }
            set
            {
                GuLog.Debug("<><PlayerData>playerData_percentOfDailyGoal:" + percentOfDailyGoal.ToString());
            }
        }

        public int currIntake
        {
            get { return playerTodayData.todayIntake; }
            set
            {
                GuLog.Debug("<><PlayerData>playerData_currIntake:" + currIntake.ToString());

                playerTodayData.todayIntake = value;
                percentOfDailyGoal = currIntake / (float)dailyGoal;
                SaveIntake();
            }
        }

        public int todayCrownGotCount
        {
            get { return playerTodayData.todayCrownGotCount; }
            set
            {
                playerTodayData.todayCrownGotCount = value;
                GuLog.Debug("<><PlayerData>playerData_crownGotCount:" + todayCrownGotCount.ToString());
                SaveCrownGotCount();
            }
        }

        public int TodayDrinkTimes
        {
            get
            {
                return this.playerTodayData != null ? this.playerTodayData.todayDrinkTimes : 0;
            }
            set
            {
                if (this.playerTodayData != null)
                {
                    this.playerTodayData.todayDrinkTimes = value;
                    saveObject<int>("DrinkTimes", playerTodayData.todayDrinkTimes);
                }
            }
        }

        public int PetID { get; set; }

        public float missionPercent
        {
            get
            {
                return this.GetMissionPercent(this.StarID, this.SceneIndex);
            }
            set
            {
                WorldInfo worldInfo = this.missionConfig.GetWorldByStarID(this.StarID);
                if (worldInfo == null || this.playerData == null || this.playerData.playerMissionPercent == null)
                    return;

                value = Mathf.Clamp01(value);
                string key = worldInfo.Type == WorldInfo.WorldTypes.Normal ? this.StarID.ToString() : string.Format("{0}_{1}", this.StarID, this.SceneIndex);

                if (!this.playerData.playerMissionPercent.ContainsKey(key))
                    this.playerData.playerMissionPercent.Add(key, value);
                else
                    this.playerData.playerMissionPercent[key] = value;
                GuLog.Debug("<><PlayerData>playerData_missionPercent:" + value.ToString());
                SaveMissionPercent();
            }
        }

        public int playerLevel
        {
            get { return playerData.playerLevel; }
            set
            {
                playerData.playerLevel = value;
                GuLog.Debug("<><PlayerData>playerData_level:" + value.ToString());
                SavePlayerLevel();
            }
        }
        public int exp
        {
            get { return playerData.playerExp; }
            set
            {
                playerData.playerExp = value;
                GuLog.Debug("<><PlayerData>playerData_exp:" + value.ToString());
                SavePlayerExp();
            }
        }

        public string CurrentPet
        {
            get { return this.LocalPetInfoAgent != null ? this.LocalPetInfoAgent.getCurrentPet() : ""; }

            set { if (this.LocalPetInfoAgent != null) this.LocalPetInfoAgent.saveCurrentPet(value); }
        }

        public int expForLevelUp
        {
            get { return levelConifg.GetExpForLevelUp(playerLevel); }
        }

        int _voicePercent = 100;
        public int voicePercentBeforeSilenceMode
        {
            get { return _voicePercent; }
            set
            {
                GuLog.Debug("<><PlayerData>Set voicePercent:" + value.ToString());
                _voicePercent = value;
                SaveVoicePercent();
            }

        }

        public int StarID
        {
            get
            {
                return this.missionConfig.ExistStar(this.playerData.starID) ? this.playerData.starID : PlayerData.defaultStar;
            }
            set
            {
                this.playerData.starID = value;
                this.missionConfig.SetStarID(value);
                GuLog.Debug("<><PlayerData>playerData_starID:" + value.ToString());
                SaveStarID();
            }
        }

        public int SceneIndex
        {
            get
            {
                WorldInfo worldInfo = this.missionConfig.GetWorldByStarID(this.StarID);
                if (worldInfo != null && worldInfo.SceneInfos != null && worldInfo.SceneInfos.Count > 0)
                    return Mathf.Clamp(this.playerData.sceneIndex, 1, worldInfo.SceneInfos.Count);
                else
                    return 1;
            }

            set
            {
                this.playerData.sceneIndex = value;
                //this.missionConifg.SetStarID(value);
                GuLog.Debug("<><PlayerData>playerData_sceneIndex:" + value.ToString());
                SaveSceneIndex();
            }
        }

        public int missionId
        {
            get
            {
                return this.GetMissionID(this.playerData.starID, PlayerData.startMission);
            }
            set
            {
                if (this.playerData == null || this.playerData.playerMissionId == null || !this.missionConfig.ExistMission(value))
                    return;

                /* 这里必须先根据MissionID求出StarID，然后用求出来的StarID作为Key值，往Dictionary中写数据
                 * 不能用this.playerData.starID作为Key的原因是：当需要写入missionId时，此时的this.playerData.starID未必是正确的
                 */
                int starID = this.missionConfig.GetStarIDByMissionID(value);
                this.SetMissionID(starID, value);
                Debug.LogFormat("<><PlayerDataManager.missionId>Set, starID: {0}, missionID: {1}", starID, value);
            }
        }

        public int MissionIdOfOpenedBox { get; set; }


        private const float DEFAULT_ENERGY_VALUE_PERCENT = 0.8f;
        public float CurEnergy
        {
            get
            {
                return gameDataHelper.GetObject<float>("LAST_PET_ENERGY_VALUE", levelConifg.GetHunger(playerLevel) * DEFAULT_ENERGY_VALUE_PERCENT);
            }
            set
            {
                if (value < 0) value = 0;
                gameDataHelper.SaveObject<float>("LAST_PET_ENERGY_VALUE", value);
            }
        }

        public bool IsPetHunger
        {
            get
            {
                if (levelConifg.GetHunger(playerLevel) == 0)
                {
                    return true;
                }
                //Debug.LogFormat("<><PlayerDataManager>:::::{0},{1}",CurEnergy,CurEnergy / levelConifg.GetHunger(playerLevel));
                //return CurEnergy / levelConifg.GetHunger(playerLevel) <= PetEnergyManager.HUNGER_THRESHOLD;
                return false;
            }
        }

        public string PurpieAccessories
        {
            get
            {
                if (this.LocalPetAccessoryAgent != null)
                {
                    List<string> accessories = this.LocalPetAccessoryAgent.GetPetAccessories(Roles.Purpie);
                    if (accessories != null && accessories.Count > 0)
                    {
                        return string.Join(",", accessories.ToArray());
                    }
                    else return "";
                }
                else return "";
            }
        }
        public string DonnyAccessories
        {
            get
            {
                if (this.LocalPetAccessoryAgent != null)
                {
                    List<string> accessories = this.LocalPetAccessoryAgent.GetPetAccessories(Roles.Donny);
                    if (accessories != null && accessories.Count > 0)
                    {
                        return string.Join(",", accessories.ToArray());
                    }
                    else return "";
                }
                else return "";
            }
        }
        public string NinjiAccessories
        {
            get
            {
                if (this.LocalPetAccessoryAgent != null)
                {
                    List<string> accessories = this.LocalPetAccessoryAgent.GetPetAccessories(Roles.Ninji);
                    if (accessories != null && accessories.Count > 0)
                    {
                        return string.Join(",", accessories.ToArray());
                    }
                    else return "";
                }
                else return "";
            }
        }
        public string SansaAccessories
        {
            get
            {
                if (this.LocalPetAccessoryAgent != null)
                {
                    List<string> accessories = this.LocalPetAccessoryAgent.GetPetAccessories(Roles.Sansa);
                    if (accessories != null && accessories.Count > 0)
                    {
                        return string.Join(",", accessories.ToArray());
                    }
                    else return "";
                }
                else return "";
            }
        }
        public string YoyoAccessories
        {
            get
            {
                if (this.LocalPetAccessoryAgent != null)
                {
                    List<string> accessories = this.LocalPetAccessoryAgent.GetPetAccessories(Roles.Yoyo);
                    if (accessories != null && accessories.Count > 0)
                    {
                        return string.Join(",", accessories.ToArray());
                    }
                    else return "";
                }
                else return "";
            }
        }
        public string NuoAccessories
        {
            get
            {
                if (this.LocalPetAccessoryAgent != null)
                {
                    List<string> accessories = this.LocalPetAccessoryAgent.GetPetAccessories(Roles.Nuo);
                    if (accessories != null && accessories.Count > 0)
                    {
                        return string.Join(",", accessories.ToArray());
                    }
                    else return "";
                }
                else return "";
            }
        }

        public int GetMissionID(int starID, int defaultMissionID = -1)
        {
            //if (this.playerData != null && this.playerData.playerMissionId != null)
            //{
            //    Debug.LogFormat("<><PlayerDataManager.GetMissionID>StarID: {0}", starID);
            //    foreach (var kvp in this.playerData.playerMissionId)
            //    {
            //        Debug.LogFormat("<><PlayerDataManager.GetMissionID>All missionID, StarID: {0}, MissionID: {1}", kvp.Key, kvp.Value);
            //    }
            //}

            if (this.playerData != null && this.playerData.playerMissionId != null &&
                this.playerData.playerMissionId.ContainsKey(starID))
                return playerData.playerMissionId[starID];
            else return defaultMissionID;
        }
        public void SetMissionID(int starID, int missionID)
        {
            if (this.playerData == null || this.playerData.playerMissionId == null)
            {
                Debug.LogErrorFormat("<><PlayerDataManager.SetMissionID>playerData is null, StarID: {0}, MissionID: {1}", starID, missionID);
                return;
            }
            else if (!this.missionConfig.ExistStar(starID) || !this.missionConfig.ExistMission(missionID) ||
                     starID != this.missionConfig.GetStarIDByMissionID(missionID))
            {
                Debug.LogErrorFormat("<><PlayerDataManager.SetMissionID>StarID or MissionID not exist, StarID: {0}, MissionID: {1}", starID, missionID);
                return;
            }

            WorldInfo worldInfo = this.missionConfig.GetWorldByStarID(this.missionConfig.GetStarIDByMissionID(missionID));
            if (worldInfo != null && worldInfo.Type == WorldInfo.WorldTypes.Duplicate && this.missionDataManager.GetMisionState(missionID) == MissionState.eLock)
            {//副本星球接收服务器的MissionID时，如果发现收到的MissionID是eLock状态的，则忽略
                Debug.LogWarningFormat("<><PlayerDataManager.SetMissionID>Mission state is lock, MissionID: {0}", missionID);
                return;
            }

            if (!this.playerData.playerMissionId.ContainsKey(starID))
                playerData.playerMissionId.Add(starID, missionID);
            else
                playerData.playerMissionId[starID] = missionID;
            SaveMissionId();
        }

        public float GetMissionPercent(int starID, int sceneIndex)
        {
            WorldInfo worldInfo = this.missionConfig.GetWorldByStarID(starID);
            if (worldInfo != null && this.playerData != null && this.playerData.playerMissionPercent != null)
            {//主线星球按星球计算关卡进度, 副本星球按地图计算关卡进度
                string key = worldInfo.Type == WorldInfo.WorldTypes.Normal ? starID.ToString() : string.Format("{0}_{1}", starID, sceneIndex);
                return this.playerData.playerMissionPercent.ContainsKey(key) ? this.playerData.playerMissionPercent[key] : 0;
            }
            else
                return 0;
        }
        public void ClearMissionPercent()
        {
            if (this.playerData != null)
                this.playerData.playerMissionPercent = new Dictionary<string, float>();
        }

        public float GetExpPercent()
        {
            return exp / (float)expForLevelUp;
        }

        public int GetDailyGoal()
        {
            return dailyGoal;
        }
        public void SetDailyGoal(int value)
        {
            dailyGoal = value;
            GuLog.Debug("<><PlayerData>playerData_DailyGoal:" + value.ToString());
            SaveDailyGoal();
        }

        public void TrySendPlayerData2Server()
        {
            bDirty = true;

            string param = gameDataHelper.GetGameDataJson(this);
            startPostGameDataCommand.Dispatch(param);

            //             string prod_name = AppData.GetAppName();
            //             string x_child_sn = mLocalChildInfoAgent.getChildSN();
            //             string x_cup_sn = CupBuild.getCupSn();
            //             mGululuNetwork.sendRequest(mUrlProvider.GameDataUrl(x_child_sn, prod_name, x_cup_sn), null, param, (r) => {
            //                 bDirty = false;
            //             }, (e) => {
            //             }, HTTPMethods.Post);
        }
        public void SetValuesLong(GameData datas)
        {
            if (datas.playerData == null || datas.playerData.getLevel() < playerLevel)
            {
                return;
            }
            playerLevel = datas.playerData.getLevel();
            StarID = datas.playerData.getStarID();
            SceneIndex = datas.playerData.getSceneIndex();
            missionId = datas.playerData.getCurrent_mission_id();
            missionPercent = datas.playerData.getCurrent_mission_percent() / (float)100;
            CrownCount = datas.playerData.getCrown_count();
            exp = datas.playerData.exp;
            this.LocalPetAccessoryAgent.SavePetAccessories(Roles.Purpie, this.CombineAccessories(datas.playerData.getPurpieAccessories()));
            this.LocalPetAccessoryAgent.SavePetAccessories(Roles.Donny, this.CombineAccessories(datas.playerData.getDonnyAccessories()));
            this.LocalPetAccessoryAgent.SavePetAccessories(Roles.Ninji, this.CombineAccessories(datas.playerData.getNinjiAccessories()));
            this.LocalPetAccessoryAgent.SavePetAccessories(Roles.Sansa, this.CombineAccessories(datas.playerData.getSansaAccessories()));
            this.LocalPetAccessoryAgent.SavePetAccessories(Roles.Yoyo, this.CombineAccessories(datas.playerData.getYoyoAccessories()));
            this.LocalPetAccessoryAgent.SavePetAccessories(Roles.Nuo, this.CombineAccessories(datas.playerData.getNuoAccessories()));
            Debug.LogFormat("<><PlayerDataManager.SetValuesLong>StarID: {0}", StarID);
        }
        List<string> CombineAccessories(string accessories)
        {
            if (!string.IsNullOrEmpty(accessories))
            {
                string[] parts = accessories.Split(',');
                if (parts != null && parts.Length > 0)
                {
                    return new List<string>(parts);
                }
                else return new List<string>();
            }
            else return new List<string>();
        }
        void SendAllWhenInternetReconnect()
        {
            if (bDirty)
            {
                TrySendPlayerData2Server();
            }
        }
        void SaveIntake()
        {
            saveObject<int>("Intake", currIntake);
        }
        void SaveCrownGotCount()
        {
            saveObject<int>("crownTodayGotCount", todayCrownGotCount);
        }
        void SaveDailyGoal()
        {
            saveObject<int>("DailyGoal", dailyGoal);
        }
        void SavePlayerLevel()
        {
            saveObject<int>("PlayerLevel", playerLevel);
        }
        void SavePlayerExp()
        {
            saveObject<int>("PlayerExp", exp);
        }
        void SaveStarID()
        {
            saveObject<int>("StarID", StarID);
        }
        void SaveSceneIndex()
        {
            saveObject<int>("SceneIndex", SceneIndex);
        }
        void SaveMissionPercent()
        {
            saveObject<string>("MissionPercent4DicNew", mJsonUtils.Json2String(this.playerData.playerMissionPercent));
        }
        void SaveMissionId()
        {
            saveObject<string>("MissionId4Dic", mJsonUtils.Json2String(this.playerData.playerMissionId));
        }
        void SaveVoicePercent()
        {
            saveObject<int>("SoundPercent", voicePercentBeforeSilenceMode);
            _voicePercent = getObject<int>("SoundPercent", 67);
        }

        public void NewDay(int currDay)
        {
            GuLog.Debug("<><PlayerData>playerData_NewDay");
            date = currDay;
            currIntake = 0;
            todayCrownGotCount = 0;
            TodayDrinkTimes = 0;

            if (missionPercent >= 1)
            {
                int nextMissionId = missionConfig.GetNextMissionId(missionId);
                if (nextMissionId != -1)
                {
                    missionId = nextMissionId;
                    missionDataManager.AddMissionState(missionId, MissionState.eUnderway);
                }
            }
        }
        public void LoadPlayerDatas()
        {
            int savedDailyGoal = getObject<int>("DailyGoal");
            if (savedDailyGoal != 0)
            {
                dailyGoal = savedDailyGoal;
            }
            Debug.Log("<><PlayerData>Read_DailyGoal:" + dailyGoal.ToString());

            playerData.starID = getObject<int>("StarID", PlayerData.defaultStar);
            this.missionConfig.SetStarID(playerData.starID);
            Debug.Log("<><PlayerData>Read_starID:" + playerData.starID.ToString());
            playerData.sceneIndex = getObject<int>("SceneIndex", 1);
            Debug.Log("<><PlayerData>Read_sceneIndex:" + playerData.sceneIndex.ToString());
            playerData.playerLevel = getObject<int>("PlayerLevel", 1);
            Debug.Log("<><PlayerData>Read_playerLevel:" + playerData.playerLevel.ToString());
            playerData.playerExp = getObject<int>("PlayerExp");
            Debug.Log("<><PlayerData>Read_playerExp:" + playerData.playerExp.ToString());

            //读取所有星球饮水百分比
            Dictionary<string, float> missionPercent = this.mJsonUtils.String2Json<Dictionary<string, float>>(getObject<string>("MissionPercent4DicNew"));
            if (missionPercent == null)
            {//先按照新的Key值读取不到数据，则需要做首次处理
                this.missionPercent = 0;
                this.SaveMissionPercent();
            }
            else
            {//否则，仅将数据记录到内存
                playerData.playerMissionPercent = missionPercent;
            }
            Debug.Log("<><PlayerData>Read_playerMissionPercent:" + this.missionPercent);

            //读取所有星球关卡ID
            Dictionary<int, int> missionId = this.mJsonUtils.String2Json<Dictionary<int, int>>(getObject<string>("MissionId4Dic"));
            if (missionId == null)
            {//先按照新的Key值读取不到数据，则需要做首次处理
                int oldValue = getObject<int>("MissionId", PlayerData.startMission);
                missionId = new Dictionary<int, int>();
                missionId.Add(this.missionConfig.GetStarIDByMissionID(oldValue), oldValue);
                playerData.playerMissionId = missionId;
                this.SaveMissionId();
            }
            else
            {//否则，仅将数据记录到内存
                playerData.playerMissionId = missionId;
            }
            Debug.Log("<><PlayerData>Read_playerMissionId:" + (playerData.playerMissionId.ContainsKey(this.playerData.starID) ?
                        playerData.playerMissionId[this.playerData.starID].ToString() : "None"));

            playerData.playerCrownCount = getObject<int>("CrownCount");
            Debug.Log("<><PlayerData>Read_playerCrownCount:" + playerData.playerCrownCount.ToString());

            _voicePercent = getObject<int>("SoundPercent", 67);

            playerTodayData.todayDrinkTimes = getObject<int>("DrinkTimes");
            Debug.Log("<><PlayerData>Read_todayDrinkTimes:" + playerTodayData.todayDrinkTimes.ToString());
            playerTodayData.todayDate = getObject<int>("Date");
            Debug.Log("<><PlayerData>Read_todayDate:" + playerTodayData.todayDate.ToString());

            int currDay = DateUtil.ConvertToDay(DateTime.Today);
            if (playerTodayData.todayDate == 0 || playerTodayData.todayDate != currDay)
            {
                NewDay(currDay);
            }
            else
            {
                currIntake = getObject<int>("Intake");
                Debug.Log("<><PlayerData>Read_todayIntake:" + playerTodayData.todayIntake.ToString());

                playerTodayData.todayCrownGotCount = getObject<int>("crownTodayGotCount");
                Debug.Log("<><PlayerData>Read_todayCrownGotCount:" + playerTodayData.todayCrownGotCount.ToString());
            }
        }

        public int GetDailyGoalExp(int type)
        {
            switch (type)
            {
                case 0:
                    {
                        return 10 + 2 * (playerLevel - 1);
                    }
                case 1:
                    {
                        return 15 + 3 * (playerLevel - 1);
                    }
                case 2:
                    {
                        return 20 + 4 * (playerLevel - 1);
                    }
            }
            return 0;
        }
        public int GetDailyGoalExp(float percent)
        {
            return (this.levelConifg.GetDailyGoalExp(this.playerLevel, percent));
        }
        public bool IsNovice()
        {
            return (this.currIntake == 0) && (playerData.playerExp == 0) && (missionPercent == 0);
        }

        public void RecoverVolume()
        {
            if (voicePercentBeforeSilenceMode != 0)
            {
#if (UNITY_ANDROID) && (!UNITY_EDITOR)
	                AudioControl.setVolume(voicePercentBeforeSilenceMode);
#endif
                voicePercentBeforeSilenceMode = 0;
            }
        }
        public void SetVolumeZero()
        {
#if (UNITY_ANDROID) && (!UNITY_EDITOR)
	            if(voicePercentBeforeSilenceMode == 0)
	            {
	                voicePercentBeforeSilenceMode = AudioControl.getVolume();
	            }
	            AudioControl.setVolume(0);
#else
            voicePercentBeforeSilenceMode = 7;
#endif
        }

        T getObject<T>(string key)
        {
            return gameDataHelper.GetObject<T>(key);
        }
        T getObject<T>(string key, object defaultObj)
        {
            return gameDataHelper.GetObject<T>(key, defaultObj);
        }
        void saveObject<T>(string key, T t)
        {
            gameDataHelper.SaveObject<T>(key, t);
        }

        public bool LanguageInitialized()
        {
            string languageShortName = this.getObject<string>("Language");
            Debug.LogFormat("----PlayerDataManager.LanguageInitialized----Languange: {0}, Valid: {1}", languageShortName, this.LanConfig.IsValid(languageShortName));
            return this.LanConfig.IsValid(languageShortName);
        }
        public string GetLanguage()
        {
            string languageShortName = this.getObject<string>("Language");
            if (this.LanConfig.IsValid(languageShortName))
                this.Language = languageShortName;
            Debug.LogFormat("----PlayerDataManager.GetLanguage----Languange: {0}", languageShortName);
            return languageShortName;
        }
        public void SaveLanguage(string languageShortName)
        {
            if (this.LanConfig.IsValid(languageShortName))
            {
                this.Language = languageShortName;
                this.saveObject<string>("Language", languageShortName);
                Debug.LogFormat("----PlayerDataManager.SaveLanguage----Languange: {0}", languageShortName);
            }
            else
            {
                GuLog.Error(string.Format("----PlayerDataManager.SaveVoicePercent----Save error, language: {0}", languageShortName));
            }
        }
        public void ClearLanguage()
        {
            this.saveObject<string>("Language", "");
        }
    }
}
