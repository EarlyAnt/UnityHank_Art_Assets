using Gululu;
using Gululu.Events;
using System.Collections.Generic;

namespace Hank.DownloadPage2
{
    public class DownloadPageMediator : BaseMediator
    {
        /************************************************�������������************************************************/
        [Inject]
        public DownloadPage2View DownloadPageView { get; set; }
        /************************************************Unity�������¼�***********************************************/

        /************************************************�� �� �� �� ��************************************************/
        //ע���������
        public override void OnRegister()
        {
            this.View = TopViewHelper.ETopView.eDownloadPage2;
            base.OnRegister();
            this.DownloadPageView.StopSignal.AddListener(this.OnStopDownload);
            BaseMediator.LockKeysIgnore(new List<MainKeys>() { MainKeys.PowerClick });
        }
        //ȡ��ע���������
        public override void OnRemove()
        {
            BaseMediator.UnlockAllKeys();
            this.DownloadPageView.StopSignal.RemoveListener(this.OnStopDownload);
            base.OnRemove();
        }
        //������Դ��
        protected override void PowerClick()
        {
            this.DownloadPageView.ConfirmExit();
        }
        //ҡ�α���
        protected override void CupShake()
        {
            this.DownloadPageView.ConfirmExit();
        }
        //��ֹͣ����ʱ
        private void OnStopDownload(bool isCompleted)
        {
            this.dispatcher.Dispatch(MainEvent.CloseDownloadPage2);
        }
    }
}