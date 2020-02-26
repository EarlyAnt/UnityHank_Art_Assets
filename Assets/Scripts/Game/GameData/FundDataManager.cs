using System.Collections.Generic;
using Gululu;
using Gululu.Config;
using Gululu.LocalData.Agents;
using Gululu.Util;
using Hank.Api;
using Hank.MainScene;
using UnityEngine;

namespace Hank
{
    public class FundDataManager : IFundDataManager
    {
        [Inject] public IGameDataHelper gameDataHelper { get; set; }

        [Inject] public FundDataChangedSignal fundDataChangedSignal { get; set; }

        [Inject]
        public IJsonUtils mJsonUtils { set; get; }

        [Inject]
        public StartPostGameDataCommandSignal startPostGameDataCommand { get; set; }
        
        [Inject]
        public ILocalChildInfoAgent mLocalChildInfoAgent { get; set; }

        [Inject]
        public IPropertyConfig propertyConifg { get; set; }

        private const string SAVE_PROPERTY_DATA_KEY = "SAVE_PROPERTY_DATA_KEY";

        private const string FIRST_INSTALL_PET_FEED_SCENE = "FIRST_INSTALL_PET_FEED_SCENE";

        private const string HAVE_DELETE_KEY = "HAVE_DELETE_KEY";

        private const int DEFAULT_COIN_COUNT = 200;

        private const int COIN_TYPE = 5;

        private const int PROPERTY_TYPE = 6;

        public void InitInfo()
        {
            string childSN = string.IsNullOrEmpty(mLocalChildInfoAgent.getChildSN())
                ? ""
                : mLocalChildInfoAgent.getChildSN();

            if (PlayerPrefs.GetInt(HAVE_DELETE_KEY, 0) == 0)
            {
                PlayerPrefs.DeleteKey(FIRST_INSTALL_PET_FEED_SCENE + childSN);
                PlayerPrefs.SetInt(HAVE_DELETE_KEY, 1);
            }

            var installed = PlayerPrefs.GetInt(FIRST_INSTALL_PET_FEED_SCENE + childSN, 0) != 0;

            if (!installed)
            {
                if (GetItemCount(GetCoinId()) <= 0)
                {
                    AddCoin(GetCoinId(), DEFAULT_COIN_COUNT);
                }
                PlayerPrefs.SetInt(FIRST_INSTALL_PET_FEED_SCENE + childSN, 1);
            }
//            AddCoin(69003, 1);
        }

        public void AddCoin(int id, int count)
        {
            Debug.LogFormat("----FundDataManager.AddCoin----id: {0}, count: {1}", id, count);
            if (count <= 0) return;
            AddProperty(id, count);
        }

        public bool ExpendCoin(int id, int count)
        {
            Debug.LogFormat("----FundDataManager.ExpendCoin----id: {0}, count: {1}", id, count);
            if (count <= 0) return false;
            return ExpendProperty(id, count);
        }

        public void AddProperty(int id, int count)
        {
            var items = ReadData();
            if (items != null)
            {
                var propertyItem = items.Find(item => item.Id == id);

                if (propertyItem != null)
                {
                    propertyItem.Count += count;
                }
                else
                {
                    var tempItem = propertyConifg.GetPropertyByID(id);
                    if (tempItem == null)
                    {
                        if (id == GetCoinId())
                        {
                            tempItem = new PropertyItem();
                            tempItem.Name = "Coin";
                            tempItem.Type = 5;
                        }
                    }

                    SavePropertyDataItem savePropertyDataItem = new SavePropertyDataItem
                    {
                        Id = id,
                        Count = count,
                        Name = tempItem.Name,
                        Type = tempItem.Type
                    };
                    items.Add(savePropertyDataItem);
                }
                WriteData(items);
            }
            else
            {
                var tempItem = propertyConifg.GetPropertyByID(id);

                if (tempItem == null)
                {
                    if (id == GetCoinId())
                    {
                        tempItem = new PropertyItem();
                        tempItem.Name = "Coin";
                        tempItem.Type = 5;
                    }
                }
                
                SavePropertyDataItem savePropertyDataItem = new SavePropertyDataItem
                {
                    Id = id,
                    Count = count,
                    Name = tempItem.Name,
                    Type = tempItem.Type
                };
                var list = new List<SavePropertyDataItem> { savePropertyDataItem };
                WriteData(list);
            }

            if (id != GetCoinId())
            {
                fundDataChangedSignal.Dispatch();
            }
        }

        public bool ExpendProperty(int id, int count)
        {
            var items = ReadData();
            if (items != null)
            {
                var propertyItem = items.Find(item => item.Id == id);
                if (propertyItem != null)
                {
                    int surplus = propertyItem.Count - count;
                    if (surplus < 0)
                    {
                        return false;
                    }
                    propertyItem.Count -= count;
                    WriteData(items);
                    return true;
                }
                return false;
            }
            return false;
        }

        private void WriteData(List<SavePropertyDataItem> items)
        {
            gameDataHelper.SaveObject(SAVE_PROPERTY_DATA_KEY, items);
            SyncData2Server();
        }

        private List<SavePropertyDataItem> ReadData()
        {
            return gameDataHelper.GetObject<List<SavePropertyDataItem>>(SAVE_PROPERTY_DATA_KEY, null);
        }

        public void SyncData2Server()
        {
            var itemDataBean = new List<ItemsDataBean>();
            var localData = ReadData();
            if (localData != null)
            {
                foreach (var localItem in localData)
                {
                    itemDataBean.Add(new ItemsDataBean()
                    {
                        item_id = localItem.Id,
                        count = localItem.Count,
                        name = localItem.Name,
                        type = localItem.Type
                    });
                }
            }

            GameData datas = GameData.getGameDataBuilder()
                .setItemsData(itemDataBean)
                .setTimestamp((int)DateUtil.GetTimeStamp())
                .build();
            string jsonData = mJsonUtils.Json2String(datas);
            startPostGameDataCommand.Dispatch(jsonData);
            UnityEngine.Debug.LogFormat("----{0}====FundDataManager.SyncData2Server: {1}", System.DateTime.Now, jsonData);
        }
        
        public void SetValuesLong(GameData datas)
        {
            var localData = ReadData();
            if(localData != null)return;
            List<ItemsDataBean> listInfos = datas.getItemsData().FindAll(item => item.type == PROPERTY_TYPE || item.type == COIN_TYPE);
            if (listInfos != null)
            {
                for (int i = 0; i < listInfos.Count; ++i)
                {
                    AddProperty(listInfos[i].getItem_id(), listInfos[i].getCount());
                }
            }
        }

        public int GetItemCount(int id)
        {
            List<SavePropertyDataItem> list = ReadData();

            if (list == null)
            {
                return 0;
            }
            var tempItem = list.Find(item => item.Id == id);

            if (tempItem == null)
            {
                return 0;
            }
            return tempItem.Count < 0 ? 0 : tempItem.Count;
        }

        public int GetCoinId()
        {
            return 50001;
        }

        public List<SavePropertyDataItem> GetSaveItemList()
        {
            return ReadData();
        }
    }
    
    public class SavePropertyDataItem
    {
        public int Type;
        public int Id;
        public int Count;
        public string Name;
    }
}