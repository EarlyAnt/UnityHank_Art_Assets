using Gululu.Util;
using strange.extensions.command.impl;
using strange.extensions.context.api;
using UnityEngine;

namespace Hank.PetDress
{
    public class StartCommand : Command
    {
        [Inject(ContextKeys.CONTEXT_VIEW)]
        public GameObject contextView { get; set; }
        [Inject]
        public IPrefabRoot mPrefabRoot { get; set; }

        public override void Execute()
        {
            GameObject go = mPrefabRoot.GetGameObject("UI", "PetDressView");
            go.transform.SetParent(contextView.transform,false);
            go.SetActive(true);
        }
    }
}