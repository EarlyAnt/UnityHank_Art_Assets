using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Spine.Unity;
using Spine;
using Cup.Utils.Touch;
using Gululu.LocalData.Agents;
using UnityEngine.Assertions;

namespace Hank.MainScene
{
    public class PlayerLevelUp : MainPresentBase
    {
        [Inject]
        public ILocalPetInfoAgent mLocalPetInfoAgent { get; set; }

        [SerializeField]
		SkeletonGraphic levelUpAni;
		[SerializeField]
		GameObject levelUpBlur;
        [SerializeField]
        LevelUI levelSC;
        bool isIdle = false;

        int level = 0;
        public override void TestSerializeField()
        {
            Assert.IsNotNull(levelUpAni);
            Assert.IsNotNull(levelUpBlur);
            Assert.IsNotNull(levelSC);
        }

        public override UIStateEnum GetStateEnum()
        {
            return UIStateEnum.ePlayerLevelUp;
        }
        public override void ProcessState(UIState state)
        {
			bFinish = false;
			isIdle = false;
            level = (int)state.value0;

            SoundPlayer.GetInstance().PlaySoundType("level_up_" + mLocalPetInfoAgent.getCurrentPet());

            levelUpBlur.SetActive(true);
            levelUpAni.AnimationState.ClearTracks();
            levelUpAni.AnimationState.SetAnimation(1, "lv_enter", false);
        }

        // Use this for initialization
        protected override void Start()
        {
            base.Start();
            levelUpBlur.SetActive(false);

			levelUpAni.AnimationState.Complete += delegate (TrackEntry trackEntry)
            {
				if (trackEntry.trackIndex == 1)
				{
					isIdle = true;
					levelUpAni.AnimationState.ClearTracks();
					levelUpAni.AnimationState.SetAnimation(0, "lv_idle", true);
				}
				else if(trackEntry.trackIndex == 3)
				{
                    levelSC.SetLevel(level);
                    levelSC.SetExp(0);
                    bFinish = true;
                    levelUpBlur.SetActive(false);
				}
            };
        }

        // Update is called once per frame
        void Update()
        {

        }

		public void powerKeyPressUp()
		{
            if (TopViewHelper.Instance.HasModulePageOpened())
                return;

            if (isIdle)
			{
				isIdle = false;
				levelUpAni.AnimationState.ClearTracks();
				levelUpAni.AnimationState.SetAnimation(3, "lv_fly", false);
			}
		}

        private void OnEnable()
        {
#if (UNITY_ANDROID) && (!UNITY_EDITOR)
            CupKeyEventHookStaticManager.addPowerKeyPressCallBack(powerKeyPressUp);
#endif
        }

        private void OnDisable()
        {
#if (UNITY_ANDROID) && (!UNITY_EDITOR)
            CupKeyEventHookStaticManager.removePowerKeyPressCallBack(powerKeyPressUp);
#endif
        }
        override public void Stop()
        {
            base.Stop();
            levelUpBlur.SetActive(false);
        }


    }
}

