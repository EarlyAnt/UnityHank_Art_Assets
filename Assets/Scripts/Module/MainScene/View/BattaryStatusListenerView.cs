using Cup.Utils.Screen;
using Cup.Utils.android;
using CupSdk.BaseSdk.Battery;
using Gululu;
using System.Collections.Generic;
using System;

namespace Hank.MainScene
{
    public class BattaryStatusListenerView : BaseView
    {
        /************************************************属性与变量命名************************************************/
        private bool percent20 = false;//20%低电时亮屏提醒
        private bool percent10 = false;//10%低电时亮屏提醒
        private bool inCharging = false;//是否正在充电
        private Queue<string> tips = new Queue<string>();
        /************************************************Unity方法与事件***********************************************/
        protected override void Start()
        {
#if (UNITY_ANDROID) && (!UNITY_EDITOR)
            int batteryCapacity = BatteryUtils.getBatteryCapacity(AndroidContextHolder.GetAndroidContext());
            this.inCharging = BatteryUtils.isCharging(AndroidContextHolder.GetAndroidContext());
#else
            int batteryCapacity = 15;
#endif
            this.InvokeRepeating("UpdateBatteryLevel", 0, 5);
            this.OnBatteryLevelChange(batteryCapacity);
            BatteryUtils.registerPowerConnectionListener(this.OnConnected, this.OnDisconnected);
            BatteryUtils.registerBatteryLevelListener(null, null, this.OnBatteryLevelChange);
            GuLog.Debug(string.Format("----BattaryStatusListenerView.Start----unSafe: {0}", true));
        }
        protected override void OnDestroy()
        {
            this.CancelInvoke("UpdateBatteryLevel");
            BatteryUtils.unRegisterPowerConnectionListener(this.OnConnected, this.OnDisconnected);
            BatteryUtils.unRegisterBatteryLevelListener(null, null, this.OnBatteryLevelChange);
            base.OnDestroy();
        }
        /************************************************自 定 义 方 法************************************************/
        //循环检测电量
        private void UpdateBatteryLevel()
        {
#if (UNITY_ANDROID) && (!UNITY_EDITOR)
            int batteryCapacity = BatteryUtils.getBatteryCapacity(AndroidContextHolder.GetAndroidContext());
            GuLog.Debug(string.Format("----BattaryStatusListenerView.UpdateBatteryLevel----Battery: {0}, InCharging: {1}", batteryCapacity, this.inCharging));
            while (this.tips.Count > 0) GuLog.Debug(this.tips.Dequeue());
#endif
        }
        //当电量变化时
        private void OnBatteryLevelChange(int batteryCapacity)
        {
            GuLog.Debug(string.Format("----BattaryStatusListenerView.OnBatteryLevelChange----Battery: {0}, InCharging: {1}", batteryCapacity, this.inCharging));

            if (batteryCapacity >= 20)
            {
                this.percent20 = false;
                this.percent10 = false;
                GuLog.Debug(string.Format("----BattaryStatusListenerView.OnBatteryLevelChange.More than 20%----Battery: {0}, {1}", batteryCapacity, DateTime.Now.ToString("HH:mm:ss:fff")));
            }
            else if (batteryCapacity >= 10 && batteryCapacity < 20)
            {
                this.percent10 = false;//只要电量处在10和20之间，percent10就应该恢复成false，一个可能的场景是：低电10%以下的时候充电，冲到10%以上拔掉电源线，再次低于10%时，应该再次亮屏
                if (!this.inCharging && !this.percent20)
                {
                    this.percent20 = true;
                    this.TurnOnScreen();
                    GuLog.Debug(string.Format("----BattaryStatusListenerView.OnBatteryLevelChange.TurnOnScreen----Battery: {0}, {1}", batteryCapacity, DateTime.Now.ToString("HH:mm:ss:fff")));
                }
            }
            else if (batteryCapacity < 10)
            {
                if (!this.inCharging && !this.percent10)
                {
                    this.percent10 = true;
                    this.TurnOnScreen();
                    GuLog.Debug(string.Format("----BattaryStatusListenerView.OnBatteryLevelChange.TurnOnScreen----Battery: {0}, {1}", batteryCapacity, DateTime.Now.ToString("HH:mm:ss:fff")));
                }
            }
        }

        private void TurnOnScreen()
        {
#if (UNITY_ANDROID) && (!UNITY_EDITOR)
            GuLog.Debug("----BattaryStatusListenerView.TurnOnScreen----");
            ScreenManager.turnOnScreen();
#endif
        }
        //当连接电源线时(充电状态)
        private void OnConnected()
        {
            this.inCharging = true;
        }
        //当断开电源线时(不充电状态)
        private void OnDisconnected()
        {
            this.inCharging = false;
        }
    }
}
