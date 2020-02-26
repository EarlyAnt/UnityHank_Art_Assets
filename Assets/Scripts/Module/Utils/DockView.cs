using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 导航按钮布局组件
/// </summary>
public class DockView : MonoBehaviour
{
    /************************************************属性与变量命名************************************************/
    [SerializeField]
    private int defaultIndex;//默认序号
    [SerializeField]
    private Vector2 itemSize;//子物体大小
    [SerializeField]
    private float spacing;//子物体之间间隔
    [SerializeField]
    private Vector2 deltaSize;//选中态放大的增量
    [SerializeField]
    private float duration = 0.2f;//按钮切换时动效的时长
    [SerializeField]
    private List<RectTransform> uiList;//按钮UI集合
    private List<Rect> originRect;//按钮初始位置和大小
    private List<Vector3> dockList;//子物体槽位
    private int selectedIndex;//选中按钮的序号
    private bool initialized;//已初始化
    /************************************************Unity方法与事件***********************************************/
    private void Start()
    {
    }
    private void Update()
    {
        //#if UNITY_EDITOR
        //        if (Input.GetKeyDown(KeyCode.Alpha1))
        //        {
        //            this.Select(0);
        //        }
        //        else if (Input.GetKeyDown(KeyCode.Alpha2))
        //        {
        //            this.Select(1);
        //        }
        //        else if (Input.GetKeyDown(KeyCode.Alpha3))
        //        {
        //            this.Select(2);
        //        }
        //        else if (Input.GetKeyDown(KeyCode.Alpha4))
        //        {
        //            this.Select(3);
        //        }
        //#endif
    }
    private void LateUpdate()
    {
        if (!this.initialized) return;

        for (int i = 0; i < this.uiList.Count; i++)
        {
            if (i < this.selectedIndex)
            {//所有当前按钮上方的图标，位置向上移
                this.uiList[i].DOAnchorPos3D(this.originRect[i].position + new Vector2(0, this.deltaSize.y / 4), this.duration);
            }
            else if (i == this.selectedIndex)
            {//当前按钮恢复初始位置
                this.uiList[i].anchoredPosition3D = this.originRect[i].position;
            }
            else if (i > this.selectedIndex)
            {//所有当前按钮下方的图标，位置向下移
                this.uiList[i].DOAnchorPos3D(this.originRect[i].position - new Vector2(0, this.deltaSize.y / 4), this.duration);
            }
        }
    }
    /************************************************自 定 义 方 法************************************************/
    //初始化
    private void Initialize()
    {
        if (!this.initialized)
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
        this.initialized = false;
        this.Initialize();
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
        this.dockList = new List<Vector3>();
        float top = ((this.itemSize.y + this.spacing) * (this.uiList.Count - 1)) / 2;
        for (int i = 0; i < this.uiList.Count; i++)
        {
            this.dockList.Add(new Vector3(0, top - (this.itemSize.y + this.spacing) * i, 0));
        }
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
    public void Select(int index)
    {
        this.Initialize();
        this.selectedIndex = index;
        for (int i = 0; i < this.uiList.Count; i++)
        {
            this.uiList[i].DOSizeDelta(i == index ? this.originRect[i].size + this.deltaSize / 2 : this.originRect[i].size, this.duration);
        }
    }
}
