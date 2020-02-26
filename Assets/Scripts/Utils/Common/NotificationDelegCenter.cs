/*
* NotificationDelegCenter
* 使用代理监听和派发事件
* 相对SendMessage使用delegate,提高了消息派发效率
* author : 大帅纷纭
*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


//C#在类定义外可以声明方法的签名（Delegate，代理或委托），但是不能声明真正的方法。
public delegate void OnNotificationDelegate(Notification note);

public class NotificationDelegCenter
{
	private static NotificationDelegCenter instance = null;
	
	private Dictionary<string, OnNotificationDelegate> eventListerners = new Dictionary<string, OnNotificationDelegate>();
	
	//Single 
	public static NotificationDelegCenter getInstance()
	{
		if (instance == null)
		{
			instance = new NotificationDelegCenter();
			return instance;
		}
		return instance;
	}
	
	/*
     * 监听事件
     */
	
	//添加监听事件
	public void addEventListener(string type, OnNotificationDelegate listener)
	{
		if (!eventListerners.ContainsKey(type))
		{
			OnNotificationDelegate deleg = null;
			eventListerners[type] = deleg;
		}
		eventListerners[type] += listener;
	}
	
	//移除监听事件
	public void removeEventListener(string type, OnNotificationDelegate listener)
	{
		if (!eventListerners.ContainsKey(type))
		{
			return;
		}
		eventListerners[type] -= listener;
	}
	
	//移除某一类型所有的监听事件
	public void removeEventListener(string type)
	{
		if (eventListerners.ContainsKey(type))
		{
			eventListerners.Remove(type);
		}
	}
	
	/*
     * 派发事件
     */
	
	//派发数据
	public void dispatchEvent(string type, Notification note)
	{
		if (eventListerners.ContainsKey(type))
		{
			eventListerners[type](note);
		}
	}
	
	//派发无数据
	public void dispatchEvent(string type)
	{
		dispatchEvent(type, null);
	}
	
	//查找是否有当前类型事件监听
	public Boolean hasEventListener(string type)
	{
		return eventListerners.ContainsKey(type);
	}
}