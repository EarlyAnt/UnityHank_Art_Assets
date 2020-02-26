using DG.Tweening;
using Gululu;
using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 弹出式页面基类
/// </summary>
public abstract class PopBaseView : BaseView
{
    /************************************************属性与变量命名************************************************/
    [SerializeField]
    private Image mask;//遮罩
    [SerializeField]
    private float maskDuration = 0.5f;//遮罩动效时长
    [SerializeField]
    protected ViewResult defaultViewResult = ViewResult.OK;//默认状态(默认按钮)
    protected ViewResult viewResult = ViewResult.OK;//当前状态(当前按钮)
    public bool IsOpen { get { return this.gameObject.activeInHierarchy; } }//页面是否已打开
    public Action<ViewResult> ViewClosed;//页面关闭时的委托
    public Action<ViewResult, object> ViewClosedWithParam;//页面关闭时返回数据的委托
    /************************************************Unity方法与事件***********************************************/

    /************************************************自 定 义 方 法************************************************/
    //打开页面
    public virtual void Open(object param = null)
    {
        this.gameObject.SetActive(true);
        //this.mask.DOFade(0.8f, this.maskDuration);
    }
    //关闭页面
    public override void Close()
    {
        this.gameObject.SetActive(false);
    }
    //关闭页面
    public override void Close(System.Action callback)
    {
        try
        {
            if (callback != null)
                callback();
        }
        catch (Exception ex)
        {
            Debug.LogErrorFormat("<><PopBaseView.Close>Exception: {0}", ex.Message);
        }
        this.gameObject.SetActive(false);
    }
    //确认返回
    public virtual void OK(System.Action callback = null)
    {
        this.viewResult = ViewResult.OK;
        this.Close(callback);
    }
    //取消返回
    public virtual void Cancel(System.Action callback = null)
    {
        this.viewResult = ViewResult.Cancel;
        this.Close(callback);
    }
    //当页面关闭时
    protected virtual void OnViewClosed(ViewResult viewResult)
    {
        if (this.ViewClosed != null)
        {
            this.ViewClosed(viewResult);
        }
    }
    //当页面关闭时
    protected virtual void OnViewClosed(ViewResult viewResult, object data)
    {
        if (this.ViewClosedWithParam != null)
        {
            this.ViewClosedWithParam(viewResult, data);
        }
    }
}

/// <summary>
/// 页面返回值
/// </summary>
public enum ViewResult : int
{
    OK = 0,
    Cancel = 1
}