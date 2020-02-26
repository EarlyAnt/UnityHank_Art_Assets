using DG.Tweening;
using Gululu;
using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ����ʽҳ�����
/// </summary>
public abstract class PopBaseView : BaseView
{
    /************************************************�������������************************************************/
    [SerializeField]
    private Image mask;//����
    [SerializeField]
    private float maskDuration = 0.5f;//���ֶ�Чʱ��
    [SerializeField]
    protected ViewResult defaultViewResult = ViewResult.OK;//Ĭ��״̬(Ĭ�ϰ�ť)
    protected ViewResult viewResult = ViewResult.OK;//��ǰ״̬(��ǰ��ť)
    public bool IsOpen { get { return this.gameObject.activeInHierarchy; } }//ҳ���Ƿ��Ѵ�
    public Action<ViewResult> ViewClosed;//ҳ��ر�ʱ��ί��
    public Action<ViewResult, object> ViewClosedWithParam;//ҳ��ر�ʱ�������ݵ�ί��
    /************************************************Unity�������¼�***********************************************/

    /************************************************�� �� �� �� ��************************************************/
    //��ҳ��
    public virtual void Open(object param = null)
    {
        this.gameObject.SetActive(true);
        //this.mask.DOFade(0.8f, this.maskDuration);
    }
    //�ر�ҳ��
    public override void Close()
    {
        this.gameObject.SetActive(false);
    }
    //�ر�ҳ��
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
    //ȷ�Ϸ���
    public virtual void OK(System.Action callback = null)
    {
        this.viewResult = ViewResult.OK;
        this.Close(callback);
    }
    //ȡ������
    public virtual void Cancel(System.Action callback = null)
    {
        this.viewResult = ViewResult.Cancel;
        this.Close(callback);
    }
    //��ҳ��ر�ʱ
    protected virtual void OnViewClosed(ViewResult viewResult)
    {
        if (this.ViewClosed != null)
        {
            this.ViewClosed(viewResult);
        }
    }
    //��ҳ��ر�ʱ
    protected virtual void OnViewClosed(ViewResult viewResult, object data)
    {
        if (this.ViewClosedWithParam != null)
        {
            this.ViewClosedWithParam(viewResult, data);
        }
    }
}

/// <summary>
/// ҳ�淵��ֵ
/// </summary>
public enum ViewResult : int
{
    OK = 0,
    Cancel = 1
}