using Gululu;
using Gululu.Events;

namespace Hank.MainScene
{
    public class DebugMediator : BaseMediator
    {
        /************************************************属性与变量命名************************************************/
        [Inject]
        public DebugView DebugView { get; set; }
        /************************************************Unity方法与事件***********************************************/

        /************************************************自 定 义 方 法************************************************/
        public override void OnRegister()
        {
            this.registerView = false;
            base.OnRegister();
            this.DebugView.ChangeMissionIdSignal.AddListener(this.DispatchEvent);
        }

        public override void OnRemove()
        {
            this.DebugView.ChangeMissionIdSignal.RemoveListener(this.DispatchEvent);
            base.OnRemove();
        }

        private void DispatchEvent(MainEvent mainEvent)
        {
            this.dispatcher.Dispatch(mainEvent);
        }
    }
}