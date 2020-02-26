using strange.extensions.context.impl;

namespace Hank.CommingSoon
{
    public class CommingSoonRootView : ContextView
    {
        void Awake()
        {
            context = new CommingSoonContext(this, strange.extensions.context.api.ContextStartupFlags.MANUAL_LAUNCH);
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