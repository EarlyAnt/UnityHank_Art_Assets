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
        /************************************************�������������************************************************/
        private bool percent20 = false;//20%�͵�ʱ��������
        private bool percent10 = false;//10%�͵�ʱ��������
        private bool inCharging = false;//�Ƿ����ڳ��
        private Queue<string> tips = new Queue<string>();
        /************************************************Unity�������¼�***********************************************/
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
        /************************************************�� �� �� �� ��************************************************/
        //ѭ��������
        private void UpdateBatteryLevel()
        {
#if (UNITY_ANDROID) && (!UNITY_EDITOR)
            int batteryCapacity = BatteryUtils.getBatteryCapacity(AndroidContextHolder.GetAndroidContext());
            GuLog.Debug(string.Format("----BattaryStatusListenerView.UpdateBatteryLevel----Battery: {0}, InCharging: {1}", batteryCapacity, this.inCharging));
            while (this.tips.Count > 0) GuLog.Debug(this.tips.Dequeue());
#endif
        }
        //�������仯ʱ
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
                this.percent10 = false;//ֻҪ��������10��20֮�䣬percent10��Ӧ�ûָ���false��һ�����ܵĳ����ǣ��͵�10%���µ�ʱ���磬�嵽10%���ϰε���Դ�ߣ��ٴε���10%ʱ��Ӧ���ٴ�����
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
        //�����ӵ�Դ��ʱ(���״̬)
        private void OnConnected()
        {
            this.inCharging = true;
        }
        //���Ͽ���Դ��ʱ(�����״̬)
        private void OnDisconnected()
        {
            this.inCharging = false;
        }
    }
}
