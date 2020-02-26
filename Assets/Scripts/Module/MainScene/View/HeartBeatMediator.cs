using Cup.Utils.Screen;
using Gululu;
using Gululu.Events;
using Gululu.LocalData.DataManager;
using Hank.Api;
using strange.extensions.mediation.impl;
using System;
using UnityEngine;

namespace Hank.MainScene
{
    public class HeartBeatMediator : EventMediator
    {
        [Inject]
        public StartHeartBeatCommandSignal startHeartBeatCommandSignal { get; set; }


        [Inject]
        public MediatorHeartBeatSignal mediatorHeartBeatSignal { get; set; }

        [Inject]
        public MediatorHeartBeatErrSignal mediatorHeartBeatErrSignal { get; set; }

        [Inject]
        public SetValuesLongOkSignal setValuesLongOkSignal { get; set; }

        [Inject(name = "GameHeartbeatAction")]
        public IAbsDoHeartbeatAction gameHeartbeatAction { get; set; }
        [Inject]
        public IHeartbeatActionManager mHeartbearActionManager { set; get; }

        DateTime lastHeartBeatTime;

        public override void OnRegister()
        {
            mHeartbearActionManager.addEventListener(gameHeartbeatAction);

            mediatorHeartBeatSignal.AddListener(HeartBeat);
            mediatorHeartBeatErrSignal.AddListener(ErrorWhenHeartBeat);
            dispatcher.UpdateListener(true, MainEvent.HeartBeatEvent, DispatchHeartBeatSignal);
        }

        public override void OnRemove()
        {
            mediatorHeartBeatSignal.RemoveListener(HeartBeat);
            mediatorHeartBeatErrSignal.RemoveListener(ErrorWhenHeartBeat);
            dispatcher.UpdateListener(false, MainEvent.HeartBeatEvent, DispatchHeartBeatSignal);
        }

        public void HeartBeat(HeartBeatResponse heatBeatData)
        {
            setValuesLongOkSignal.Dispatch();
            lastHeartBeatTime = DateTime.Now;
        }

        public void ErrorWhenHeartBeat(ResponseErroInfo str)
        {
            setValuesLongOkSignal.Dispatch();
            GuLog.Info("HeartBeat Error Code:" + str.ErrorCode + "     Error Info:" + str.ErrorInfo);
        }


        private void DispatchHeartBeatSignal()
        {
            GuLog.Info("<><>HeartBeatMediator<>DispatchHeartBeatSignal<>");
            startHeartBeatCommandSignal.Dispatch();
        }

        void DispatchHeartBeatSignalPer1800Sceonds()
        {
            TimeSpan interval = DateTime.Now - lastHeartBeatTime;
            if (interval.TotalSeconds >= 1800)
            {
                DispatchHeartBeatSignal();
            }

        }


        // Use this for initialization
        void Start()
        {
            //DispatchHeartBeatSignalPer1800Sceonds();
            InvokeRepeating("DispatchHeartBeatSignalPer1800Sceonds", 0, 1800);

        }

        // Update is called once per frame
        void Update()
        {

        }
        private void OnDisable()
        {
#if (UNITY_ANDROID) && (!UNITY_EDITOR)
            ScreenManager.removeListener(ScreenOn);
#endif
        }

        private void OnEnable()
        {
#if (UNITY_ANDROID) && (!UNITY_EDITOR)
            ScreenManager.addListener(ScreenOn);
#endif

        }
        void ScreenOn(string status)
        {
            switch (status)
            {
                case "ACTION_SCREEN_ON":
                    {
                        Debug.Log("<><HeartBeatMediator>ScreenOn");
                        DispatchHeartBeatSignalPer1800Sceonds();
                    }
                    break;
            }

        }

    }
}

