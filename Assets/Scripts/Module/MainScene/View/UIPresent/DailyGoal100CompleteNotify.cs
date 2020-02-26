using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Spine.Unity;
using UnityEngine.Assertions;

namespace Hank.MainScene
{
    public class DailyGoal100CompleteNotify : MainPresentBase
    {
		[SerializeField]
        SkeletonGraphic complete100CwGp;
		[SerializeField]
        SkeletonGraphic complete100PgGp;
        public override void TestSerializeField()
        {
            Assert.IsNotNull(complete100CwGp);
            Assert.IsNotNull(complete100PgGp);
        }

        public override UIStateEnum GetStateEnum()
        {
            return UIStateEnum.eDailyGoal100CompleteNotify;
        }
        public override void ProcessState(UIState state)
        {
			bFinish = false;
			complete100CwGpAni();
			complete100PgGpAni();
        }

        // Use this for initialization
        protected override void Start()
        {
            base.Start();
            complete100CwGp.gameObject.SetActive(false);
            complete100PgGp.gameObject.SetActive(false);

            complete100PgGp.AnimationState.Complete += delegate
			{
                complete100PgGp.gameObject.SetActive(false);
                bFinish = true;
			};
        }

        // Update is called once per frame
        void Update()
        {

        }

		void complete100CwGpAni()
		{
			complete100CwGp.gameObject.SetActive(true);
            complete100CwGp.AnimationState.ClearTracks();
			complete100CwGp.AnimationState.SetAnimation(0, "daily goal crown_jump", false);
			complete100CwGp.AnimationState.AddAnimation(0, "daily goal crown_idle", true, 0);
		}

		void complete100PgGpAni()
		{
            complete100PgGp.gameObject.SetActive(true);
            complete100PgGp.AnimationState.ClearTracks();
			complete100PgGp.AnimationState.SetAnimation(0, "daily goal glow", false);
		}
    }
}

