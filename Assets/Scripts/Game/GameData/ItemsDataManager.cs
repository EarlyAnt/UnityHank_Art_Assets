using Gululu;
using Gululu.Config;
using Gululu.LocalData.Agents;
using Gululu.Util;
using Hank.Api;
using Hank.MainScene;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Hank
{
    public class ItemsDataManager : IItemsDataManager
    {
        [Inject]
        public IJsonUtils mJsonUtils { set; get; }
        [Inject]
        public IGameDataHelper gameDataHelper { get; set; }
        [Inject]
        public ILocalChildInfoAgent mLocalChildInfoAgent { get; set; }
        [Inject]
        public StartPostGameDataCommandSignal startPostGameDataCommand { get; set; }

        [Inject]
        public IItemConfig itemConifg { get; set; }

        public const string itemsKey = "ItemsData";

        Dictionary<int, int> itemsInfo = new Dictionary<int, int>();

        struct ItemInfo
        {
            public int itemId;
            public int itemCount;
        }

        [PostConstruct]
        public void PostConstruct()
        {
            LoadItemsData();
        }

        public int GetItemCount(int itemId)
        {
            int itemCount;
            if (itemsInfo.TryGetValue(itemId, out itemCount))
            {
                return itemCount;
            }
            return 0;

        }

        public void SetItem(int itemId, int itemCount)
        {
            Debug.LogFormat("--------{0}-------ItemsDataManager.SetItem: {1}, {2}", System.DateTime.Now, itemId, itemCount);
            int count = 0;
            if (itemsInfo.TryGetValue(itemId, out count))
            {
                itemsInfo.Remove(itemId);
            }

            if (itemCount > 0)
            {
                itemsInfo.Add(itemId, itemCount);
            }
        }

        public void AddItem(int itemId, int itemCount = 1)
        {
            int count = 0;
            if (itemsInfo.TryGetValue(itemId, out count))
            {
                itemsInfo.Remove(itemId);
            }
            int newCount = count;
            if (int.MaxValue - itemCount > count)
            {
                newCount = itemCount + count;
            }
            itemsInfo.Add(itemId, newCount);

            SaveItemsData();
            SyncItemsData2Server(itemId, count + itemCount);
        }

        private void SendItemsData(List<ItemsDataBean> dataItems)
        {
            GameData datas = GameData.getGameDataBuilder()
                .setItemsData(dataItems)
                .setTimestamp((int)DateUtil.GetTimeStamp())
                .build();
            string jsonData = mJsonUtils.Json2String(datas);
            startPostGameDataCommand.Dispatch(jsonData);
            Debug.LogFormat("----{0}====ItemsDataManager.SendItemsData: {1}", System.DateTime.Now, jsonData);
        }

        void SyncItemsData2Server(int itemId, int itemCount)
        {
            Item item = itemConifg.GetItemById(itemId);
            if (item != null)
            {
                List<ItemsDataBean> items = new List<ItemsDataBean>();
                items.Add(new ItemsDataBean
                {
                    item_id = itemId,
                    type = item.Type,
                    count = itemCount,
                    name = item.Name
                });
                SendItemsData(items);
            }
        }

        public void SendAllItemsData(bool force)
        {
            List<ItemsDataBean> items = new List<ItemsDataBean>();
            //StringBuilder strbContent = new StringBuilder();
            foreach (var pair in itemsInfo)
            {
                Item item = itemConifg.GetItemById(pair.Key);
                if (item != null)
                {
                    items.Add(new ItemsDataBean
                    {
                        item_id = pair.Key,
                        type = item.Type,
                        count = pair.Value,
                        name = item.Name
                    });
                    //strbContent.AppendFormat("ID: {0}, Type: {1}, Count: {2}, Name: {3}\n", pair.Key, item.Type, pair.Value, item.Name);
                }
            }
            SendItemsData(items);
            //Debug.LogFormat("<><ItemsDataManager.SendAllItemsData>Datas: {0}", strbContent.ToString());
        }
        public void SetValuesLong(GameData datas)
        {
            List<ItemsDataBean> listInfos = datas.getItemsData();
            if (listInfos != null)
            {
                for (int i = 0; i < listInfos.Count; ++i)
                {
                    AddItem(listInfos[i].getItem_id(), listInfos[i].getCount());
                }
            }
        }

        private List<ItemInfo> Convert2List()
        {
            List<ItemInfo> listInfos = new List<ItemInfo>();
            foreach (var pair in itemsInfo)
            {
                listInfos.Add(new ItemInfo
                {
                    itemId = pair.Key,
                    itemCount = pair.Value
                });
            }
            return listInfos;
        }

        public void SaveItemsData()
        {
            List<ItemInfo> listInfos = Convert2List();

            gameDataHelper.SaveObject<List<ItemInfo>>(itemsKey, listInfos);

        }
        public void LoadItemsData()
        {
            itemsInfo.Clear();
            List<ItemInfo> listInfos = gameDataHelper.GetObject<List<ItemInfo>>(itemsKey);
            if (listInfos != null)
            {
                foreach (var info in listInfos)
                {
                    itemsInfo.Add(info.itemId, info.itemCount);
                    GuLog.Debug("<><ItemsData>itemId:" + info.itemId + "    Count:" + info.itemCount.ToString());
                }
            }
        }

        public List<int> GetAllItemInfo()
        {
            return this.itemsInfo == null ? null : this.itemsInfo.Keys.ToList();
        }

        public bool HasItem(int itemId, int itemCount = 1)
        {
            int count = 0;
            if (itemsInfo.TryGetValue(itemId, out count))
            {
                return count >= itemCount;
            }
            return false;
        }
    }
}

