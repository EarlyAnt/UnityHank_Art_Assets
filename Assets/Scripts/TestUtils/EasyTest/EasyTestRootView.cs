using strange.extensions.context.impl;

namespace Hank.EasyTest
{
    public class EasyTestRootView : ContextView
    {
        void Awake()
        {
            context = new EasyTestContext(this, strange.extensions.context.api.ContextStartupFlags.MANUAL_LAUNCH);
        }

        private void OnEnable()
        {
            if (context != null)
            {
                context.Launch();
            }
        }

    }
}