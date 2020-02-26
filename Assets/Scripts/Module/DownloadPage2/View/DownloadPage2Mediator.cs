using Gululu;
using Gululu.Events;
using System.Collections.Generic;

namespace Hank.DownloadPage2
{
    public class DownloadPageMediator : BaseMediator
    {
        /************************************************属性与变量命名************************************************/
        [Inject]
        public DownloadPage2View DownloadPageView { get; set; }
        /************************************************Unity方法与事件***********************************************/

        /************************************************自 定 义 方 法************************************************/
        //注册输入监听
        public override void OnRegister()
        {
            this.View = TopViewHelper.ETopView.eDownloadPage2;
            base.OnRegister();
            this.DownloadPageView.StopSignal.AddListener(this.OnStopDownload);
            BaseMediator.LockKeysIgnore(new List<MainKeys>() { MainKeys.PowerClick });
        }
        //取消注册输入监听
        public override void OnRemove()
        {
            BaseMediator.UnlockAllKeys();
            this.DownloadPageView.StopSignal.RemoveListener(this.OnStopDownload);
            base.OnRemove();
        }
        //单击电源键
        protected override void PowerClick()
        {
            this.DownloadPageView.ConfirmExit();
        }
        //摇晃杯子
        protected override void CupShake()
        {
            this.DownloadPageView.ConfirmExit();
        }
        //当停止下载时
        private void OnStopDownload(bool isCompleted)
        {
            this.dispatcher.Dispatch(MainEvent.CloseDownloadPage2);
        }
    }
}