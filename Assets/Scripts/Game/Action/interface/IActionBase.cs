using BestHTTP;
using Cup.Utils.android;
using Gululu;
using Gululu.net;
using Gululu.Util;
using Hank.Api;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hank.Action
{

    // public struct ActionPostBody
    // {
    //     public struct Value
    //     {
    //         public string key;
    //         public string value;

    //     }
    //     public string msg;
    //     public string status;
    //     public string cup_hw_sn;
    //     public string todoid;
    //     public List<Value> values;

    // }

    public interface IActionBase
    {
        void Init(HeartBeatActionBase actionModel);
        void DoAction();
        ActionPostBody backAckBody { get ; }

        string ackAdress { get ; }

    }
}
