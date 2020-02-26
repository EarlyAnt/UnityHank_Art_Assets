using Gululu.Util;
using strange.extensions.signal.impl;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TopViewHelper : SingletonBase<TopViewHelper>
{
    /************************************************属性与变量命名************************************************/
    #region 变量与属性命名
    public enum ETopView
    {
        eNone = 0,
        eMainView = 1,
        eHome = 2,
        eMissionBrower = 3,
        eMainMenu = 4,
        eLowBattary = 11,//以LowBattery为分界线，数值小的为默认页面，数值大的为各功能页面
        eInCharge = 12,
        eLanguageSetting = 21,
        eNoviceGuide = 22,
        eSystemMenuGuide = 23,
        eSystemMenu = 24,
        eCupPair = 25,
        eCollection = 26,
        eAnimalPlayer = 27,
        ePetPage = 28,
        eCommingSoon = 29,
        eTmall = 30,
        eNewsEvent = 31,
        eAlarmClock = 32,
        eAlertBoard = 33,
        eAchievement = 34,
        eFriend = 35,
        eDownloadPage = 36,
        eDownloadPage2 = 37,
        ePetFeed = 38,
        eWorldMap = 39,
        eGuideTip = 40,
        ePetDress = 41,
        eEasyTest = 42
    }
    public ETopView TopView
    {
        get
        {
            if (this.topViewList == null || this.topViewList.Count == 0)
                return ETopView.eNone;
            return this.topViewList[this.topViewList.Count - 1];
        }
    }
    public const string ADD_VIEW = "AddView";
    public const string REMOVE_VIEW = "RemoveView";
    public const string RESET_VIEW = "ResetView";
    private List<ETopView> topViewList = new List<ETopView>();//普通页面集合
    private Dictionary<ETopView, bool> silenceViewList = new Dictionary<ETopView, bool>();//静默页面集合
    private Signal<string, ETopView> topViewChangedSignal = new Signal<string, ETopView>();
    #endregion
    /************************************************构  造   函  数***********************************************/
    public TopViewHelper()
    {
        this.silenceViewList.Add(ETopView.eLowBattary, false);
        this.silenceViewList.Add(ETopView.eInCharge, false);
        this.silenceViewList.Add(ETopView.eGuideTip, false);
    }
    /************************************************自 定 义 方 法************************************************/
    //添加页面栈变化监听
    public void AddListener(Action<string, ETopView> action)
    {
        this.topViewChangedSignal.AddListener(action);
    }
    //移除页面栈变化监听
    public void RemoveListener(Action<string, ETopView> action)
    {
        this.topViewChangedSignal.RemoveListener(action);
    }
    //页面入栈
    public void AddTopView(ETopView view)
    {
        if (this.silenceViewList.ContainsKey(view))
        {//静默页面状态置为打开
            this.silenceViewList[view] = true;
        }
        else if (!topViewList.Contains(view))
        {//记录普通页面
            topViewList.Add(view);
            this.OnViewChanged(ADD_VIEW, view);
        }
        GuLog.Info(this.ToString());
    }
    //页面出栈(已弃用)
    public void PopTopView()
    {
        throw new Exception("此方法已作废");
    }
    //页面出栈
    public void RemoveView(ETopView view)
    {
        if (this.silenceViewList.ContainsKey(view))
        {//静默页面状态置为关闭
            this.silenceViewList[view] = false;
        }
        else if (topViewList.Count > 0 && this.TopView != ETopView.eMainView && topViewList.Contains(view))
        {//移除普通页面
            topViewList.Remove(view);
            this.OnViewChanged(REMOVE_VIEW, this.TopView);
        }
        GuLog.Info(this.ToString());
    }
    //指定页面是否为最上层页面(时间上的后打开页面，而不是视觉上的前后层)
    public bool IsTopView(ETopView view)
    {
        if (topViewList.Count > 0)
        {
            return view == topViewList[topViewList.Count - 1];
        }
        return false;
    }
    //指定页面是否已打开
    public bool IsOpenedView(ETopView view)
    {
        //普通页面在列表中，或者静默页面为打开状态，则视为已打开
        return ((this.topViewList != null && this.topViewList.Contains(view)) ||
                (this.silenceViewList != null && this.silenceViewList.ContainsKey(view) && this.silenceViewList[view]));
    }
    //是否已返回主页面
    public bool IsBackToMainView()
    {
        List<ETopView> moduleViews = new List<ETopView>() { ETopView.eCollection, ETopView.eAnimalPlayer,
                                                            ETopView.eCommingSoon, ETopView.ePetPage, ETopView.eSystemMenu };
        if (this.topViewList == null || this.topViewList.Count == 0 ||
            !this.topViewList.Exists(t => moduleViews.Contains(t)))
            return true;
        else
            return false;
    }
    //是否在引导环节
    public bool IsInGuideView()
    {        
        return (TopViewHelper.Instance.IsTopView(TopViewHelper.ETopView.eNoviceGuide)
                || TopViewHelper.Instance.IsTopView(TopViewHelper.ETopView.eSystemMenuGuide));
    }
    //是否有低电页以上层级的模块页面打开(层级是指ETopView枚举中的位置)
    public bool HasModulePageOpened()
    {
        return this.HasModulePageOpened(ETopView.eInCharge);
    }
    //是否有指定页以上层级的模块页面打开(层级是指ETopView枚举中的位置)
    public bool HasModulePageOpened(ETopView obstructViewLevel)
    {
        if (this.topViewList != null)
        {
            int index = this.topViewList.FindIndex(t => (int)t > (int)obstructViewLevel);
            if (index >= 0)
            {
                //Debug.LogFormat("<><TopViewHelper.HasModulePageOpened>ModulePage: {0}, {1}", index, this.topViewList[index]);
                return true;
            }
            else return false;
        }
        else return false;
    }
    //是否有指定页以上层级的模块页面打开(层级是指ETopView枚举中的位置)
    public bool HasSilenceViewOpened(ETopView silenceView)
    {
        if (this.topViewList != null)
        {
            if (this.silenceViewList.ContainsKey(silenceView) && this.silenceViewList[silenceView])
            {
                //Debug.LogFormat("<><TopViewHelper.HasModulePageOpened>SilenceView: {0}, {1}", silenceView, this.silenceViewList[silenceView]);
                return true;
            }
            else return false;
        }
        else return false;
    }
    //重置页面状态栈
    public override void Reset()
    {
        this.topViewList.Clear();
        this.topViewList.Add(ETopView.eMainView);
        List<ETopView> keys = this.silenceViewList.Keys.ToList();
        for (int i = 0; i < keys.Count; i++)
            this.silenceViewList[keys[i]] = false;
        this.OnViewChanged(RESET_VIEW, this.TopView);
    }
    //输出页面栈内容
    public override string ToString()
    {
        System.Text.StringBuilder strbContent = new System.Text.StringBuilder("<><TopViewHelper.ToString>TopViewList: ");
        if (this.topViewList != null && this.topViewList.Count > 0)
        {
            for (int i = 0; i < this.topViewList.Count; i++)
            {
                strbContent.AppendFormat("[{0}, {1}], ", i + 1, this.topViewList[i]);
            }
            strbContent = strbContent.Remove(strbContent.Length - 2, 2);
            return strbContent.ToString();
        }
        else return "";
    }
    //当页面栈变化时
    private void OnViewChanged(string operate, ETopView topView)
    {
        Debug.LogFormat("<><TopViewHelper.OnViewChanged>operate: {0}, topView: {1}", operate, topView);
        this.topViewChangedSignal.Dispatch(operate, this.TopView);
    }
}
