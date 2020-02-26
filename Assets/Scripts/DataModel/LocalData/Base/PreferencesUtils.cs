using UnityEngine;

namespace Gululu
{
    public class PreferenceUtils : IPreferencesUtils
    {
        private static IPreferencesUtils mPreferencesUtils;

        public IJsonUtils mJsonUtils;

        public static IPreferencesUtils getInstance()
        {
            if (mPreferencesUtils == null)
            {
                mPreferencesUtils = new PreferenceUtils();
            }

            return mPreferencesUtils;
        }


        public PreferenceUtils()
        {
            mJsonUtils = new JsonUtils();
        }

        public void saveObject<T>(string key, T t)
        {
            if (t == null)
            {
                return;
            }
            if (typeof(T).Name == typeof(string).Name)
            {
                object result = t;
                PlayerPrefs.SetString(key, (string)result);
            }
            else if (typeof(T).Name == typeof(float).Name)
            {
                object result = t;
                PlayerPrefs.SetFloat(key, (float)result);
            }
            else if (typeof(T).Name == typeof(int).Name)
            {
                object result = t;
                PlayerPrefs.SetInt(key, (int)result);
            }
            else if (typeof(T).Name == typeof(bool).Name)
            {
                bool status = (bool)((object)t);
                int saveResult;

                if (status)
                {
                    saveResult = 1;
                }
                else
                {
                    saveResult = 0;
                }
                PlayerPrefs.SetInt(key + "bool", saveResult);
            }
            else
            {
                string info = mJsonUtils.Json2String(t);
                PlayerPrefs.SetString(key, info);
            }
            PlayerPrefs.Save();
        }
        public T getObject<T>(string key, object objDefault)
        {
            if (typeof(T).Name == typeof(string).Name)
            {
                object info = (object)PlayerPrefs.GetString(key: key, defaultValue: (string)objDefault);
                return (T)info;
            }
            else if (typeof(T).Name == typeof(float).Name)
            {
                object info = (object)PlayerPrefs.GetFloat(key: key, defaultValue: (float)objDefault);
                return (T)info;
            }
            else if (typeof(T).Name == typeof(int).Name)
            {
                object info = (object)PlayerPrefs.GetInt(key: key, defaultValue: (int)objDefault);
                return (T)info;
            }
            else if (typeof(T).Name == typeof(bool).Name)
            {
                int info = PlayerPrefs.GetInt(key + "bool", defaultValue: ((bool)objDefault) ? 1 : 0);
                bool result;

                if (info == 1)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }

                return (T)((object)result);
            }
            else
            {
                string info = PlayerPrefs.GetString(key, "");
                if ("".Equals(info))
                {
                    return default(T);
                }
                T obj = mJsonUtils.String2Json<T>(info);

                return obj;
            }

        }

        public T getObject<T>(string key)
        {
            if (typeof(T).Name == typeof(string).Name)
            {
                object info = (object)PlayerPrefs.GetString(key: key, defaultValue: "");
                return (T)info;
            }
            else if (typeof(T).Name == typeof(float).Name)
            {
                object info = (object)PlayerPrefs.GetFloat(key: key, defaultValue: 0);
                return (T)info;
            }
            else if (typeof(T).Name == typeof(int).Name)
            {
                object info = (object)PlayerPrefs.GetInt(key: key, defaultValue: 0);
                return (T)info;
            }
            else if (typeof(T).Name == typeof(bool).Name)
            {
                int info = PlayerPrefs.GetInt(key + "bool", defaultValue: 0);
                bool result;

                if (info == 1)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }

                return (T)((object)result);
            }
            else
            {
                string info = PlayerPrefs.GetString(key, "");
                if ("".Equals(info))
                {
                    return default(T);
                }
                T obj = mJsonUtils.String2Json<T>(info);

                return obj;
            }

        }

        public void deleteAll()
        {
            GuLog.Info("PreferenceUtils deleteAll");
            PlayerPrefs.DeleteAll();
        }

        public void deleteObject(string key)
        {
            PlayerPrefs.DeleteKey(key);
        }
    }
}