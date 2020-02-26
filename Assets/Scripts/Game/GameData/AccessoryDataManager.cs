using Gululu;
using Gululu.Config;
using Gululu.LocalData.Agents;
using Gululu.Util;
using Hank.Api;
using Hank.MainScene;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Hank
{
    public class AccessoryDataManager : IAccessoryDataManager
    {
        [Inject]
        public IAccessoryConfig AccessoryConfig { get; set; }
        [Inject]
        public IJsonUtils JsonUtils { set; get; }
        [Inject]
        public IGameDataHelper GameDataHelper { get; set; }
        [Inject]
        public ILocalChildInfoAgent LocalChildInfoAgent { get; set; }
        [Inject]
        public StartPostGameDataCommandSignal StartPostGameDataCommand { get; set; }
        private const string DATA_KEY = "AccessoryDatas";
        private const int ACCESSORY_TYPE = 2;
        private Dictionary<int, int> accessoryInfos = new Dictionary<int, int>();
        struct AccessoryInfo
        {
            public int AccessoryID { get; set; }
            public int AccessoryCount { get; set; }
        }

        [PostConstruct]
        public void PostConstruct()
        {
            LoadItemsData();
        }
        public void AddItem(int accessoryID, int accessoryCount = 1)
        {
            int count = 0;
            if (accessoryInfos.TryGetValue(accessoryID, out count))
            {
                accessoryInfos.Remove(accessoryID);
            }
            int newCount = count;
            if (int.MaxValue - accessoryCount > count)
            {
                newCount = accessoryCount + count;
            }
            accessoryInfos.Add(accessoryID, newCount);

            SaveItemsData();
            SyncItemsData2Server(accessoryID, count + accessoryCount);
            Debug.LogFormat("<><AccessoryDataManager.AddItem>: {0}, {1}", accessoryID, accessoryCount);
        }
        public void SetItem(int accessoryID, int accessoryCount)
        {
            int count = 0;
            if (accessoryInfos.TryGetValue(accessoryID, out count))
            {
                accessoryInfos.Remove(accessoryID);
            }

            if (accessoryCount > 0)
            {
                accessoryInfos.Add(accessoryID, accessoryCount);
            }
            Debug.LogFormat("<><AccessoryDataManager.SetItem>: {0}, {1}", accessoryID, accessoryCount);
        }
        public bool HasItem(int accessoryID, int accessoryCount = 1)
        {
            int count = 0;
            if (accessoryInfos.TryGetValue(accessoryID, out count))
            {
                return count >= accessoryCount;
            }
            return false;
        }
        public int GetItemCount(int accessoryID)
        {
            int itemCount;
            if (accessoryInfos.TryGetValue(accessoryID, out itemCount))
            {
                return itemCount;
            }
            return 0;

        }
        public void LoadItemsData()
        {
            accessoryInfos.Clear();
            List<AccessoryInfo> listInfos = GameDataHelper.GetObject<List<AccessoryInfo>>(DATA_KEY);
            if (listInfos != null)
            {
                foreach (var info in listInfos)
                {
                    accessoryInfos.Add(info.AccessoryID, info.AccessoryCount);
                    Debug.LogFormat("<><AccessoryDataManager.LoadItemsData>ItemId: {0}, Count: {1}", info.AccessoryID, info.AccessoryCount);
                }
            }
        }
        public void SaveItemsData()
        {
            List<AccessoryInfo> listInfos = Convert2List();
            this.GameDataHelper.SaveObject<List<AccessoryInfo>>(DATA_KEY, listInfos);
        }
        public void SendAllItemsData(bool force)
        {
            {
                List<ItemsDataBean> items = new List<ItemsDataBean>();

                foreach (var pair in accessoryInfos)
                {
                    Accessory accessory = this.AccessoryConfig.GetAccessory(pair.Key);
                    if (accessory != null)
                    {
                        items.Add(new ItemsDataBean
                        {
                            item_id = pair.Key,
                            type = accessory.Type,
                            count = pair.Value,
                            name = accessory.Name
                        });
                    }
                }
                SendItemsData(items);
            }
        }
        public void SetValuesLong(GameData datas)
        {
            List<ItemsDataBean> listInfos = datas.getItemsData();
            if (listInfos != null)
            {
                Debug.LogFormat("<><AccessoryDataManager.SetValuesLong>listInfo: {0}", listInfos.Count);
                List<ItemsDataBean> accessoryItems = listInfos.FindAll(t => t.type == ACCESSORY_TYPE);
                if (accessoryItems != null)
                {
                    Debug.LogFormat("<><AccessoryDataManager.SetValuesLong>accessoryItems: {0}", accessoryItems.Count);
                    for (int i = 0; i < accessoryItems.Count; ++i)
                    {
                        AddItem(accessoryItems[i].getItem_id(), accessoryItems[i].getCount());
                    }
                }
            }
        }
        private void SyncItemsData2Server(int accessoryID, int accessoryCount)
        {
            Accessory accessory = AccessoryConfig.GetAccessory(accessoryID);
            if (accessory != null)
            {
                List<ItemsDataBean> items = new List<ItemsDataBean>();
                items.Add(new ItemsDataBean
                {
                    item_id = accessoryID,
                    type = accessory.Type,
                    count = accessoryCount,
                    name = accessory.Name
                });
                SendItemsData(items);
            }
        }
        public List<int> GetAllItemInfo()
        {
            return this.accessoryInfos == null ? null : this.accessoryInfos.Keys.ToList();
        }
        private void SendItemsData(List<ItemsDataBean> dataItems)
        {
            GameData datas = GameData.getGameDataBuilder()
                .setItemsData(dataItems)
                .setTimestamp((int)DateUtil.GetTimeStamp())
                .build();
            string jsonData = JsonUtils.Json2String(datas);
            StartPostGameDataCommand.Dispatch(jsonData);
            Debug.LogFormat("<><AccessoryDataManager.SendItemsData>Json Data: {0}", jsonData);
        }
        private List<AccessoryInfo> Convert2List()
        {
            List<AccessoryInfo> listInfos = new List<AccessoryInfo>();
            foreach (var pair in accessoryInfos)
            {
                listInfos.Add(new AccessoryInfo
                {
                    AccessoryID = pair.Key,
                    AccessoryCount = pair.Value
                });
            }
            return listInfos;
        }
    }
}

