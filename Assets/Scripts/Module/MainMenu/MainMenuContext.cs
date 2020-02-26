using strange.extensions.command.api;
using strange.extensions.command.impl;
using strange.extensions.context.api;
using strange.extensions.context.impl;
using UnityEngine;
using Gululu.Events;

namespace Hank.MainMenu
{
    public class MainMenuContext : MVCSContext
    {
        public MainMenuContext(MonoBehaviour view) : base(view)
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

        public MainMenuContext(MonoBehaviour view, ContextStartupFlags flags) : base(view, flags)
        {


        }

        protected override void mapBindings()
        {
            mediationBinder.Bind<MainMenuView>().To<MainMenuMediator>();

            commandBinder.Bind<StartSignal>().To<StartCommand>();
        }
    }
}