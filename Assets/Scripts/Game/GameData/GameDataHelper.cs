using Gululu;
using Gululu.LocalData.Agents;
using Gululu.Util;
using Hank.Api;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hank
{
    public class GameDataHelper : IGameDataHelper
    {
        [Inject]
        public ILocalChildInfoAgent localChildInfoAgent { get; set; }
        [Inject]
        public ILocalDataManager localDataRepository { get; set; }
        [Inject]
        public ILocalPetInfoAgent LocalPetInfoAgent { get; set; }
        [Inject]
        public IJsonUtils mJsonUtils { get; set; }

        public string GetGameDataJson(IPlayerDataManager dataManager)
        {
            GameData data = GameData.getGameDataBuilder()
                .setPlayerData(
                    PlayerDataBean.getPlayerDataBeanBuilder()
                    .setCurrent_mission_id(dataManager.missionId)
                    .setCurrent_mission_percent((int)dataManager.missionPercent > 1 ? 100 : (int)dataManager.missionPercent * 100)
                    .setExp(dataManager.exp)
                    .setLevel(dataManager.playerLevel)
                    .setCrown_count(dataManager.CrownCount)
                    .setStarID(dataManager.StarID)
                    .setSceneIndex(dataManager.SceneIndex)
                    .setPurpieAccessories(dataManager.PurpieAccessories)
                    .setDonnyAccessories(dataManager.DonnyAccessories)
                    .setNinjiAccessories(dataManager.NinjiAccessories)
                    .setSansaAccessories(dataManager.SansaAccessories)
                    .setYoyoAccessories(dataManager.YoyoAccessories)
                    .setNuoAccessories(dataManager.NuoAccessories)
                    .build()
                )
                .setTimestamp((int)DateUtil.GetTimeStamp())
                .build();

            string jsonStr = mJsonUtils.Json2String(data);
            return jsonStr;
        }
        public T GetObject<T>(string key)
        {
            T t = localDataRepository.getObject<T>(localChildInfoAgent.getChildSN() + key);
            //             GuLog.Log("<><localChildInfoAgent>getObject key:" + localChildInfoAgent.getChildSN() + key + "    Value:" + t.ToString());
            return t;
        }
        public T GetObject<T>(string key, object defaultObj)
        {
            T t = localDataRepository.getObject<T>(localChildInfoAgent.getChildSN() + key, defaultObj);

            //             GuLog.Log("<><localChildInfoAgent>getObject key:" + localChildInfoAgent.getChildSN() + key + "    Value:" + t.ToString());
            return t;
        }
        public void SaveObject<T>(string key, T t)
        {
            //localDataRepository.saveObject<string>(localChildInfoAgent.getChildSN() + "TimeStamp", DateUtil.GetTimeStamp().ToString());
            localDataRepository.saveObject<T>(localChildInfoAgent.getChildSN() + key, t);
            //GuLog.Log("<><localChildInfoAgent>saveObject key:" + localChildInfoAgent.getChildSN() + key + "    Value:" + t.ToString());
        }
        public void Clear(string key)
        {
            localDataRepository.removeObject(localChildInfoAgent.getChildSN() + key);
        }

        static void ParserGameDataJson(string jsonStr)
        {

        }
    }
}
