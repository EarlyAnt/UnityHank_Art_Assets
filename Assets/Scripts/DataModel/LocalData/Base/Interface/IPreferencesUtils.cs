namespace Gululu
{
    public interface IPreferencesUtils
    {
         void saveObject<T>(string key, T t);

         T getObject<T>(string key,object defaultObj);
        T getObject<T>(string key);

        void deleteAll();

         void deleteObject(string key);
    }
}