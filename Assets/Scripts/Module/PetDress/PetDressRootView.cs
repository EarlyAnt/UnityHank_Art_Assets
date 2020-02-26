using strange.extensions.context.impl;

namespace Hank.PetDress
{
    public class PetDressRootView : ContextView
    {
        void Awake()
        {
            context = new PetDressContext(this, strange.extensions.context.api.ContextStartupFlags.MANUAL_LAUNCH);
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