using System.Collections.Generic;
using Hank.Api;

namespace Hank
{
    public interface IFundDataManager
    {
        void InitInfo();
        void AddCoin(int id,int count);
        
        void AddProperty(int id, int count);

        bool ExpendCoin(int id,int count);

        bool ExpendProperty(int id, int count);

        void SyncData2Server();

        int GetItemCount(int id);

        int GetCoinId();
        List<SavePropertyDataItem> GetSaveItemList();
        void SetValuesLong(GameData datas);
    }
}