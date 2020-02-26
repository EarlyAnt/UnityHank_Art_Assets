using System.Collections.Generic;
using Gululu.LocalData.RequstBody;

namespace Gululu.LocalData.Agents
{
    public interface ILocalIntakeLogsData
    {
        ILocalDataManager localDataRepository{get;set;}
        bool addIntakeLog(string current_child_sn, string type, int vl, long time_stamp);
        List<LogItem> getAllIntakeLogsInfo(string current_child_sn);

        void deleteIntakeLogsInfo(string current_child_sn);
    }
}