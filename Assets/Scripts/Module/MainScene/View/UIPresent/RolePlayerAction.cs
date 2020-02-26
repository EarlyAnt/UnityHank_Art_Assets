using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Hank.MainScene
{
    public class RolePlayerAction : MainPresentBase
    {
        [SerializeField]
        RolePlayer rolePlayerSC;

        Coroutine myCoroutine;

        public override void TestSerializeField()
        {
            Assert.IsNotNull(rolePlayerSC);
        }

        public override UIStateEnum GetStateEnum()
        {
            return UIStateEnum.eRolePlayAction;
        }


        IEnumerator WaitActionOver(string actionName)
        {
            sleepTimeManager.AddSleepStatus(SleepTimeout.NeverSleep);
            while (!rolePlayerSC.IsActionOnPlaying(actionName))
            {
                yield return null;
            }
            float time = rolePlayerSC.GetCurrentAnimTime();

            yield return new WaitForSeconds(time);
            sleepTimeManager.PopSleepStatus();
            myCoroutine = null;
            bFinish = true;
        }


        public override void ProcessState(UIState state)
        {
            string actionName = (string)state.value0;
            Debug.Log("<><RolePlayerAction> actionName:" + actionName + "   needWait:" + state.needWait);

            rolePlayerSC.Play(actionName);

            if(state.needWait)
            {
                bFinish = false;
                if(myCoroutine != null)
                {
                    StopCoroutine(myCoroutine);
                }
                myCoroutine = StartCoroutine(WaitActionOver(actionName));
            }
            else
            {
                bFinish = true;
            }
        }


        // Update is called once per frame
        void Update()
        {

        }
    }

}
