
using System;
using System.Collections.Generic;

namespace Gululu
{
    public class LocalDataManager:ILocalDataManager
    {
        private IFileManager fileManager;

        private IPreferencesUtils preferences;
        private JsonUtils mJsonUtils;

        private static readonly string  DONT_DELETE_LIST_KEY= "DONT_DELETE_LIST_KEY";


        private static LocalDataManager INSTANCE;

        public static LocalDataManager getInstance(){
            if(INSTANCE == null){
                INSTANCE = new LocalDataManager();
            }

            return INSTANCE;
        }

        public LocalDataManager(){
            
                fileManager = new FileManager();
                preferences = new PreferenceUtils();
                mJsonUtils = new JsonUtils();
           
        }

        public T getObject<T>(string key,object defaultObj)
        {
            return preferences.getObject<T>(key, defaultObj);
        }

        public T getObject<T>(string key)
        {
            return preferences.getObject<T>(key);
        }


        public void justRemoveWarning()
        {
            GuLog.Debug(fileManager.ToString());
        }

        public void saveObject<T>(string key, T t)
        {
            preferences.saveObject<T>(key, t);
        }

        public void deleteAll(){
            deleteAll(false);
        }

        public void removeObject(string key){
            preferences.deleteObject(key);
        }

        public bool saveSpecialObject<T>(string key,T value){

            DontDeleteDataItem oldDontDeleteDataItem = null;

            try{
                string strValue = mJsonUtils.Json2String(value);
                DontDeleteDataItem item = new DontDeleteDataItem();
                item.key = key;
                item.itemInfo = strValue;

                List<string> dontDeleteList = preferences.getObject<List<string>>(DONT_DELETE_LIST_KEY);
                if(dontDeleteList == null){
                    dontDeleteList = new List<string>();
                }
                if(dontDeleteList.Contains(key)){
                    oldDontDeleteDataItem = preferences.getObject<DontDeleteDataItem>(key);
                }else{
                    dontDeleteList.Add(key);
                    preferences.saveObject(DONT_DELETE_LIST_KEY,dontDeleteList);
                }
                
                preferences.saveObject(key,item);
            }catch(Exception e){
                if(oldDontDeleteDataItem == null){
                    List<string> dontDeleteList = preferences.getObject<List<string>>(DONT_DELETE_LIST_KEY);
                    dontDeleteList.Remove(key);
                    preferences.saveObject(DONT_DELETE_LIST_KEY,dontDeleteList);
                    preferences.deleteObject(key);
                }else{
                    preferences.saveObject(key,oldDontDeleteDataItem);
                }

                return false;
                
            }

            return true;
           
        }

        public T getSpecialObject<T>(string key){

            DontDeleteDataItem item = preferences.getObject<DontDeleteDataItem>(key);

            if(item == null || string.IsNullOrEmpty(item.itemInfo)){
                return default(T);
            }

            T result = mJsonUtils.String2Json<T>(item.itemInfo);

            return result;
        }

        public void deleteAll(bool isRealDeleteAll){
            if(isRealDeleteAll){
                preferences.deleteAll();
            }else{
                List<string> dontDeleteKeyList = preferences.getObject<List<string>>(DONT_DELETE_LIST_KEY);
                if(dontDeleteKeyList == null || dontDeleteKeyList.Count == 0){
                    preferences.deleteAll();
                }else{
                    List<DontDeleteDataItem> saveDontDeleteList = new List<DontDeleteDataItem>();
                    foreach (string key in dontDeleteKeyList){
                        DontDeleteDataItem dontDeleteDataItem = preferences.getObject<DontDeleteDataItem>(key);
                        saveDontDeleteList.Add(dontDeleteDataItem);
                    }
                    preferences.deleteAll();

                    dontDeleteKeyList.Clear();

                    foreach (DontDeleteDataItem item in saveDontDeleteList)
                    {
                        preferences.saveObject(item.key,item);
                        dontDeleteKeyList.Add(item.key);
                    }

                    preferences.saveObject(DONT_DELETE_LIST_KEY,dontDeleteKeyList);
                    
                }
            }
        }

    }
}