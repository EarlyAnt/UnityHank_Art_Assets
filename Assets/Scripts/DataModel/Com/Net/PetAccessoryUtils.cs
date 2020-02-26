using Cup.Utils.android;
using Gululu.LocalData.Agents;
using Hank.Api;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gululu.net
{
    //С������������ͬ������
    public class PetAccessoryUtils : IPetAccessoryUtils
    {
        /// <summary>
        /// ͬ��������
        /// </summary>
        class ActionData
        {
            public DateTime RegisterTime { get; set; }//����ע��ʱ��
            public string Header { get; set; }
            public string Body { get; set; }
            public int SendTimes { get; set; }//���ʹ���(Ĭ��3�Σ�3��ʧ�ܺ󣬲����ط�)
            public Action<Result> OnSuccess { get; set; }//http����ɹ�ʱ�ص�
            public Action<Result> OnFailed { get; set; }//http����ʧ��ʱ�ص�
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

        //��������
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
            {//��������Ҳ����Ѿ������ĳ����Ĭ�ϳ������Ϊ��һ���ѽ�������
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

            //���������Ƿ��Ѿ�������
            if (this.lastActionData != null && this.lastActionData.Body == strBody)
            {//�����������ݣ�ֻ��Ҫ���������Ƿ�ѹջ
                Debug.LogFormat("<><PetAccessoryUtils.BuyPetAccessories>Ignore same data, Header: {0}, Body: {1}", strHeader, strBody);
                return;//��������������һ�����ݵ�Body��ͬ��ֱ�Ӻ���
            }
            else
            {//������û�����ݣ�����Ҫ����ѹջ����������������ͬ������
                Debug.LogFormat("<><PetAccessoryUtils.BuyPetAccessories>Append new data and execute, Header: {0}, Body: {1}", strHeader, strBody);
                this.lastActionData = newActionData;
                this.actionDatas.Add(newActionData);
                this.SendPetAccessoryOrder();
            }
        }
        //���͹�����������
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
                    //�������
                    if (this.actionDatas.Count > 0)
                    {
                        this.lastActionData = null;
                        this.actionDatas.RemoveAt(0);//�Ƴ��Ѿ�ִ�гɹ�������
                        if (this.actionDatas.Count > 0)//ִ����һ������
                            this.SendPetAccessoryOrder();
                    }

                }, (errorResult) =>
                {
                    Debug.LogFormat("<><PetAccessoryUtils.SendPetAccessoryOrder>Error: {0}", errorResult.ErrorInfo);
                    if (actionData.OnFailed != null) Loom.QueueOnMainThread(() => actionData.OnFailed(Result.Error(errorResult.ErrorInfo)));
                    //�������
                    if (this.actionDatas.Count > 0)
                    {
                        this.lastActionData = null;
                        if (this.actionDatas[0].SendTimes > 0)
                        {//�ظ��ϴ�(���3��)
                            this.actionDatas[0].SendTimes -= 1;
                            Debug.LogFormat("<><PetAccessoryUtils.SendPetAccessoryOrder>Repeat, SendTimes: {0}, Body: {1}", actionData.SendTimes, actionData.Body);
                            this.SendPetAccessoryOrder();
                        }
                        else
                        {//3���ش�ʧ�ܷ���
                            this.actionDatas.RemoveAt(0);
                            Debug.LogFormat("<><PetAccessoryUtils.SendPetAccessoryOrder>Abandon, SendTimes: {0}, Body: {1}", actionData.SendTimes, actionData.Body);
                        }
                    }
                });
            }
        }
        //��ȡ������
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
        //��ȡ�����ѹ���������б�
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