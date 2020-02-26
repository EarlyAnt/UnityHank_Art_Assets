using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Assertions;

namespace Hank.MainScene
{
    public class DrinkFail : MainPresentBase
    {

        [SerializeField]
        Animator shakeDrop;
        bool shakeAniEnd;
        public override void TestSerializeField()
        {
            Assert.IsNotNull(shakeDrop);
        }

        public override UIStateEnum GetStateEnum()
        {
            return UIStateEnum.eDrinkFail;
        }
        public override void ProcessState(UIState state)
        {
            shakeAniEnd = false;
            bFinish = false;
            shakeDrop.Play("shakeDrop");
            SoundPlayer.GetInstance().PlaySoundType("water_cheat");

            StartCoroutine(WaitActionOver("shakeDrop"));
        }

        bool IsActionOnPlaying(string actionName)
        {
            AnimatorStateInfo stateinfo = shakeDrop.GetCurrentAnimatorStateInfo(0);
            return stateinfo.IsName(actionName);

        }

        IEnumerator WaitActionOver(string actionName)
        {
            sleepTimeManager.AddSleepStatus(SleepTimeout.NeverSleep);
            while (!IsActionOnPlaying(actionName))
            {
                yield return null;
            }
            AnimatorStateInfo stateinfo = shakeDrop.GetCurrentAnimatorStateInfo(0);
            float time = stateinfo.length;

            yield return new WaitForSeconds(time);
            bFinish = true;
            sleepTimeManager.PopSleepStatus();

        }


        //         void shakeDropAni()
        //         {
        //             AnimatorStateInfo shakeDropInfo = shakeDrop.GetCurrentAnimatorStateInfo(0);
        //             if (shakeDropInfo.IsName("shakeDrop") && shakeDropInfo.normalizedTime >= 1.0f)
        //             {
        //                 shakeDrop.Play("empty");
        //                 shakeAniEnd = true;
        //                 bFinish = true;
        //             }
        //         }


        // Update is called once per frame
        void Update()
        {
//             if (!shakeAniEnd)
//             {
//                 shakeDropAni();
//             }
        }

    }
}

