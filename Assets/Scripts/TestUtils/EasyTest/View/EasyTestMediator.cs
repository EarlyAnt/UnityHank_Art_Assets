using Gululu;
using System.Collections;
using UnityEngine;

namespace Hank.EasyTest
{
    public class EasyTestMediator : BaseMediator
    {
        /************************************************属性与变量命名************************************************/
        [Inject]
        public EasyTestView EasyTestView { get; set; }
        private Coroutine changeIconCoroutine;//切换图标协程
        /************************************************Unity方法与事件***********************************************/

        /************************************************自 定 义 方 法************************************************/
        //注册输入监听
        public override void OnRegister()
        {
            this.View = TopViewHelper.ETopView.eEasyTest;
            base.OnRegister();
        }
        //取消注册输入监听
        public override void OnRemove()
        {
            base.OnRemove();
        }
        //单击电源键
        protected override void PowerClick()
        {
            if (this.changeIconCoroutine != null)
                this.StopCoroutine(this.changeIconCoroutine);

            this.EasyTestView.Select();
        }
        //长按电源键
        protected override void PowerHold()
        {
            this.EasyTestView.Fade(true);
        }
        //摇晃杯子
        protected override void CupShake()
        {
            if (this.changeIconCoroutine != null)
                this.StopCoroutine(this.changeIconCoroutine);

            this.EasyTestView.Fade(false);
        }
        //杯子左倾
        protected override void CupTiltLeft()
        {
            if (this.changeIconCoroutine != null)
                this.StopCoroutine(this.changeIconCoroutine);
            this.changeIconCoroutine = this.StartCoroutine(this.ContinueMove(true));
        }
        //杯子右倾
        protected override void CupTiltRight()
        {
            if (this.changeIconCoroutine != null)
                this.StopCoroutine(this.changeIconCoroutine);
            this.changeIconCoroutine = this.StartCoroutine(this.ContinueMove(false));
        }
        //杯子放平
        protected override void CupKeepFlat()
        {
            if (this.changeIconCoroutine != null)
                this.StopCoroutine(this.changeIconCoroutine);
        }
        //连续切换宠物图标
        private IEnumerator ContinueMove(bool left)
        {
            while (true)
            {
                if (left)
                    this.EasyTestView.PreIcon();
                else
                    this.EasyTestView.NextIcon();
                yield return new WaitForSeconds(0.7f);
            }
        }
    }
}