using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using strange.extensions.mediation.impl;
using Gululu;

namespace Hank
{
    public abstract class MainPresentBase : BaseView 
    {
        [Inject]
        public ISleepTimeManager sleepTimeManager { get; set; }
        public abstract UIStateEnum GetStateEnum();
        public virtual void ProcessState(UIState state)
        {
            bFinish = true;
        }


        protected bool bFinish = false;

        virtual public bool IsFinish()
        {
            return bFinish;
        }

        virtual public void Stop()
        {
            bFinish = true;
        }

        protected void SetFinishAndOpenScreenSave(bool flag)
        {
            bFinish = flag;
            if (bFinish)
            {
                sleepTimeManager.PopSleepStatus();
            }
            else
            {
                sleepTimeManager.AddSleepStatus(SleepTimeout.NeverSleep);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

