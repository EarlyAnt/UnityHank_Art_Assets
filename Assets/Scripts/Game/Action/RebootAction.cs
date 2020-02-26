using Cup.Utils.android;
using Hank.Api;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hank.Action
{
    public class RebootAction : ActionBase
    {
        HeartBeatReboot value;
        public override void Init(HeartBeatActionBase actionModel)
        {
            value = actionModel as HeartBeatReboot;
        }
        public override void DoAction()
        {
            PowerManager.reboot();

            BuildBackAckBody(value.ack, "Reboot", "OK", value.todoid);
        }
    }
}
