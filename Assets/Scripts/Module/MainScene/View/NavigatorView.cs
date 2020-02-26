using DG.Tweening;
using Gululu;
using strange.extensions.signal.impl;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Hank.MainScene
{
    [ExecuteInEditMode]
    public class NavigatorView : BaseView
    {
        /************************************************属性与变量命名************************************************/
        [SerializeField]
        private CanvasGroup canvasGroup;
        [SerializeField]
        private Transform panel;
        [SerializeField]
        private Transform subPanel;
        [SerializeField]
        private List<NavigatorItem> items;
        [SerializeField, Range(0f, 1000f)]
        private float radius = 2f;
        [SerializeField, Range(0f, 2f)]
        private float rotateDuration = 0.5f;
        [SerializeField, Range(0f, 2f)]
        private float fadeDuration = 0.3f;
        [SerializeField, Range(0f, 5f)]
        private float reaminSeconds = 3f;
        [SerializeField]
        private int defaultIndex = 1;
        private int selectedIndex = 1;
        private int lastSelectedIndex = 1;
        private NavigatorItem lastNavigatorItem = null;
        private bool indexChanged { get { return this.selectedIndex != this.lastSelectedIndex; } }
        private bool viewChanged { get { return this.items[this.selectedIndex] != this.lastNavigatorItem; } }
        private bool displayed;
        private Sequence sequence;
        public Signal<TopViewHelper.ETopView> CloseLastViewSignal = new Signal<TopViewHelper.ETopView>();
        public Action<TopViewHelper.ETopView> ViewSelectedAction;
        /************************************************Unity方法与事件***********************************************/
        protected override void Awake()
        {
            this.canvasGroup.alpha = 0;
            this.panel.transform.localScale = Vector3.one * 0.9f;
        }
        protected override void Start()
        {
            base.Start();
            this.CreateItems();
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
        }
        private void LateUpdate()
        {
            this.TowardUp();
        }
        /************************************************自 定 义 方 法************************************************/
        [ContextMenu("生成轮盘图标")]
        private void CreateItems()
        {
            if (this.subPanel == null)
                return;

            while (this.subPanel.childCount > 0)
            {
                GameObject.DestroyImmediate(this.subPanel.GetChild(0).gameObject);
            }

            for (int i = 0; i < this.items.Count; i++)
            {
                float x = Mathf.Sin(this.items[i].Angle * Mathf.Deg2Rad) * this.radius;
                float y = Mathf.Cos(this.items[i].Angle * Mathf.Deg2Rad) * this.radius;

                GameObject newItem = new GameObject() { name = string.Format("{0}-{1}", i + 1, this.items[i].IconName) };
                newItem.transform.SetParent(this.subPanel);
                newItem.AddComponent<RectTransform>().localPosition = new Vector3(x, y, 0);
                newItem.GetComponent<RectTransform>().localScale = Vector3.one;
                newItem.GetComponent<RectTransform>().sizeDelta = Vector2.one * 32;
                this.items[i].normalSprite = Resources.Load<Sprite>(string.Format("Texture/MainScene/Navigator/{0}_us", this.items[i].IconName));
                this.items[i].selectedSprite = Resources.Load<Sprite>(string.Format("Texture/MainScene/Navigator/{0}_s", this.items[i].IconName));
                this.items[i].ImageBox = newItem.AddComponent<Image>();
                this.items[i].SetStatus(false);
            }
            this.TurnToHomePage();
        }
        [ContextMenu("向上划(左转)")]
        public void TurnLeft()
        {
            if (this.selectedIndex + 1 < this.items.Count)
                this.selectedIndex += 1;
            this.Turn(this.selectedIndex);
        }
        [ContextMenu("向下划(右转)")]
        public void TurnRight()
        {
            if (this.selectedIndex > 0)
                this.selectedIndex -= 1;
            this.Turn(this.selectedIndex);
        }
        //旋转至Home图标
        public void TurnToHomePage()
        {
            this.Turn(this.defaultIndex, false);
            this.ResetPet();
            this.lastNavigatorItem = this.items.Find(t => t.View == TopViewHelper.ETopView.eHome);
        }
        //旋转轮盘
        private void Turn(int selectedIndex, bool visible = true)
        {
            this.selectedIndex = selectedIndex;
            if (this.items == null)
            {
                Debug.LogError("<><NavigatorView.Turn>Parameter 'items' is null");
                return;
            }
            else if (selectedIndex < 0 || selectedIndex >= this.items.Count)
            {
                Debug.LogErrorFormat("<><NavigatorView.Turn>Parameter 'selectedIndex' is invalid, selectedIndex: {0}, items.Count: {1}", selectedIndex, this.items.Count);
                return;
            }

            if (this.sequence != null) this.sequence.Kill(true);
            this.items.ForEach(t => t.SetStatus(t == this.items[this.selectedIndex]));
            if (visible)
            {
                this.canvasGroup.DOFade(1f, this.indexChanged && !this.displayed ? this.fadeDuration : 0);
                this.panel.transform.DOScale(1f, this.indexChanged && !this.displayed ? this.fadeDuration : 0);
            }
            this.displayed = true;
            NavigatorItem navigatorItem = this.items[this.selectedIndex];
            Vector3 endAngle = new Vector3(0, 0, navigatorItem.Angle);
            this.sequence = DOTween.Sequence();
            if (visible)
            {
                if (this.indexChanged)
                {
                    sequence.Append(this.panel.DOLocalRotate(endAngle, this.rotateDuration));
                    //SoundPlayer.GetInstance().StopSound("navigator_change_icon");
                    SoundPlayer.GetInstance().PlaySoundType("navigator_change_icon");
                }
                sequence.AppendInterval(this.reaminSeconds);
                sequence.Append(this.canvasGroup.DOFade(0f, this.fadeDuration / 2));
                sequence.Append(this.panel.transform.DOScale(0.9f, this.fadeDuration / 2));
                if (this.viewChanged) sequence.AppendCallback(() => this.Dispatch(navigatorItem.View));
            }
            else
            {
                sequence.Append(this.panel.DOLocalRotate(endAngle, 0f));
            }
            sequence.AppendCallback(() => this.displayed = false);
            this.lastSelectedIndex = this.selectedIndex;
        }
        //轮盘上的图标的方向始终保持朝上
        private void TowardUp()
        {
            for (int i = 0; i < this.items.Count; i++)
            {
                if (this.items[i].ImageBox != null)
                    this.items[i].ImageBox.GetComponent<RectTransform>().eulerAngles = Vector3.zero;
            }
        }
        //重设小宠物状态
        private void ResetPet()
        {
            if (this.lastNavigatorItem != null && this.lastNavigatorItem.ResetPet && !this.items[this.selectedIndex].ResetPet)
                RoleManager.Instance.Reset(false);//检查是否需要重设小宠物状态(关闭某个功能页时重设，功能页之前切换不重设)
        }
        //发送消息
        private void Dispatch(TopViewHelper.ETopView selectedView)
        {
            if (this.lastNavigatorItem != null && TopViewHelper.Instance.IsOpenedView(this.lastNavigatorItem.View))
                this.CloseLastViewSignal.Dispatch(this.lastNavigatorItem.View);//关闭上一个功能页面

            this.ResetPet();

            if (this.ViewSelectedAction != null)
                this.ViewSelectedAction(selectedView);//打开新的功能页或关闭所有功能页(目前仅指喂食页和换装页)

            if (this.items[this.selectedIndex].View != TopViewHelper.ETopView.eHome)
                this.lastNavigatorItem = this.items[this.selectedIndex];
            else
                this.lastNavigatorItem = null;
            Debug.LogFormat("<><NavigatorView.Dispatch>SelectedView: {0}", selectedView);
        }
    }

    [Serializable]
    public class NavigatorItem
    {
        public int Angle;
        public string IconName;
        public TopViewHelper.ETopView View;
        public bool ResetPet;
        public Image ImageBox;
        public Sprite normalSprite;
        public Sprite selectedSprite;

        public void SetStatus(bool selected)
        {
            if (this.ImageBox != null)
            {
                this.ImageBox.sprite = selected ? this.selectedSprite : this.normalSprite;
                this.ImageBox.GetComponent<RectTransform>().localScale = Vector3.one * (selected ? 1.3f : 1f);
            }
        }
    }
}