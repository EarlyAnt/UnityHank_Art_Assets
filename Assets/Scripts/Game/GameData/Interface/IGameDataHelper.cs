namespace Hank
{
    public interface IGameDataHelper
    {
        string GetGameDataJson(IPlayerDataManager dataManager);
        T GetObject<T>(string key);
        T GetObject<T>(string key, object defaultObj);
        void SaveObject<T>(string key, T t);        
        void Clear(string key);
    }
}