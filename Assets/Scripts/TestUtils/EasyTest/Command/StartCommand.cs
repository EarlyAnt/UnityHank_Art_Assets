using strange.extensions.command.impl;
using strange.extensions.context.api;
using UnityEngine;

namespace Hank.EasyTest
{
    public class StartCommand : Command
    {
        [Inject(ContextKeys.CONTEXT_VIEW)]
        public GameObject contextView { get; set; }

        public override void Execute()
        {
        }
    }
}