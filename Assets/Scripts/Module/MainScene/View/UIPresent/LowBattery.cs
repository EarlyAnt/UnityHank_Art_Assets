using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;
using Spine;
using DG.Tweening;
using CupSdk.BaseSdk.Battery;
using Cup.Utils.android;
using Cup.Utils.Touch;
using UnityEngine.Assertions;

namespace Hank.MainScene
{
    public class LowBattery : Gululu.BaseView
    {
        private enum BatteryStatus { Normal, LowBattery_20, LowBattery_10 }
        [Inject]
        public IPlayerDataManager PlayerDataManager { get; set; }
        [SerializeField]
        private SkeletonGraphic lowBatteryGp;
        [SerializeField]
        private GameObject backBtn;
        [SerializeField]
        private Image backImg;
        [SerializeField]
        private GameObject smallBattery;
        private BatteryStatus myStatus = BatteryStatus.Normal;

        protected override void Start()
        {
            base.Start();
            HideAll();
            myStatus = BatteryStatus.Normal;
            //InvokeRepeating("LowBatteryUpdate", 0, 1);
        }

        public void PowerKeyPressUp()
        {
            if (!TopViewHelper.Instance.HasModulePageOpened())
                ShowSmallBatteryIcon();
        }

        public void HideCloseLowBattary()
        {
            HideAll();
            CancelInvoke("LowBatteryUpdate");
            myStatus = BatteryStatus.Normal;
        }

        public void StartLowBattery()
        {
            gameObject.SetActive(true);
            CancelInvoke("LowBatteryUpdate");
            InvokeRepeating("LowBatteryUpdate", 0, 60);
        }

        public override void TestSerializeField()
        {
            Assert.IsNotNull(lowBatteryGp);
            Assert.IsNotNull(backBtn);
            Assert.IsNotNull(backImg);
            Assert.IsNotNull(smallBattery);
        }

        private string GetPostfixOfAnimation()
        {
            if (myStatus == BatteryStatus.LowBattery_10)
            {
                return "10";
            }
            return "20";
        }

        private void ShowLowBattery(BatteryStatus lowBattery)
        {
            myStatus = lowBattery;

            if (myStatus == BatteryStatus.LowBattery_10)
            {
                FlurryUtil.LogEvent("Petscreen_LowBattery_10_Ani_View");
            }
            else
            {
                FlurryUtil.LogEvent("Petscreen_LowBattery_20_Ani_View");
            }
            if (this.PlayerDataManager.LanguageInitialized())//设置过语言后才能播放此声音
                SoundPlayer.GetInstance().PlaySoundType("system_charging_low_battery_in");

            if (TopViewHelper.Instance.IsInGuideView())
            {
                ShowSmallBatteryIcon();
            }
            else
            {
                lowBatteryGp.AnimationState.ClearTracks();
                smallBattery.SetActive(false);
                backBtn.SetActive(true);
                backImg.gameObject.SetActive(true);
                backImg.DOFade(0.8f, 0.5f);
                lowBatteryGp.gameObject.SetActive(true);
                lowBatteryGp.AnimationState.SetAnimation(1, "enter_" + GetPostfixOfAnimation(), false);
                lowBatteryGp.AnimationState.Complete += delegate (TrackEntry trackEntry)
                {
                    if (trackEntry.trackIndex == 1)
                    {
                        lowBatteryGp.AnimationState.ClearTracks();
                        lowBatteryGp.AnimationState.SetAnimation(3, "idle_" + GetPostfixOfAnimation(), true);
#if (UNITY_ANDROID) && (!UNITY_EDITOR)
	            CupKeyEventHookStaticManager.addPowerKeyPressCallBack(PowerKeyPressUp);
#endif
                    }
                };
                TopViewHelper.Instance.AddTopView(TopViewHelper.ETopView.eLowBattary);
            }
        }

        private void LowBatteryUpdate()
        {

#if (UNITY_ANDROID) && (!UNITY_EDITOR)
            int batteryCapacity = BatteryUtils.getBatteryCapacity(AndroidContextHolder.GetAndroidContext());
#else
            int batteryCapacity = 15;
#endif
            //GuLog.Info("<lowbattery><>batteryCapacity<><>" + batteryCapacity.ToString());

            switch (myStatus)
            {
                case BatteryStatus.Normal:
                    {
                        if (batteryCapacity >= 10 && batteryCapacity < 20)
                        {
                            ShowLowBattery(BatteryStatus.LowBattery_20);
                        }
                        else if (batteryCapacity < 10)
                        {
                            ShowLowBattery(BatteryStatus.LowBattery_10);
                        }
                    }
                    break;
                case BatteryStatus.LowBattery_20:
                    {
                        if (batteryCapacity >= 20)
                        {
                            FlurryUtil.LogEvent("LowBattery_20_Event");

                            lowBatteryGp.AnimationState.ClearTracks();
                            HideAll();
                            myStatus = BatteryStatus.Normal;
                        }
                        else if (batteryCapacity < 10)
                        {
                            ShowLowBattery(BatteryStatus.LowBattery_10);
                        }

                    }
                    break;
                case BatteryStatus.LowBattery_10:
                    {
                        if (batteryCapacity >= 20)
                        {
                            FlurryUtil.LogEvent("LowBattery_20_Event");
                            FlurryUtil.LogEvent("LowBattery_10_Event");

                            lowBatteryGp.AnimationState.ClearTracks();
                            HideAll();
                            myStatus = BatteryStatus.Normal;
                        }
                        if (batteryCapacity >= 10 && batteryCapacity < 20)
                        {
                            FlurryUtil.LogEvent("LowBattery_10_Event");

                            ShowLowBattery(BatteryStatus.LowBattery_20);
                        }

                    }
                    break;
            }

        }

        private void HideBackBtn()
        {
            if (backBtn.activeSelf)
            {
                backBtn.SetActive(false);
            }
            TopViewHelper.Instance.RemoveView(TopViewHelper.ETopView.eLowBattary);
        }

        private void HideAll()
        {
            smallBattery.SetActive(false);
            HideBackBtn();
            backImg.gameObject.SetActive(false);
            lowBatteryGp.gameObject.SetActive(false);
        }

        private void ShowSmallBatteryIcon()
        {
#if (UNITY_ANDROID) && (!UNITY_EDITOR)
	            CupKeyEventHookStaticManager.removePowerKeyPressCallBack(PowerKeyPressUp);
#endif
            backImg.DOFade(0, 3.33f);
            if (this.PlayerDataManager.LanguageInitialized())//设置过语言后才能播放此声音
                SoundPlayer.GetInstance().PlaySoundType("system_charging_low_battery_out");
            lowBatteryGp.AnimationState.ClearTracks();
            lowBatteryGp.AnimationState.SetAnimation(5, "shrink_" + GetPostfixOfAnimation(), false);
            lowBatteryGp.AnimationState.Complete += delegate (TrackEntry trackEntry)
            {
                if (trackEntry.trackIndex == 5)
                {
                    smallBattery.SetActive(true);
                    HideBackBtn();
                    backImg.gameObject.SetActive(false);
                    lowBatteryGp.AnimationState.ClearTracks();
                    lowBatteryGp.gameObject.SetActive(false);
                }
            };
        }
    }
}

