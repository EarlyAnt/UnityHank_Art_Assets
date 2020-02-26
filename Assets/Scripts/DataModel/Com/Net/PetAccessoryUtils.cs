using Cup.Utils.android;
using Gululu.LocalData.Agents;
using Hank.Api;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gululu.net
{
    //小宠物配饰数据同步工具
    public class PetAccessoryUtils : IPetAccessoryUtils
    {
        /// <summary>
        /// 同步数据类
        /// </summary>
        class ActionData
        {
            public DateTime RegisterTime { get; set; }//数据注册时间
            public string Header { get; set; }
            public string Body { get; set; }
            public int SendTimes { get; set; }//发送次数(默认3次，3次失败后，不再重发)
            public Action<Result> OnSuccess { get; set; }//http请求成功时回调
            public Action<Result> OnFailed { get; set; }//http请求失败时回调
        }

        [Inject]
        public IUrlProvider UrlProvider { get; set; }
        [Inject]
        public IJsonUtils JsonUtils { get; set; }
        [Inject]
        public ILocalCupAgent LocalCupAgent { get; set; }
        [Inject]
        public ILocalChildInfoAgent LocalChildInfoAgent { get; set; }
        [Inject]
        public ILocalPetAccessoryAgent LocalPetAccessoryAgent { get; set; }
        [Inject]
        public INativeOkHttpMethodWrapper NativeOkHttpMethodWrapper { get; set; }
        [Inject]
        public GululuNetworkHelper GululuNetworkHelper { get; set; }
        [Inject]
        public OrderReceiptSignal OrderReceiptSignal { get; set; }
        [Inject]
        public OrderPaymentSignal OrderPaymentSignal { get; set; }
        private List<ActionData> actionDatas = new List<ActionData>();
        private ActionData lastActionData = null;

        //购买配饰
        public void BuyPetAccessories(PetAccessoryOrder petAccessoryOrder, Action<Result> callBack = null, Action<Result> errCallBack = null)
        {
            Header header = new Header();
            header.headers = new List<HeaderData>();
            header.headers.Add(new HeaderData() { key = "Gululu-Agent", value = GululuNetworkHelper.GetAgent() });
            header.headers.Add(new HeaderData() { key = "udid", value = GululuNetworkHelper.GetUdid() });
            header.headers.Add(new HeaderData() { key = "Accept-Language", value = GululuNetworkHelper.GetAcceptLang() });
            header.headers.Add(new HeaderData() { key = "Content-Type", value = "application/json" });
            string strHeader = this.JsonUtils.Json2String(header);

            if (petAccessoryOrder == null || petAccessoryOrder.items == null || petAccessoryOrder.items.Count == 0)
            {//如果本地找不到已经解锁的宠物，那默认宠物就作为第一个已解锁宠物
                if (errCallBack != null)
                {
                    errCallBack(new Result()
                    {
                        boolValue = false,
                        info = "<><PetAccessoryUtils.BuyPetAccessories>Error: shopping cart is empty"
                    });
                }
                return;
            }

            string strBody = this.JsonUtils.Json2String(petAccessoryOrder);
            Debug.Log("<><PetAccessoryUtils.BuyPetAccessories>Prepare data");

            ActionData newActionData = new ActionData()
            {
                Header = strHeader,
                Body = strBody,
                SendTimes = 3,
                OnSuccess = callBack,
                OnFailed = errCallBack
            };

            //检查队列里是否已经有数据
            if (this.lastActionData != null && this.lastActionData.Body == strBody)
            {//队列里有数据，只需要处理数据是否压栈
                Debug.LogFormat("<><PetAccessoryUtils.BuyPetAccessories>Ignore same data, Header: {0}, Body: {1}", strHeader, strBody);
                return;//如果新数据与最后一条数据的Body相同，直接忽略
            }
            else
            {//队列里没有数据，才需要数据压栈，且主动调用数据同步方法
                Debug.LogFormat("<><PetAccessoryUtils.BuyPetAccessories>Append new data and execute, Header: {0}, Body: {1}", strHeader, strBody);
                this.lastActionData = newActionData;
                this.actionDatas.Add(newActionData);
                this.SendPetAccessoryOrder();
            }
        }
        //发送购买配饰数据
        private void SendPetAccessoryOrder()
        {
            if (this.actionDatas.Count > 0)
            {
                ActionData actionData = this.actionDatas[0];
                Debug.LogFormat("<><PetAccessoryUtils.SendPetAccessoryOrder>Header: {0}, Body: {1}, SendTimes: {2}", actionData.Header, actionData.Body, actionData.SendTimes);
                Debug.LogFormat("<><PetAccessoryUtils.SendPetAccessoryOrder>Url: {0}", this.UrlProvider.BuyPetAccessories(this.LocalChildInfoAgent.getChildSN(), CupBuild.getCupSn()));
                this.NativeOkHttpMethodWrapper.post(this.UrlProvider.BuyPetAccessories(this.LocalChildInfoAgent.getChildSN(), CupBuild.getCupSn()), actionData.Header, actionData.Body, (result) =>
                {
                    Debug.LogFormat("<><PetAccessoryUtils.SendPetAccessoryOrder>GotResponse: {0}", result);
                    OrderReceipt orderReceipt = this.JsonUtils.String2Json<OrderReceipt>(result);
                    this.OrderReceiptSignal.Dispatch(orderReceipt);
                    if (actionData.OnSuccess != null) Loom.QueueOnMainThread(() => actionData.OnSuccess(Result.Success(result)));
                    Debug.LogFormat("<><PetAccessoryUtils.SendPetAccessoryOrder>Success:\n{0}", result);
                    //检查数据
                    if (this.actionDatas.Count > 0)
                    {
                        this.lastActionData = null;
                        this.actionDatas.RemoveAt(0);//移除已经执行成功的数据
                        if (this.actionDatas.Count > 0)//执行下一条数据
                            this.SendPetAccessoryOrder();
                    }

                }, (errorResult) =>
                {
                    Debug.LogFormat("<><PetAccessoryUtils.SendPetAccessoryOrder>Error: {0}", errorResult.ErrorInfo);
                    if (actionData.OnFailed != null) Loom.QueueOnMainThread(() => actionData.OnFailed(Result.Error(errorResult.ErrorInfo)));
                    //检查数据
                    if (this.actionDatas.Count > 0)
                    {
                        this.lastActionData = null;
                        if (this.actionDatas[0].SendTimes > 0)
                        {//重复上传(最多3次)
                            this.actionDatas[0].SendTimes -= 1;
                            Debug.LogFormat("<><PetAccessoryUtils.SendPetAccessoryOrder>Repeat, SendTimes: {0}, Body: {1}", actionData.SendTimes, actionData.Body);
                            this.SendPetAccessoryOrder();
                        }
                        else
                        {//3次重传失败放弃
                            this.actionDatas.RemoveAt(0);
                            Debug.LogFormat("<><PetAccessoryUtils.SendPetAccessoryOrder>Abandon, SendTimes: {0}, Body: {1}", actionData.SendTimes, actionData.Body);
                        }
                    }
                });
            }
        }
        //获取付款结果
        public void GetPaymentResult(string orderSN, Action<GetPaymentResultResponse> callBack = null, Action<string> errCallBack = null)
        {
            string url = this.UrlProvider.GetPaymentResult(this.LocalChildInfoAgent.getChildSN(), CupBuild.getCupSn(), orderSN);
            Debug.LogFormat("<><PetAccessoryUtils.GetPaymentResult>ChildSN: {0}, CupSN: {1}, Url: {2}", this.LocalChildInfoAgent.getChildSN(), CupBuild.getCupSn(), url);

            this.NativeOkHttpMethodWrapper.get(url, "", (result) =>
            {
                Debug.LogFormat("<><PetAccessoryUtils.GetPaymentResult>Result: {0}", result);
                GetPaymentResultResponse response = this.JsonUtils.String2Json<GetPaymentResultResponse>(result);
                if (response != null && !string.IsNullOrEmpty(response.status))
                {
                    this.OrderPaymentSignal.Dispatch(response.status.ToUpper() == "OK");
                    Debug.LogFormat("<><PetAccessoryUtils.GetPaymentResult>Response data is valid: {0}", result);
                    if (callBack != null) callBack(response);
                }
            },
            (errorInfo) =>
            {
                Debug.LogErrorFormat("<><PetAccessoryUtils.GetPaymentResult>Error: {0}", errorInfo.ErrorInfo);
                if (errCallBack != null) errCallBack(errorInfo.ErrorInfo);
            });
        }
        //获取所有已购买的配饰列表
        public void GetPaidItems(Action<GetPaidItemsResponse> callBack = null, Action<string> errCallBack = null)
        {
            string url = this.UrlProvider.GetPaidItems(this.LocalChildInfoAgent.getChildSN(), CupBuild.getCupSn());
            //Debug.LogFormat("<><PetAccessoryUtils.GetPaidItems>ChildSN: {0}, CupSN: {1}, Url: {2}", this.LocalChildInfoAgent.getChildSN(), CupBuild.getCupSn(), url);

            this.NativeOkHttpMethodWrapper.get(url, "", (result) =>
            {
                //Debug.LogFormat("<><PetAccessoryUtils.GetPaidItems>Result: {0}", result);
                GetPaidItemsResponse response = this.JsonUtils.String2Json<GetPaidItemsResponse>(result);
                if (response != null && !string.IsNullOrEmpty(response.status) && response.status.ToUpper() == "OK")
                {
                    Debug.LogFormat("<><PetAccessoryUtils.GetPaidItems>Response data is valid: {0}", result);
                    if (callBack != null) callBack(response);
                }
            },
            (errorInfo) =>
            {
                Debug.LogErrorFormat("<><PetAccessoryUtils.GetPaidItems>Error: {0}", errorInfo.ErrorInfo);
                if (errCallBack != null) errCallBack(errorInfo.ErrorInfo);
            });
        }
    }
}