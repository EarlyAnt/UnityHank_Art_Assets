using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Assertions;

namespace Hank.MainScene
{
    public class ExpIncrease : MainPresentBase
    {
		[SerializeField]
        Image imageExpProcess;
        public override void TestSerializeField()
        {
            Assert.IsNotNull(imageExpProcess);
        }

        public override UIStateEnum GetStateEnum()
        {
            return UIStateEnum.eExpIncrease;
        }
        public override void ProcessState(UIState state)
        {
			bFinish = false;
			imageExpProcess.DOKill();
			Debug.Log("=-------exp fill" + state.value0.ToString() + ">>>>>>>>>>>:"+state.value1);
			imageExpProcess.DOFillAmount((float)state.value0, 0.5f).onComplete = delegate ()
			{
				bFinish = true;
			};
        }


        // Update is called once per frame
        void Update()
        {

        }
    }
}

