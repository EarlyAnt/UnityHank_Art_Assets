using Cup.Utils.android;
using CupSdk.BaseSdk.Battery;
using Gululu;
using Gululu.LocalData.Agents;
using Gululu.Util;
using Hank.Api;
using strange.extensions.command.impl;
using System.Collections.Generic;
using UnityEngine;
using Gululu.LocalData.DataManager;
using Hank.Action;
using System;
namespace Hank.MainScene
{
    public class HeartBeatCommand : Command
    {
        [Inject]
        public IHeartBeatService getHeartBeatService { get; set; }

        [Inject]
        public MediatorHeartBeatErrSignal mediatorHeartBeatErrSignal{ get; set; }

        [Inject]
        public MediatorHeartBeatSignal mediatorHeartBeatSignal { get; set; }

        [Inject]
        public ILocalChildInfoAgent mLocalChildInfoAgent { get; set; }
        [Inject]
        public ILocalPetInfoAgent mLocalPetInfoAgent { get; set; }

        [Inject(name = "GameHeartbeatAction")]
        public IAbsDoHeartbeatAction gameHeartbeatAction { get; set; }
        [Inject]
        public IActionFactory mActionFactory { get; set; }
        [Inject]
        public StartHeatBeatSendBackAckCommandSignal startHeatBeatSendBackAckCommandSignal { get; set; }
        [Inject]
        public ILanguageUtils languageUtils { get; set; }

        //         [Inject]
        //         public HeartBeatBody body { set; get; }

        public override void Execute()
        {
            GuLog.Info("<><HeartBeatCommand> Execute!");
            if (mLocalChildInfoAgent.getChildSN() == null || mLocalChildInfoAgent.getChildSN() == string.Empty)
            {
                GuLog.Info("<><HeartBeatCommand> child sn null!");
                FinishAndDispatchResult(ResponseErroInfo.GetErrorInfo(100, "child sn null!"));
                return;
            }
            getHeartBeatService.serviceHeartBeatBackSignal.AddListener(HeartBeatResult);
            getHeartBeatService.serviceHeartBeatErrBackSignal.AddListener(HeartBeatResultErr);

            SChild child = SChild.getBuilder().setFriend_count(10)
                    .setX_child_sn(mLocalChildInfoAgent.getChildSN()).setLanguage(languageUtils.getLanguageToAgentKey()).build();

            CupBuild.getCupHwMac((mac)=>{
                Debug.Log("HeartBeatCommand mac:"+mac);
                SCup cup = SCup.getBuilder().setCup_hw_mac(mac)
#if UNITY_EDITOR
                .setBattery(50)
                .setCharge(false)
                .setCapability(0)
                .build();
#else
                .setBattery(BatteryUtils.getBatteryCapacity(AndroidContextHolder.GetAndroidContext()))
                .setCharge(BatteryUtils.isCharging(AndroidContextHolder.GetAndroidContext()))
                .setCapability(0)
                .build();
#endif
                SGame game = SGame.getBuilder()
                            .setGame_name(AppData.GetAppName())
#if UNITY_EDITOR
                            .setGame_version("1.0.31")
#else
                            .setGame_version(CupBuild.getAppVersionName())
#endif
                            .setPet_model(mLocalPetInfoAgent.getCurrentPet()).build();

                SExtra extra = SExtra.getBuilder().setTimestamp(DateUtil.GetTimeStamp()).setTimezone(8).build();

                HeartBeatBody body = HeartBeatBody.getBuilder().setChild(child).setCup(cup).setGame(game).setExtra(extra).build();


                getHeartBeatService.HeartBeat(body);  
            });

          

            Retain();
        }

        void DoAction(Type type, HeartBeatActionBase actionData)
        {
            Debug.LogFormat("<><HeartBeatCommand.HeartBeatResult>Type: {0}", type.Name);
            IActionBase action = mActionFactory.CreateAction(type, actionData);
            action.DoAction();
            startHeatBeatSendBackAckCommandSignal.Dispatch(action.backAckBody, action.ackAdress);
        }

        public void HeartBeatResult(HeartBeatResponse heatBeatData)
        {
            getHeartBeatService.serviceHeartBeatBackSignal.RemoveListener(HeartBeatResult);
            getHeartBeatService.serviceHeartBeatErrBackSignal.RemoveListener(HeartBeatResultErr);

            if (heatBeatData.actions.get_values != null)
            {
                foreach (var action in heatBeatData.actions.get_values)
                {
                    DoAction(action.GetType(), action);
                }
            }
            if (heatBeatData.actions.set_values != null)
            {
                foreach (var action in heatBeatData.actions.set_values)
                {
                    DoAction(action.GetType(), action);
                }
            }
            if (heatBeatData.actions.get_values_long != null)
            {
                foreach (var action in heatBeatData.actions.get_values_long)
                {
                    DoAction(action.GetType(), action);
                }
            }
            if (heatBeatData.actions.set_values_long != null)
            {
                foreach (var action in heatBeatData.actions.set_values_long)
                {
                    DoAction(action.GetType(), action);
                }
            }
            if (heatBeatData.actions.reset != null)
            {
                foreach (var action in heatBeatData.actions.reset)
                {
                    DoAction(action.GetType(), action);
                }
            }
            if (heatBeatData.actions.reboot != null)
            {
                foreach (var action in heatBeatData.actions.reboot)
                {
                    DoAction(action.GetType(), action);
                }
            }
            if (heatBeatData.actions.delete != null)
            {
                foreach (var action in heatBeatData.actions.delete)
                {
                    DoAction(action.GetType(), action);
                }
            }
            gameHeartbeatAction.onFinish();

            Loom.QueueOnMainThread(() =>
            {
                mediatorHeartBeatSignal.Dispatch(heatBeatData);
            });

            Release();
        }

        private void FinishAndDispatchResult(ResponseErroInfo result)
        {
            gameHeartbeatAction.onFinish();
            Loom.QueueOnMainThread(() =>
            {
                mediatorHeartBeatErrSignal.Dispatch(result);
            });
        }

        public void HeartBeatResultErr(ResponseErroInfo result)
        {
            getHeartBeatService.serviceHeartBeatBackSignal.RemoveListener(HeartBeatResult);
            getHeartBeatService.serviceHeartBeatErrBackSignal.RemoveListener(HeartBeatResultErr);
            FinishAndDispatchResult(result);
            Release();
        }
    }
}
