using UnityEngine;
using System.Collections.Generic;
using System;
using Gululu;
using Gululu.Util;
using Cup.Utils.android;
using Gululu.Events;

public class FlurryUtil : SingletonBase<FlurryUtil>
{
    private static string LogFirstActiveEvent = "LogFirstActiveEvent";
    private static string active_new = "active_new";

    private static IFlurryUtils mFlurryUtils;

    private void LogFirstActiveView()
    {
        // LocalDataManager.getInstance().saveObject(LogFirstActiveEvent, "LogFirstActiveEvent");
    }

    public void checkoutNeedLogFirstActiveViewEvent()
    {
        // string firstActive = LocalDataManager.getInstance().getObject<string>(LogFirstActiveEvent);
        // if(firstActive == null){
        //     UnityToNative.LogFlurryEvent(active_new);
        //     LogFirstActiveView();
        // }
    }

    public static void Init()
    {
        if (mFlurryUtils == null)
        {
            mFlurryUtils = new Cup.Utils.android.FlurryUtils();
        }

        mFlurryUtils.init();
    }

    public static void LogEvent(string eventStr)
    {
        if (mFlurryUtils == null)
        {
            mFlurryUtils = new Cup.Utils.android.FlurryUtils();
        }

        mFlurryUtils.onEvent(eventStr);
    }

    public static void SetUserId(string userid)
    {
        if (mFlurryUtils == null)
        {
            mFlurryUtils = new Cup.Utils.android.FlurryUtils();
        }

        mFlurryUtils.setUserId(userid);
    }

    public static void LogTimeEvent(string eventStr)
    {
        if (mFlurryUtils == null)
        {
            mFlurryUtils = new Cup.Utils.android.FlurryUtils();
        }

        mFlurryUtils.logTimeEvent(eventStr, eventStr);
    }

    public static void EndTimedEvent(string eventStr)
    {
        if (mFlurryUtils == null)
        {
            mFlurryUtils = new Cup.Utils.android.FlurryUtils();
        }

        mFlurryUtils.endTimedEvent(eventStr);
    }

    public static void LogTimeEvent(TopViewHelper.ETopView view)
    {
        if (mFlurryUtils == null)
        {
            mFlurryUtils = new Cup.Utils.android.FlurryUtils();
        }

        mFlurryUtils.logTimeEvent(view.ToString(), view.ToString());
    }

    public static void EndTimedEvent(TopViewHelper.ETopView view)
    {
        if (mFlurryUtils == null)
        {
            mFlurryUtils = new Cup.Utils.android.FlurryUtils();
        }

        mFlurryUtils.endTimedEvent(view.ToString());
    }

    public static void LogEvent(TopViewHelper.ETopView view, MainKeys key)
    {
        if (mFlurryUtils == null)
        {
            mFlurryUtils = new Cup.Utils.android.FlurryUtils();
        }

        mFlurryUtils.onEvent(string.Format("{0}_{1}", view, key));
    }

    public static void LogEventWithParam(string eventId, string paramKey, string paramValue)
    {
        if (mFlurryUtils == null)
        {
            mFlurryUtils = new Cup.Utils.android.FlurryUtils();
        }
        mFlurryUtils.onEvent(eventId, paramKey, paramValue);
    }
}
