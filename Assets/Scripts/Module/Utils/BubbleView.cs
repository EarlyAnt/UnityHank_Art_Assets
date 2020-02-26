using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 导航按钮布局组件
/// </summary>
public class BubbleView : MonoBehaviour
{
    /************************************************属性与变量命名************************************************/
    [SerializeField]
    private bool autoMove = false;
    [SerializeField]
    private int defaultIndex;//默认序号
    [SerializeField]
    private Vector2 itemSize;//子物体大小
    [SerializeField]
    private float spacing;//子物体之间间隔
    [SerializeField]
    private Vector2 deltaSize;//选中态放大的增量
    [SerializeField]
    private float duration = 0.5f;//按钮切换时动效的时长
    [SerializeField]
    private List<RectTransform> uiList;//按钮UI集合
    [SerializeField]
    private GameObject leftBorder;//左边界
    [SerializeField]
    private GameObject rightBorder;//有边界
    [SerializeField]
    private int arrowSpaceCount = 2;//箭头边界空位数(左右两端各还有多少个icon时显示或隐藏箭头)
    private List<Rect> originRect;//按钮初始位置和大小
    private List<Vector3> dockList;//子物体槽位
    private bool initialized;//已初始化
    public int SelectedIndex { get; private set; }//选中按钮的序号
    public RectTransform CurrentIcon
    {
        get
        {
            if (this.uiList != null && this.uiList.Count > this.SelectedIndex)
                return this.uiList[this.SelectedIndex];
            else
                return null;
        }
    }
    public Action<RectTransform> OnItemChange { get; set; }
    /************************************************Unity方法与事件***********************************************/
    private void Start()
    {
        if (this.autoMove)
            this.Select(this.defaultIndex);
    }
    private void Update()
    {
        //#if UNITY_EDITOR
        //        if (Input.GetKeyDown(KeyCode.LeftArrow))
        //        {
        //            this.MovePrevious();
        //        }
        //        else if (Input.GetKeyDown(KeyCode.RightArrow))
        //        {
        //            this.MoveNext();
        //        }
        //        else if (Input.GetKeyDown(KeyCode.R))
        //        {
        //            this.Select(this.defaultIndex);
        //        }
        //#endif
    }
    /************************************************自 定 义 方 法************************************************/
    //初始化
    private void Initialize()
    {
        if (!this.initialized || this.originRect != null && this.uiList != null && this.originRect.Count != this.uiList.Count)
        {//记录所有按钮的初始位置和大小
            this.originRect = new List<Rect>();
            for (int i = 0; i < this.uiList.Count; i++)
            {
                this.originRect.Add(new Rect()
                {
                    position = this.uiList[i].anchoredPosition3D,
                    size = this.uiList[i].sizeDelta
                });
            }
            this.initialized = true;
        }
    }
    //增加图标
    public void AddChild(RectTransform uiItem)
    {
        uiList.Add(uiItem);
    }
    //清除所有子物体
    public void ClearAllChild()
    {
        foreach (var item in uiList.ToArray())
        {
            uiList.Remove(item);
            GameObject.Destroy(item.gameObject);
        }
    }
    //更新子物体的位置
    public void UpdateItemPosition()
    {
        this.SetChildSize();
        this.CreateDockList();
        this.SetChildPosition();
    }
    [ContextMenu("1-设置子物体大小")]
    private void SetChildSize()
    {
        for (int i = 0; i < this.uiList.Count; i++)
        {
            this.uiList[i].sizeDelta = this.itemSize;
        }
    }
    [ContextMenu("2-计算产生按钮槽位")]
    private void CreateDockList()
    {
        List<Vector3> dockList = new List<Vector3>();
        for (int i = 0; i < this.uiList.Count; i++)
        {
            dockList.Add(new Vector3((this.itemSize.x + this.spacing) * i, 0, 0));
        }
        this.dockList = new List<Vector3>();
        this.dockList.AddRange(dockList);
    }
    [ContextMenu("3-设置子物体位置")]
    private void SetChildPosition()
    {
        for (int i = 0; i < this.uiList.Count; i++)
        {
            this.uiList[i].anchoredPosition3D = this.dockList[i];
        }
    }
    [ContextMenu("4-按默认位置预览效果")]
    private void Preview()
    {
        this.Select(this.defaultIndex);
    }
    //选择按钮
    public void Select(int index, bool immediately = false)
    {
        this.Initialize();
        this.SelectedIndex = index;
        for (int i = 0; i < this.uiList.Count; i++)
        {
            if (!immediately)
                this.uiList[i].DOSizeDelta(i == index ? this.originRect[i].size + this.deltaSize / 2 : this.originRect[i].size, this.duration);
            else
                this.uiList[i].sizeDelta = i == index ? this.originRect[i].size + this.deltaSize / 2 : this.originRect[i].size;
        }
        if (this.leftBorder != null) this.leftBorder.SetActive(index >= this.arrowSpaceCount);
        if (this.rightBorder != null) this.rightBorder.SetActive(index + this.arrowSpaceCount < this.uiList.Count);
        this.MoveView(immediately);
    }
    //上一个宠物
    public void MovePrevious()
    {
        if (this.SelectedIndex > 0)
        {
            this.Select(this.SelectedIndex - 1);
        }
    }
    //下一个宠物
    public void MoveNext()
    {
        if (this.SelectedIndex + 1 < this.uiList.Count)
        {
            this.Select(this.SelectedIndex + 1);
        }
    }
    //设置子物体位置
    private void MoveView(bool immediately = false)
    {
        if (immediately)
        {
            Vector3 newPosition = this.transform.localPosition;
            newPosition.x = this.SelectedIndex * -(this.itemSize.x + this.spacing);
            this.transform.localPosition = newPosition;
            this.OnItemChanged();
        }
        else
        {
            this.transform.DOLocalMoveX(this.SelectedIndex * -(this.itemSize.x + this.spacing), this.duration).onComplete = () =>
            {
                this.OnItemChanged();
            };
        }
    }
    //当切换图标时
    private void OnItemChanged()
    {
        if (this.OnItemChange != null)
            this.OnItemChange.Invoke(this.CurrentIcon);
    }
}
