using strange.extensions.context.impl;
namespace Hank.MainMenu
{
    public class MainMenuRootView : ContextView
    {
        void Awake(){
            context = new MainMenuContext(this, strange.extensions.context.api.ContextStartupFlags.MANUAL_LAUNCH);
        }

        private void OnEnable()
        {
            if(context!= null)
            {
                context.Launch();
            }
        }

    }
}