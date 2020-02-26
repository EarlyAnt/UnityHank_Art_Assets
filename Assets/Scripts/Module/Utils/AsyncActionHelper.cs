﻿using Gululu.Util;
using System;
using System.Collections.Generic;

/// <summary>
/// 异步方法回主线程工具
/// </summary>
public class AsyncActionHelper : SingletonMono<AsyncActionHelper>
{
    /************************************************属性与变量命名************************************************/
    private Queue<System.Action> actions = new Queue<System.Action>();
    /************************************************Unity方法与事件***********************************************/
    private void Awake()
    {
        UnityEngine.Object.DontDestroyOnLoad(this.gameObject);
    }
    private void Update()
    {
        while (this.actions.Count > 0)
        {
            try
            {
                this.actions.Dequeue()();
            }
            catch (Exception ex)
            {
                GuLog.Error(string.Format("执行回主线程的异步action发生错误：{0}", ex.Message));
            }
        }
    }
    /************************************************自 定 义 方 法************************************************/
    //添加需要回主线程执行的方法
    public void QueueOnMainThread(System.Action action)
    {
        if (action != null)
            this.actions.Enqueue(action);
    }
}
