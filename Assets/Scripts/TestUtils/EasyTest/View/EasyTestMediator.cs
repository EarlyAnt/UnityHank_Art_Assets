using Gululu;
using System.Collections;
using UnityEngine;

namespace Hank.EasyTest
{
    public class EasyTestMediator : BaseMediator
    {
        /************************************************�������������************************************************/
        [Inject]
        public EasyTestView EasyTestView { get; set; }
        private Coroutine changeIconCoroutine;//�л�ͼ��Э��
        /************************************************Unity�������¼�***********************************************/

        /************************************************�� �� �� �� ��************************************************/
        //ע���������
        public override void OnRegister()
        {
            this.View = TopViewHelper.ETopView.eEasyTest;
            base.OnRegister();
        }
        //ȡ��ע���������
        public override void OnRemove()
        {
            base.OnRemove();
        }
        //������Դ��
        protected override void PowerClick()
        {
            if (this.changeIconCoroutine != null)
                this.StopCoroutine(this.changeIconCoroutine);

            this.EasyTestView.Select();
        }
        //������Դ��
        protected override void PowerHold()
        {
            this.EasyTestView.Fade(true);
        }
        //ҡ�α���
        protected override void CupShake()
        {
            if (this.changeIconCoroutine != null)
                this.StopCoroutine(this.changeIconCoroutine);

            this.EasyTestView.Fade(false);
        }
        //��������
        protected override void CupTiltLeft()
        {
            if (this.changeIconCoroutine != null)
                this.StopCoroutine(this.changeIconCoroutine);
            this.changeIconCoroutine = this.StartCoroutine(this.ContinueMove(true));
        }
        //��������
        protected override void CupTiltRight()
        {
            if (this.changeIconCoroutine != null)
                this.StopCoroutine(this.changeIconCoroutine);
            this.changeIconCoroutine = this.StartCoroutine(this.ContinueMove(false));
        }
        //���ӷ�ƽ
        protected override void CupKeepFlat()
        {
            if (this.changeIconCoroutine != null)
                this.StopCoroutine(this.changeIconCoroutine);
        }
        //�����л�����ͼ��
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