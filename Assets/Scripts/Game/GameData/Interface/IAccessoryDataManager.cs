using System.Collections.Generic;

namespace Hank
{
    public interface IAccessoryDataManager
    {
        void AddItem(int accessoryID, int accessoryCount = 1);
        void SetItem(int accessoryID, int accessoryCount);
        bool HasItem(int accessoryID, int accessoryCount = 1);
        int GetItemCount(int accessoryCount);
        void LoadItemsData();
        void SaveItemsData();
        void SendAllItemsData(bool force);
        void SetValuesLong(Hank.Api.GameData datas);
        List<int> GetAllItemInfo();
    }
}

