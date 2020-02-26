using System.Collections.Generic;
using Gululu.LocalData.RequstBody;
using UnityEngine;

namespace Gululu.LocalData.Agents
{
    public class LocalIntakeLogsDataUtils
    {
        [Inject]
        public ILocalDataManager localDataRepository{get;set;}

        [Inject]
        public IJsonUtils mJsonUtils{get;set;}
        public List<LogItem> getLocalIntakeLogList(string key){
            string localIntakeLogsStr = localDataRepository.getObject<string>(key, "");

            Debug.Log("getAllIntakeLogsInfo current_child_sn:"+key+" localIntakeLogs:"+localIntakeLogsStr);

            List<LogItem> mIntakeLogList;

            if(localIntakeLogsStr == ""){
                mIntakeLogList = new List<LogItem>();
            }else{
                mIntakeLogList = mJsonUtils.String2Json<List<LogItem>>(localIntakeLogsStr);
            }

            return mIntakeLogList;
        }

        public void saveIntakeLogList(string key,List<LogItem> mIntakeLogList){
            string saveIntakeLogData = mJsonUtils.Json2String(mIntakeLogList);
            
            localDataRepository.saveObject<string>(key,saveIntakeLogData);
        }

        public void deleteLocalIntakeLog(string key){
            localDataRepository.removeObject(key);
        }
    }
}