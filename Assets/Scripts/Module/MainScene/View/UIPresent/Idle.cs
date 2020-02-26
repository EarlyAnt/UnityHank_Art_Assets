using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hank.MainScene
{
    public  class Idle : MainPresentBase
    {

        public override UIStateEnum GetStateEnum()
        {
            return UIStateEnum.eIdle;
        }
        public override void ProcessState(UIState state)
        {
            bFinish = true;
        }


        // Update is called once per frame
        void Update()
        {

        }
    }
}

