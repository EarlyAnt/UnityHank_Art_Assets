using Hank;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cup.Utils.audio;
using Cup.Utils.Screen;

namespace Hank.MainScene
{
    public abstract class SilenceMode : MainPresentBase
    {

        [Inject]
        public IPlayerDataManager dataManager { get; set; }

        [SerializeField]
        protected dailyProcessBar processBar;
        [Inject]
        public ProcessFreshNewsSignal processFreshNewsSignal { get; set; }





        //abstract protected void ScreenOn();
        //protected void ScreenOnOff(string status)
        //{
        //    switch (status)
        //    {
        //        case "ACTION_SCREEN_ON":
        //            {
        //                ScreenOn();
        //            }
        //            break;
        //    }
        //}

        // Use this for initialization
        protected override void Start()
        {
            base.Start();
            gameObject.SetActive(false);
        }

    }
}

