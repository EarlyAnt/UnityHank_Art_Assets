using strange.extensions.context.impl;

namespace Hank.DownloadPage2
{
    public class DownloadPage2RootView : ContextView
    {
        void Awake()
        {
            context = new DownloadPage2Context(this, strange.extensions.context.api.ContextStartupFlags.MANUAL_LAUNCH);
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