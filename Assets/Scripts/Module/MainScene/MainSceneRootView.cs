using strange.extensions.context.impl;
namespace Hank.MainScene
{
    public class MainSceneRootView : ContextView
    {
        void Awake(){
            context = new MainSceneContext(this, strange.extensions.context.api.ContextStartupFlags.MANUAL_LAUNCH);
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