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
    public enum ETaskType
    {
        Type_Main = 1, // 主线任务
        Type_Mission = 2, // 关卡任务
        Type_Sub = 3, // 剧情支线任务
        Type_DailyGoal = 4, // 每日饮水任务

    }
    public interface ITaskConfig
    {

        TaskSetting GetTaskById(int id);

        void LoadTaskData();

        bool IsLoadedOK();
    }
}
