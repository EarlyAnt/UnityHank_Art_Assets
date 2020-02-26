using DG.Tweening;
using Gululu;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Hank.MainMenu
{
    public class MainMenuView : BaseView
    {
        /************************************************属性与变量命名************************************************/
        public enum Tasks : byte { Achievement, Cup, Home, Collection, Friend, Pet }
        [SerializeField]
        private GuideSpineView guideSpine;
        [SerializeField]
        private GameObject upBoard;
        [SerializeField]
        private GameObject downBoard;
        [SerializeField]
        private Image mask;
        [SerializeField]
        private BubbleView bubbleView;
        [SerializeField]
        private List<MainMenuButton> buttons;
        public Tasks CurrentTask
        {
            get
            {
                return (Tasks)this.bubbleView.SelectedIndex;
            }
        }
        private Vector3 menuContentDefaultPos;
        private RectTransform menuContent;
        private float duration = 0.3f;
        private static int lastOpenedIndex = 2;//上一次打开的按钮的序号
        /************************************************Unity方法与事件***********************************************/
        protected override void Start()
        {
            base.Start();
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
        }
        /************************************************自 定 义 方 法************************************************/
        public override void Open()
        {
            this.upBoard.transform.DOLocalMoveY(190, this.duration).From();
            this.downBoard.transform.DOLocalMoveY(-190, this.duration).From();
            this.mask.DOFade(0f, this.duration).From();
            this.CheckTopView();
        }
        public override void Close(System.Action callback)
        {
            //Debug.Log("----MainMenuMediator-->>--CloseMainMenu----");
            this.HideGuide();
            this.StartCoroutine(this.DelayClose(callback));
        }
        private IEnumerator DelayClose(System.Action callback)
        {
            this.upBoard.transform.DOLocalMoveY(190, this.duration);
            this.downBoard.transform.DOLocalMoveY(-190, this.duration);
            this.mask.DOFade(0f, this.duration);
            yield return new WaitForSeconds(1f);
            base.Close(callback);
        }
        public void MovePreviousItem()
        {
            this.bubbleView.MovePrevious();
            this.SetButtonStatus();
        }
        public void MoveNextItem()
        {
            this.bubbleView.MoveNext();
            this.SetButtonStatus();
        }
        public void SetOpenModuleIndex()
        {
            lastOpenedIndex = this.bubbleView.SelectedIndex;
        }
        private void SetButtonStatus()
        {
            for (int i = 0; i < this.buttons.Count; i++)
            {
                this.buttons[i].SetStatus(i == this.bubbleView.SelectedIndex);
            }
        }
        private void CheckTopView()
        {
            if (TopViewHelper.Instance.IsBackToMainView())
            {//当打开MainMenu时，如果已经返回根页面，则两个标记都置为2（Home图标位置）
                lastOpenedIndex = 2;
            }
            Debug.LogFormat("--------MainMenuMediator received a message, LastIndex: {0}, LastOpenIndex: {1}, TopView: {2}--------\n{3}",
                            this.bubbleView.SelectedIndex, lastOpenedIndex, TopViewHelper.Instance.TopView, TopViewHelper.Instance.ToString());
            this.bubbleView.Select(lastOpenedIndex, true);
            this.SetButtonStatus();
        }
        public void ShowGuide()
        {
            this.guideSpine.Play("shake_cup");
        }
        public void HideGuide()
        {
            this.guideSpine.Stop();   
        }
    }
}

[Serializable]
internal class MainMenuButton
{
    [SerializeField]
    private GameObject staticItem;
    [SerializeField]
    private GameObject dynamicItem;
    //设置按钮状态
    public void SetStatus(bool selected)
    {
        this.staticItem.SetActive(!selected);
        this.dynamicItem.SetActive(selected);
    }
}