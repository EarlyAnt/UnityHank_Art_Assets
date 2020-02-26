using UnityEngine;
using Spine.Unity;
using CupSdk.BaseSdk.Battery;
using Cup.Utils.android;
using UnityEngine.UI;
using Gululu;
using UnityEngine.Assertions;

namespace Hank.MainScene
{
    public class InCharging : BaseView
    {
        [Inject]
        public StartHeartBeatCommandSignal startHeartBeatCommandSignal { get; set; }

        public void onBatteryLow()
        {
            GuLog.Info("<><InCharging>onBatteryLow<><>");

        }

        public void onBatteryOkay()
        {
            GuLog.Info("<><InCharging>onBatteryOkay<><>");
        }

        public void onBatteryLevelChange(int level)
        {
            Loom.QueueOnMainThread(() =>
            {
                GuLog.Info("<><InCharging>onBatteryLevelChange<><>" + level);
            });
        }

        [SerializeField]
        SkeletonGraphic batteryAni;
        [SerializeField]
        LowBattery lowBattery;
        [SerializeField]
        Text textTemperature;
        [SerializeField]
        Text textTemperatureLabel;
        void BatteryUpdate()
        {
            if (gameObject.activeInHierarchy)
            {
                textTemperatureLabel.gameObject.SetActive(false);
#if SHOW_VERSION
                textTemperatureLabel.gameObject.SetActive(true);
                int temperature = BatteryUtils.getBatteryTemperature(AndroidContextHolder.GetAndroidContext());
                textTemperature.text = temperature.ToString();
#endif
                int batteryCapacity = BatteryUtils.getBatteryCapacity(AndroidContextHolder.GetAndroidContext());
                string aniPlay = "power100";

                if (batteryCapacity >= 100)
                {
                    aniPlay = "power100end";
                }
                else if (batteryCapacity > 60)
                {
                    aniPlay = "power100";
                }
                else if (batteryCapacity > 40)
                {
                    aniPlay = "power80";
                }
                else if (batteryCapacity > 20)
                {
                    aniPlay = "power60";
                }
                else
                {
                    aniPlay = "power40";
                }
                //batteryAni.AnimationState.ClearTracks();
                batteryAni.AnimationState.SetAnimation(0, aniPlay, true);
                Debug.LogFormat("<><InCharging.BatteryUpdate>battery capaticy: {0}, aniPlay: {1}", batteryCapacity, aniPlay);
            }
        }

        private void RepeatBatteryUppdate()
        {
            gameObject.SetActive(true);
            lowBattery.HideCloseLowBattary();
            startHeartBeatCommandSignal.Dispatch();
            InvokeRepeating("BatteryUpdate", 0, 60);
            TopViewHelper.Instance.AddTopView(TopViewHelper.ETopView.eInCharge);
        }

        void onConnected()
        {
            GuLog.Info("<><InCharging><>onConnected<><>");

            RepeatBatteryUppdate();
            SoundPlayer.GetInstance().PlaySoundType("system_charging_start");
        }

        void onDisconnected()
        {
            TopViewHelper.Instance.RemoveView(TopViewHelper.ETopView.eInCharge);
            lowBattery.StartLowBattery();
            gameObject.SetActive(false);
            GuLog.Info("<><InCharging>onDisconnected<><>");
            CancelInvoke("BatteryUpdate");
        }

        private void BatteryInitial()
        {
            BatteryUtils.registerBatteryLevelListener(onBatteryLow, onBatteryOkay, onBatteryLevelChange);

            BatteryUtils.registerPowerConnectionListener(onConnected, onDisconnected);

            if (BatteryUtils.isCharging(AndroidContextHolder.GetAndroidContext()))
            {
                RepeatBatteryUppdate();
            }
            else
            {
                onDisconnected();
            }
            bool batteryStatus = BatteryUtils.isCharging(AndroidContextHolder.GetAndroidContext());
            GuLog.Info("<><InCharging>BatteryUtils.isCharging<><>" + batteryStatus.ToString());
        }

        // Use this for initialization
        override protected void Start()
        {
            base.Start();
            gameObject.SetActive(false);

#if (UNITY_ANDROID) && (!UNITY_EDITOR)
            BatteryInitial();
#else
            //             onDisconnected();
#endif
            textTemperature.gameObject.SetActive(false);
#if SHOW_VERSION
            textTemperature.gameObject.SetActive(true);
#endif
        }



        // Update is called once per frame
        void Update()
        {

        }

        public override void TestSerializeField()
        {
            Assert.IsNotNull(batteryAni);
            Assert.IsNotNull(lowBattery);
            Assert.IsNotNull(textTemperature);
            Assert.IsNotNull(textTemperatureLabel);
        }

    }
}

