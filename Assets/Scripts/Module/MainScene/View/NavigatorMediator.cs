using Gululu;
using Gululu.Events;
using strange.extensions.dispatcher.eventdispatcher.api;

namespace Hank.MainScene
{
    public class NavigatorMediator : BaseMediator
    {
        /************************************************属性与变量命名************************************************/
        [Inject]
        public NavigatorView NavigatorView { get; set; }
        /************************************************Unity方法与事件***********************************************/

        /************************************************自 定 义 方 法************************************************/
        public override void OnRegister()
        {
            this.registerView = false;
            base.OnRegister();
            this.dispatcher.UpdateListener(true, MainEvent.ResetNavigator, this.OnResetNavigator);
            this.dispatcher.UpdateListener(true, MainEvent.ClosePetFeed, this.OnCloseView);
            this.dispatcher.UpdateListener(true, MainEvent.ClosePetDress, this.OnCloseView);
            this.NavigatorView.CloseLastViewSignal.AddListener(this.OnCloseLastView);
        }

        public override void OnRemove()
        {
            this.dispatcher.UpdateListener(false, MainEvent.ResetNavigator, this.OnResetNavigator);
            this.dispatcher.UpdateListener(false, MainEvent.ClosePetFeed, this.OnCloseView);
            this.dispatcher.UpdateListener(false, MainEvent.ClosePetDress, this.OnCloseView);
            this.NavigatorView.CloseLastViewSignal.RemoveListener(this.OnCloseLastView);
            base.OnRemove();
        }

        private void OnResetNavigator(IEvent evt)
        {
            this.NavigatorView.TurnToHomePage();
        }

        private void OnCloseView(IEvent evt)
        {
            if (evt != null && evt.data != null && (bool)evt.data)
                this.NavigatorView.TurnToHomePage();
        }

        private void OnCloseLastView(TopViewHelper.ETopView view)
        {
            this.dispatcher.Dispatch(MainEvent.CloseView, view);
        }
    }
}