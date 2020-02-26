using strange.extensions.command.api;
using strange.extensions.command.impl;
using strange.extensions.context.api;
using strange.extensions.context.impl;
using UnityEngine;

namespace Hank.EasyTest
{
    public class EasyTestContext : MVCSContext
    {
        public EasyTestContext(MonoBehaviour view) : base(view)
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

        public EasyTestContext(MonoBehaviour view, ContextStartupFlags flags) : base(view, flags)
        {


        }

        protected override void mapBindings()
        {
            mediationBinder.Bind<EasyTestView>().To<EasyTestMediator>();

            /*Insert Bingder Here*/

            commandBinder.Bind<StartSignal>().To<StartCommand>();
        }
    }
}