using strange.extensions.command.api;
using strange.extensions.command.impl;
using strange.extensions.context.api;
using strange.extensions.context.impl;
using UnityEngine;

namespace Hank.DownloadPage2
{
    public class DownloadPage2Context : MVCSContext
    {
        public DownloadPage2Context(MonoBehaviour view) : base(view)
        {
        }

        protected override void addCoreComponents()
        {
            base.addCoreComponents();
            injectionBinder.Unbind<ICommandBinder>();
            injectionBinder.Bind<ICommandBinder>().To<SignalCommandBinder>().ToSingleton();
        }

        override public void Launch()
        {
            base.Launch();
            StartSignal startSignal = (StartSignal)injectionBinder.GetInstance<StartSignal>();
            startSignal.Dispatch();
        }

        public DownloadPage2Context(MonoBehaviour view, ContextStartupFlags flags) : base(view, flags)
        {


        }

        protected override void mapBindings()
        {
            mediationBinder.Bind<DownloadPage2View>().To<DownloadPageMediator>();

            /*Insert Bingder Here*/

            commandBinder.Bind<StartSignal>().To<StartCommand>();
        }
    }
}