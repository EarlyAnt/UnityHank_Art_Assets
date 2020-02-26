using Gululu.Util;
using strange.extensions.command.impl;
using strange.extensions.context.api;
using UnityEngine;
namespace Hank.PreLoad
{
    public class StartCommand : Command
    {
        [Inject(ContextKeys.CONTEXT_VIEW)]
        public GameObject contextView { get; set; }
        [Inject]
        public IPrefabRoot mPrefabRoot { get; set; }

        public override void Execute()
        {
            GameObject go = mPrefabRoot.GetGameObject("UI", "MainSceneLoader");
            GameObject Canvas = GameObject.Find("Canvas");
            go.transform.SetParent(Canvas.transform, false);
            go.SetActive(true);

            //GameObject newObject = new GameObject();
            //newObject.AddComponent<MainSceneLoader>();
        }
    }
}