using System;
using System.Collections;
using System.Collections.Generic;
using Hank.Api;
using UnityEngine;

namespace Hank.Action
{
    public interface IActionFactory 
    {
        IActionBase CreateAction(Type key, HeartBeatActionBase actionData);
    }
}
