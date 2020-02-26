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

    public abstract class ActionBase : IActionBase
    {

        [Inject]
        public IPlayerDataManager dataManager { get; set; }
        [Inject]
        public IMissionDataManager missionDataManager { get; set; }
        [Inject]
        public IItemsDataManager itemsDataManager { get; set; }
        [Inject]
        public ISilenceTimeDataManager silenceTimeDataManager { get; set; }        
        [Inject]
        public IFundDataManager fundDataManager { get; set; }
        [Inject]
        public IAccessoryDataManager accessoryDataManager { get; set; }

        public abstract void Init(HeartBeatActionBase actionModel);
        public abstract void DoAction();

        ActionPostBody _backAckBody;
        public ActionPostBody backAckBody{get{return _backAckBody;}}

        string _ackAdress;
        public string ackAdress { get { return _ackAdress; } }


        string GetAckAddress(HeartBeatAck ack)
        {
            return "http://" + ack.server + ":" + ack.port + "/" + ack.url;
        }

        protected void BuildBackAckBody(HeartBeatAck ack, string msg, string status, string todoid, List<ActionPostBodyValue> values = null)
        {
            _ackAdress = GetAckAddress(ack);
            _backAckBody = ActionPostBody.getBuilder().build();
            _backAckBody.cup_hw_sn = CupBuild.getCupSn();
            _backAckBody.msg = msg;
            _backAckBody.status = status;
            _backAckBody.todoid = todoid;
            if (values == null)
            {
                _backAckBody.values = new List<ActionPostBodyValue>();
            }
            else
            {
                _backAckBody.values = values;
            }
            // body.values.Add(new ActionPostBody.Value
            // {
            //     key = "timestamp",
            //     value = DateUtil.GetTimeStamp().ToString()
            // });

//             backAckBody.values.Add(ActionPostBodyValue.getBuilder().setKey("timestamp").setValue( DateUtil.GetTimeStamp().ToString()).build());
            // body.values.Add(new ActionPostBody.Value
            // {
            //     key = "timezone",
            //     value = "8"
            // });

//             backAckBody.values.Add(ActionPostBodyValue.getBuilder().setKey("timezone").setValue("8").build());
            // string param = new NetUtils().Json2String<ActionPostBody>(body);

            // IUrlProvider mUrlProvider = new UrlProvider();
            // INetUtils mNetUtils = new NetUtils();
            // IAuthenticationUtils mAuthenticationUtils = new AuthenticationUtils();
            // mAuthenticationUtils.mNetUtils = mNetUtils;
            // mAuthenticationUtils.mUrlProvider = mUrlProvider;
            // GululuNetwork mGululuNetwork = new GululuNetwork();
            // mGululuNetwork.mAuthenticationUtils = mAuthenticationUtils;
            // mGululuNetwork.mNetUtils = mNetUtils;

            // mGululuNetwork.sendRequest(GetAckAddress(ack), null, param, (r) =>
            // {

            //     GuLog.Log("ackRet:" + r);

            // }, (e) =>
            // {
            //     GuLog.Log("ackRet Error:" + e.ErrorInfo);
            // }, HTTPMethods.Post);

//             sendAckService.serviceSendAckBackSignal.AddListener((result)=>{
//                 GuLog.Log("ackRet:" + result);
//             });
// 
//             sendAckService.serviceSendAckErrBackSignal.AddListener((errorResult)=>{
//                 GuLog.Log("ackRet Error:" + errorResult.ErrorInfo);
//             });
// 
//             sendAckService.sendAck(GetAckAddress(ack),backAckBody);


        }
    }
}
