using System;
using System.Collections.Generic;
using System.Text;
using BestHTTP;
using Cup.Utils.android;
using Gululu.LocalData.Agents;
using Hank.Api;
using UnityEngine;

namespace Gululu.net
{
    //已解锁宠物数据同步工具
    public class UnlockedPetsUtils : IUnlockedPetsUtils
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
        public ILocalPetInfoAgent LocalPetInfoAgent { get; set; }
        [Inject]
        public ILocalChildInfoAgent LocalChildInfoAgent { get; set; }
        [Inject]
        public ILocalUnlockedPetsAgent LocalUnlockedPetsAgent { get; set; }
        [Inject]
        public INativeOkHttpMethodWrapper mNativeOkHttpMethodWrapper { get; set; }
        [Inject]
        public GululuNetworkHelper GululuNetworkHelper { get; set; }
        private List<ActionData> actionDatas = new List<ActionData>();
        private ActionData lastActionData = null;

        //数据同步
        public void SyncUnlockedPets(Action<Result> callBack = null, Action<Result> errCallBack = null)
        {
            Header header = new Header();
            header.headers = new List<HeaderData>();
            header.headers.Add(new HeaderData() { key = "Gululu-Agent", value = GululuNetworkHelper.GetAgent() });
            header.headers.Add(new HeaderData() { key = "udid", value = GululuNetworkHelper.GetUdid() });
            header.headers.Add(new HeaderData() { key = "Accept-Language", value = GululuNetworkHelper.GetAcceptLang() });
            header.headers.Add(new HeaderData() { key = "Content-Type", value = "application/json" });
            string strHeader = this.JsonUtils.Json2String(header);

            List<string> unlockedPets = this.LocalUnlockedPetsAgent.GetUnlockedPets();
            if (unlockedPets == null || unlockedPets.Count == 0)
            {//如果本地找不到已经解锁的宠物，那默认宠物就作为第一个已解锁宠物
                unlockedPets = new List<string>();
                unlockedPets.Add(this.LocalPetInfoAgent.getCurrentPet());
            }
            LocalPetsInfo body = new LocalPetsInfo()
            {
                current_pet = this.LocalPetInfoAgent.getCurrentPet(),
                unlocked_pets = unlockedPets
            };
            string strBody = this.JsonUtils.Json2String(body);
            Debug.Log("--------UnlockedPetsUtils.SyncUnlockedPets.Prepare data--------");

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
                Debug.LogFormat("--------UnlockedPetsUtils.SyncUnlockedPets.Ignore same data--------Header: {0}, Body: {1}", strHeader, strBody);
                return;//如果新数据与最后一条数据的Body相同，直接忽略
            }
            else
            {//队列里没有数据，才需要数据压栈，且主动调用数据同步方法
                Debug.LogFormat("--------UnlockedPetsUtils.SyncUnlockedPets.Append new data and execute--------Header: {0}, Body: {1}", strHeader, strBody);
                this.lastActionData = newActionData;
                this.actionDatas.Add(newActionData);
                this.SyncUnlockedPets();
            }
        }
        //数据同步
        private void SyncUnlockedPets()
        {
            if (this.actionDatas.Count > 0)
            {
                ActionData actionData = this.actionDatas[0];
                Debug.LogFormat("--------UnlockedPetsUtils.SyncUnlockedPets--------Header: {0}, Body: {1}, SendTimes: {2}", actionData.Header, actionData.Body, actionData.SendTimes);
                mNativeOkHttpMethodWrapper.post(this.UrlProvider.GetUnlockedPets(this.LocalChildInfoAgent.getChildSN(), CupBuild.getCupSn()), actionData.Header, actionData.Body, (result) =>
                {
                    Debug.LogFormat("--------UnlockedPetsUtils.SyncUnlockedPets.GotResponse--------{0}", result);
                    RemotePetsInfo remotePetsInfo = this.JsonUtils.String2Json<RemotePetsInfo>(result);
                    this.LocalUnlockedPetsAgent.SaveUnlockedPets(remotePetsInfo.pets);
                    if (actionData.OnSuccess != null) Loom.QueueOnMainThread(() => actionData.OnSuccess(Result.Success(result)));
                    Debug.LogFormat("--------UnlockedPetsUtils.SyncUnlockedPets.Success--------\n{0}", result);
                    //检查数据
                    if (this.actionDatas.Count > 0)
                    {
                        this.actionDatas.RemoveAt(0);//移除已经执行成功的数据
                        if (this.actionDatas.Count > 0)//执行下一条数据
                            this.SyncUnlockedPets();
                    }

                }, (errorResult) =>
                {
                    Debug.LogFormat("--------UnlockedPetsUtils.SyncUnlockedPets.Error--------{0}", Result.Error(errorResult.ErrorInfo));
                    if (actionData.OnFailed != null) Loom.QueueOnMainThread(() => actionData.OnFailed(Result.Error(errorResult.ErrorInfo)));
                    //检查数据
                    if (this.actionDatas.Count > 0)
                    {
                        if (this.actionDatas[0].SendTimes > 0)
                        {//重复上传(最多3次)
                            this.actionDatas[0].SendTimes -= 1;
                            Debug.LogFormat("--------UnlockedPetsUtils.SyncUnlockedPets.Repeat--------SendTimes: {0}, Body: {1}", actionData.SendTimes, actionData.Body);
                            this.SyncUnlockedPets();
                        }
                        else
                        {//3次重传失败放弃
                            this.actionDatas.RemoveAt(0);
                            Debug.LogFormat("--------UnlockedPetsUtils.SyncUnlockedPets.Abandon--------SendTimes: {0}, Body: {1}", actionData.SendTimes, actionData.Body);
                        }
                    }
                });
            }
        }
    }
}