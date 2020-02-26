using Gululu;
using Gululu.Events;
using strange.extensions.dispatcher.eventdispatcher.api;
using System.Collections;
using UnityEngine;

namespace Hank.PetDress
{
    public class PetDressMediator : BaseMediator
    {
        /************************************************属性与变量命名************************************************/
        [Inject]
        public PetDressView PetDressView { get; set; }
        private Coroutine changeAccessoryCoroutine;//切换宠物协程(保持左倾或右倾时连续切换宠物图标)
        /************************************************Unity方法与事件***********************************************/

        /************************************************自 定 义 方 法************************************************/
        //注册输入监听
        public override void OnRegister()
        {
            this.View = TopViewHelper.ETopView.ePetDress;
            this.PetDressView.CloseViewSignal.AddListener(this.OnClose);
            base.OnRegister();
        }
        //取消注册输入监听
        public override void OnRemove()
        {
            this.PetDressView.CloseViewSignal.RemoveListener(this.OnClose);
            base.OnRemove();
        }
        //单击电源键
        protected override void PowerClick()
        {
            if (this.changeAccessoryCoroutine != null)
                this.StopCoroutine(this.changeAccessoryCoroutine);

            this.PetDressView.Confirm();
        }
        //摇晃杯子
        protected override void CupShake()
        {
            if (this.changeAccessoryCoroutine != null)
                this.StopCoroutine(this.changeAccessoryCoroutine);

            this.OnClose(true);
        }
        //杯子左倾
        protected override void CupTiltLeft()
        {
            if (this.changeAccessoryCoroutine != null)
                this.StopCoroutine(this.changeAccessoryCoroutine);
            this.changeAccessoryCoroutine = this.StartCoroutine(this.ContinueMove(true));
        }
        //杯子右倾
        protected override void CupTiltRight()
        {
            if (this.changeAccessoryCoroutine != null)
                this.StopCoroutine(this.changeAccessoryCoroutine);
            this.changeAccessoryCoroutine = this.StartCoroutine(this.ContinueMove(false));
        }
        //杯子放平
        protected override void CupKeepFlat()
        {
            if (this.changeAccessoryCoroutine != null)
                this.StopCoroutine(this.changeAccessoryCoroutine);
        }
        //连续切换宠物图标
        private IEnumerator ContinueMove(bool left)
        {
            while (true)
            {
                if (left)
                    this.PetDressView.PreAccessory();
                else
                    this.PetDressView.NextAccessory();
                yield return new WaitForSeconds(0.7f);
            }
        }
        //杯子左侧上划
        protected override void CupLeftSwipeUp()
        {
            if (this.changeAccessoryCoroutine != null)
                this.StopCoroutine(this.changeAccessoryCoroutine);
            this.PetDressView.PreAccessoryType();
        }
        //杯子左侧下划
        protected override void CupLeftSwipeDown()
        {
            if (this.changeAccessoryCoroutine != null)
                this.StopCoroutine(this.changeAccessoryCoroutine);
            this.PetDressView.NextAccessoryType();
        }
        //杯子右侧上划
        protected override void CupRightSwipeUp()
        {
            if (this.changeAccessoryCoroutine != null)
                this.StopCoroutine(this.changeAccessoryCoroutine);
            this.dispatcher.Dispatch(MainEvent.ShowNavigator, MainEvent.NavigatorUp);
        }
        //杯子右侧下划
        protected override void CupRightSwipeDown()
        {
            if (this.changeAccessoryCoroutine != null)
                this.StopCoroutine(this.changeAccessoryCoroutine);
            this.dispatcher.Dispatch(MainEvent.ShowNavigator, MainEvent.NavigatorDown);
        }
        //当关闭页面时
        protected override void OnClose(bool initiative)
        {
            this.PetDressView.SetViewSnapShot();
            dispatcher.Dispatch(MainEvent.ClosePetDress, initiative);
        }
    }
}