using BestHTTP;
using Gululu;
using Gululu.net;
using Hank.Api;
using Hank.MainScene;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hank.Action
{
    public class GetValuesLongAction : ActionBase
    {
        HeartBeatGetValuesLong value;

        [Inject]
        public SyncAllGameDataSignal syncAllGameDataSingal { get; set; }
        public override void Init(HeartBeatActionBase actionModel)
        {
            value = actionModel as HeartBeatGetValuesLong;
        }

        string GetDataUploadAddress(HeartBeatGetValuesLong.GetValuesLongTask ack)
        {
            return "http://" + ack.server + ":" + ack.port + "/" + ack.url;
        }


        public override void DoAction()
        {
            bool error = false;

            switch (value.task.type)
            {
                case "game_data":
                    {
                        syncAllGameDataSingal.Dispatch();
                    }
                    break;
                case "config_times":
                    {
                    }
                    break;
                default:
                    {
                        error = true;
                    }
                    break;

            }
            if (error)
            {
                BuildBackAckBody(value.ack, value.task.type, "ERROR", value.todoid);
            }
            else
            {
                BuildBackAckBody(value.ack, "GetValuesLong", "OK", value.todoid);
            }
        }
    }
}
