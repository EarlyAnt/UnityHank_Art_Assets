using DG.Tweening;
using Gululu;
using ModelTest;
using strange.extensions.signal.impl;
using System.Collections.Generic;
using UnityEngine;

namespace Hank.EasyTest
{
    public class EasyTestView : BaseView
    {
        /************************************************属性与变量命名************************************************/
        #region 注入接口

        #endregion
        #region 页面UI组件
        [SerializeField]
        private CanvasGroup rootObject;
        [SerializeField]
        private Navigator navigator;//功能导航(宠物，动画，换装)
        [SerializeField]
        private Navigator subNavigator;//换装导航(配饰，套装，小精灵)
        [SerializeField]
        private PetLoader petLoader;
        [SerializeField]
        private PetControl petControl;
        [SerializeField]
        private MateLoader mateLoader;
        #endregion
        #region 其他变量
        private bool show;
        private Operations operation;
        public Signal<bool> IconSelectedSignal = new Signal<bool>();
        #endregion
        /************************************************Unity方法与事件***********************************************/
        protected override void Start()
        {
            base.Start();
            this.Initialize();
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
        }
        /************************************************自 定 义 方 法************************************************/
        //初始化
        private void Initialize()
        {
            this.navigator.Initialize();
            this.subNavigator.Initialize();
            this.navigator.ButtonChanged += this.OnModuleChanged;
            this.subNavigator.ButtonChanged += this.OnDressChanged;
            Debug.Log("<><EasyTestView.Initialize>+ + + + + + + +");
        }
        //入场动画
        public void Fade(bool show)
        {
            this.show = show;
            this.rootObject.DOFade(show ? 1f : 0f, 1f);

            if (this.show)
            {
                switch (this.operation)
                {
                    case Operations.Pet:
                    case Operations.Animation:
                    case Operations.Dress:
                        this.navigator.Visible = true;
                        this.subNavigator.Visible = false;
                        break;
                    case Operations.Accessory:
                    case Operations.Suit:
                    case Operations.Sprite:
                        this.navigator.Visible = false;
                        this.subNavigator.Visible = true;
                        break;
                }
            }

            Debug.LogFormat("<><EasyTestView.Fade>Fade {0} + + + + + + + +", show ? "in" : "out");
        }
        //确认选择
        public void Select()
        {
            if (this.show)
            {
                if (this.navigator.Visible)
                {
                    this.navigator.Select();
                    Debug.Log("<><EasyTestView.Select>Select module");
                }
                else if (this.subNavigator.Visible)
                {
                    this.subNavigator.Select();
                    Debug.Log("<><EasyTestView.Select>Select dress");
                }
            }
            else
            {
                switch (this.operation)
                {
                    case Operations.Animation:
                        this.petControl.PlayAnimation();
                        break;
                    case Operations.Accessory:
                        this.petLoader.SetupAccessory(Operations.Accessory);
                        break;
                    case Operations.Suit:
                        this.petLoader.SetupAccessory(Operations.Suit);
                        break;
                    case Operations.Sprite:
                        this.mateLoader.SetMate();
                        break;
                }
            }
        }
        //前一个图标
        public void PreIcon()
        {
            if (this.show)
            {
                if (this.navigator.Visible)
                {
                    this.navigator.MovePrevious();
                    Debug.Log("<><EasyTestView.PreIcon>Previous module");
                }
                else if (this.subNavigator.Visible)
                {
                    this.subNavigator.MovePrevious();
                    Debug.Log("<><EasyTestView.PreIcon>Previous dress");
                }
            }
            else
            {
                switch (this.operation)
                {
                    case Operations.Pet:
                        this.petLoader.PreviousPet();
                        break;
                    case Operations.Animation:
                        this.petControl.PreviousAnimation();
                        break;
                    case Operations.Accessory:
                        this.petLoader.PreviousAccessory(Operations.Accessory);
                        break;
                    case Operations.Suit:
                        this.petLoader.PreviousAccessory(Operations.Suit);
                        break;
                    case Operations.Sprite:
                        this.mateLoader.PreviousMate();
                        break;
                }
            }
        }
        //后一个图标
        public void NextIcon()
        {
            if (this.show)
            {
                if (this.navigator.Visible)
                {
                    this.navigator.MoveNext();
                    Debug.LogFormat("<><EasyTestView.NextIcon>Next module, {0}", this.navigator.CurrentButton.Operation);
                }
                else if (this.subNavigator.Visible)
                {
                    this.subNavigator.MoveNext();
                    Debug.LogFormat("<><EasyTestView.NextIcon>Next dress, {0}", this.subNavigator.CurrentButton.Operation);
                }
            }
            else
            {
                switch (this.operation)
                {
                    case Operations.Pet:
                        this.petLoader.NextPet();
                        break;
                    case Operations.Animation:
                        this.petControl.NextAnimation();
                        break;
                    case Operations.Accessory:
                        this.petLoader.NextAccessory(Operations.Accessory);
                        break;
                    case Operations.Suit:
                        this.petLoader.NextAccessory(Operations.Suit);
                        break;
                    case Operations.Sprite:
                        this.mateLoader.NextMate();
                        break;
                }
            }
        }
        //当功能改变时
        private void OnModuleChanged(FlashButton button)
        {
            if (button == null) return;
            Debug.LogFormat("<><EasyTestView.OnModuleChanged>Current module{0}", button.Operation);

            this.navigator.Visible = false;
            this.operation = button.Operation;
            switch (button.Operation)
            {
                case Operations.Pet:
                case Operations.Animation:
                    this.Fade(false);
                    break;
                case Operations.Dress:
                    this.subNavigator.Visible = true;
                    break;
            }
        }
        //当装扮改变时
        private void OnDressChanged(FlashButton button)
        {
            if (button == null) return;
            Debug.LogFormat("<><EasyTestView.OnDressChanged>Current dress: {0}", button.Operation);

            this.operation = button.Operation;
            switch (button.Operation)
            {
                case Operations.Accessory:

                    break;
                case Operations.Suit:

                    break;
                case Operations.Sprite:

                    break;
            }
            this.subNavigator.Visible = false;
            this.Fade(false);
        }

        [System.Serializable]
        public class Navigator
        {
            [SerializeField]
            private GameObject rootObject;
            [SerializeField]
            private List<FlashButton> buttons;
            private int selectedIndex;
            public FlashButton CurrentButton
            {
                get
                {
                    return this.buttons[this.selectedIndex];
                }
            }
            public bool Visible
            {
                get
                {
                    return this.rootObject.activeSelf;
                }
                set
                {
                    this.rootObject.SetActive(value);
                }
            }
            public System.Action<FlashButton> ButtonChanged;
            //初始化
            public void Initialize()
            {
                this.Visible = false;
                this.selectedIndex = 0;
                this.buttons[0].SetStatus(true);
            }
            //上一个图标
            public void MovePrevious()
            {
                if (this.selectedIndex > 0)
                    this.selectedIndex -= 1;

                this.buttons.ForEach(t => t.SetStatus(t == this.CurrentButton));
            }
            //下一个图标
            public void MoveNext()
            {
                if (this.selectedIndex + 1 < this.buttons.Count)
                    this.selectedIndex += 1;

                this.buttons.ForEach(t => t.SetStatus(t == this.CurrentButton));
            }
            //选择图标
            public void Select()
            {
                this.OnButtonChanged();
            }
            //当切换图标时
            private void OnButtonChanged()
            {
                if (this.ButtonChanged != null)
                    this.ButtonChanged(this.CurrentButton);
            }
        }
    }
}
