namespace Gululu
{
    public interface ILocalDataManager
    {
         void saveObject<T>(string key, T t);

         void removeObject(string key);

         T getObject<T>(string key, object defaultObj);
        T getObject<T>(string key);

        void deleteAll();

        bool saveSpecialObject<T>(string key, T value);
        T getSpecialObject<T>(string key);
        void deleteAll(bool isRealDeleteAll);
    }
}