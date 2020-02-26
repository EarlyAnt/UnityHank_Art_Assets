using strange.extensions.context.impl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Hank.PreLoad
{
    public class GlobalContextView : ContextView
    {
        protected void Awake()
        {
            context = new GlobalContext(this, strange.extensions.context.api.ContextStartupFlags.AUTOMATIC);
        }
    }

}
