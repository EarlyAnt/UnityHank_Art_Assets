using System.Collections.Generic;
using Gululu.LocalData.RequstBody;
using UnityEngine;

namespace Gululu.LocalData.Agents
{
    public class LocalIntakeLogsData : ILocalIntakeLogsData
    {
        
        [Inject]
        public ILocalDataManager localDataRepository{get;set;}

        [Inject]
        public LocalIntakeLogsDataUtils mLocalIntakeLogsDataUtils{get;set;}

        
        public bool addIntakeLog(string current_child_sn,string type, int vl, long time_stamp){
            
            if(current_child_sn == "" || current_child_sn == null){
                return false;
            }
            LogItem mLogItem = new LogItem(type,vl,time_stamp);

            List<LogItem> mIntakeLogList = mLocalIntakeLogsDataUtils.getLocalIntakeLogList(getSaveKey(current_child_sn));
            
            mIntakeLogList.Add(mLogItem);

            mLocalIntakeLogsDataUtils.saveIntakeLogList(getSaveKey(current_child_sn),mIntakeLogList);

            return true;
        }

        public List<LogItem> getAllIntakeLogsInfo(string current_child_sn){

            List<LogItem> mIntakeLogs = mLocalIntakeLogsDataUtils.getLocalIntakeLogList(getSaveKey(current_child_sn));

            return mIntakeLogs;
        }

        public void deleteIntakeLogsInfo(string current_child_sn){
            Debug.Log("deleteIntakeLogsInfo :"+current_child_sn);
            mLocalIntakeLogsDataUtils.deleteLocalIntakeLog(getSaveKey(current_child_sn));
        }

        private string getSaveKey(string child_sn){
            return child_sn+"_CHILD_INTAKE_LOGS";
        }




    }
}