using Cup.Utils.Wifi;
using Gululu;
using UnityEngine;
using UnityEngine.Assertions;

namespace Hank.MainMenu
{
    public class WifiStatusView : BaseView
    {
        /************************************************属性与变量命名************************************************/
        [Inject]
        public ILangManager mLangManager { get; set; }
        NativeWifiManager mNativeWifiManager;
        [SerializeField]
        GameObject connnectedIcon;
        [SerializeField]
        GameObject unconnectedIcon;
        /************************************************Unity方法与事件***********************************************/
        override protected void Start()
        {
            base.Start();

#if (UNITY_ANDROID) && (!UNITY_EDITOR)
            mNativeWifiManager = new NativeWifiManager();
            mNativeWifiManager.setWifiEnabled(true);
            InvokeRepeating("UpdateWifiName", 0, 30);
#endif
        }
        /************************************************自 定 义 方 法************************************************/
        //测试脚本外部资源是否为空
        public override void TestSerializeField()
        {
            Assert.IsNull(connnectedIcon);
            Assert.IsNull(unconnectedIcon);
        }
        //轮询更新Wifi名字
        private void UpdateWifiName()
        {
            AndroidJavaObject wifiInfo = mNativeWifiManager.getConnectionInfo();
            string ssid = string.Empty;
            bool wifiEnable = false;
            if (wifiInfo != null)
            {
                ssid = wifiInfo.Call<string>("getSSID");
                if (ssid != "<unknown ssid>")
                {
                    wifiEnable = true;
                    ssid = ssid.Replace("\"", "");
                }
            }

            this.connnectedIcon.SetActive(wifiEnable);
            this.unconnectedIcon.SetActive(!wifiEnable);
            GuLog.Debug("Rssi:  " + mNativeWifiManager.getRssi());
            //Debug.LogFormat("----Component Test----WifiStatusView.UpdateWifiName: {0}, {1}", wifiEnable, ssid);
        }
    }
}
