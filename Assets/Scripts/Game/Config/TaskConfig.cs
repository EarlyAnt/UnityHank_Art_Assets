using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using System.Security;
using Hank.Data;
using Gululu.Util;

namespace Gululu.Config
{
    public class TaskConfig : AbsConfigBase, ITaskConfig
    {

        public Dictionary<int, TaskSetting> m_TaskMap = new Dictionary<int, TaskSetting>();

        public TaskSetting GetTaskById(int id)
        {
            TaskSetting xTask = null;
            bool bResult = m_TaskMap.TryGetValue(id, out xTask);
            if (!bResult)
            { return null; }

            return xTask;
        }

        public void LoadTaskData()
        {
            if (m_LoadOk)
                return;
            ConfigLoader.Instance.LoadConfig("Tasks.bytes", Parser);
//             StartCoroutine(_LoadTaskData());
        }


        private void Parser(WWW www)
        {
            m_LoadOk = true;
            if (www.isDone && (www.error == null || www.error.Length == 0))
            {
                MemoryStream dataStream = new MemoryStream(www.bytes);
                dataStream.Seek(4, SeekOrigin.Begin);

                TaskSettingStorage monsterStorage = TaskSettingStorage.Parser.ParseFrom(dataStream);

                m_TaskMap.Clear();
                Google.Protobuf.Collections.RepeatedField<TaskSetting> Tasks = monsterStorage.Tasksettings;
                foreach (TaskSetting monster in Tasks)
                {
                    TaskSetting outTask;
                    if (!m_TaskMap.TryGetValue((int)monster.TaskId, out outTask))
                    {
                        m_TaskMap.Add((int)monster.TaskId, monster);
                    }
                }
            }
        }

//         IEnumerator _LoadTaskData()
//         {
//             string sPath = ResourceLoadBase.GetResourceBasePath() + "Tasks.bytes";
//             WWW www = new WWW(sPath);
//             yield return www;
// 
//             Parser(www);
//         }
    }
}
