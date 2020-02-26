using System;
using System.Collections;
using System.Collections.Generic;
using Hank.Api;
using UnityEngine;

namespace Hank.Action
{
    public class ActionFactory : IActionFactory
    {
        [Inject(name = "SetValuesAction")]
        public IActionBase mSetValuesAction { get; set; }
        [Inject(name = "GetValuesAction")]
        public IActionBase mGetValuesAction { get; set; }
        [Inject(name = "SetValuesLongAction")]
        public IActionBase mSetValuesLongAction { get; set; }
        [Inject(name = "GetValuesLongAction")]
        public IActionBase mGetValuesLongAction { get; set; }
        [Inject(name = "ResetAction")]
        public IActionBase mResetAction { get; set; }
        [Inject(name = "RebootAction")]
        public IActionBase mRebootAction { get; set; }
        [Inject(name = "DeleteAction")]
        public IActionBase mDeleteAction { get; set; }

        public IActionBase CreateAction(Type key, HeartBeatActionBase actionData)
        {
            IActionBase retAction = null;
            switch (key.Name)
            {
                case "HeartBeatSetValues":
                    {
                        retAction = mSetValuesAction;
                    }
                    break;
                case "HeartBeatGetValues":
                    {
                        retAction = mGetValuesAction;
                    }
                    break;
                case "HeartBeatSetValuesLong":
                    {
                        retAction = mSetValuesLongAction;
                    }
                    break;
                case "HeartBeatGetValuesLong":
                    {
                        retAction = mGetValuesLongAction;
                    }
                    break;
                case "HeartBeatReset":
                    {
                        retAction = mResetAction;
                    }
                    break;
                case "HeartBeatReboot":
                    {
                        retAction = mRebootAction;
                    }
                    break;
                case "HeartBeatDelete":
                    {
                        retAction = mDeleteAction;
                    }
                    break;

            }
            Debug.LogFormat("<><ActionFactory.CreateAction>Type: {0}", key.Name);
            if (retAction != null)
            {
                Debug.LogFormat("<><ActionFactory.CreateAction>retAction: {0}", retAction.GetType().Name);
                retAction.Init(actionData);
            }
            return retAction;
        }
    }
}
