using Gululu;
using Gululu.Events;
using strange.extensions.dispatcher.eventdispatcher.api;
using System.Collections;
using UnityEngine;

namespace Hank.PetDress
{
    public class PetDressMediator : BaseMediator
    {
        /************************************************�������������************************************************/
        [Inject]
        public PetDressView PetDressView { get; set; }
        private Coroutine changeAccessoryCoroutine;//�л�����Э��(�������������ʱ�����л�����ͼ��)
        /************************************************Unity�������¼�***********************************************/

        /************************************************�� �� �� �� ��************************************************/
        //ע���������
        public override void OnRegister()
        {
            this.View = TopViewHelper.ETopView.ePetDress;
            this.PetDressView.CloseViewSignal.AddListener(this.OnClose);
            base.OnRegister();
        }
        //ȡ��ע���������
        public override void OnRemove()
        {
            this.PetDressView.CloseViewSignal.RemoveListener(this.OnClose);
            base.OnRemove();
        }
        //������Դ��
        protected override void PowerClick()
        {
            if (this.changeAccessoryCoroutine != null)
                this.StopCoroutine(this.changeAccessoryCoroutine);

            this.PetDressView.Confirm();
        }
        //ҡ�α���
        protected override void CupShake()
        {
            if (this.changeAccessoryCoroutine != null)
                this.StopCoroutine(this.changeAccessoryCoroutine);

            this.OnClose(true);
        }
        //��������
        protected override void CupTiltLeft()
        {
            if (this.changeAccessoryCoroutine != null)
                this.StopCoroutine(this.changeAccessoryCoroutine);
            this.changeAccessoryCoroutine = this.StartCoroutine(this.ContinueMove(true));
        }
        //��������
        protected override void CupTiltRight()
        {
            if (this.changeAccessoryCoroutine != null)
                this.StopCoroutine(this.changeAccessoryCoroutine);
            this.changeAccessoryCoroutine = this.StartCoroutine(this.ContinueMove(false));
        }
        //���ӷ�ƽ
        protected override void CupKeepFlat()
        {
            if (this.changeAccessoryCoroutine != null)
                this.StopCoroutine(this.changeAccessoryCoroutine);
        }
        //�����л�����ͼ��
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
        //��������ϻ�
        protected override void CupLeftSwipeUp()
        {
            if (this.changeAccessoryCoroutine != null)
                this.StopCoroutine(this.changeAccessoryCoroutine);
            this.PetDressView.PreAccessoryType();
        }
        //��������»�
        protected override void CupLeftSwipeDown()
        {
            if (this.changeAccessoryCoroutine != null)
                this.StopCoroutine(this.changeAccessoryCoroutine);
            this.PetDressView.NextAccessoryType();
        }
        //�����Ҳ��ϻ�
        protected override void CupRightSwipeUp()
        {
            if (this.changeAccessoryCoroutine != null)
                this.StopCoroutine(this.changeAccessoryCoroutine);
            this.dispatcher.Dispatch(MainEvent.ShowNavigator, MainEvent.NavigatorUp);
        }
        //�����Ҳ��»�
        protected override void CupRightSwipeDown()
        {
            if (this.changeAccessoryCoroutine != null)
                this.StopCoroutine(this.changeAccessoryCoroutine);
            this.dispatcher.Dispatch(MainEvent.ShowNavigator, MainEvent.NavigatorDown);
        }
        //���ر�ҳ��ʱ
        protected override void OnClose(bool initiative)
        {
            this.PetDressView.SetViewSnapShot();
            dispatcher.Dispatch(MainEvent.ClosePetDress, initiative);
        }
    }
}