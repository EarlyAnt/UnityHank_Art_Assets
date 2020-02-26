using UnityEngine;

namespace Assets.Script.Global
{
    public class AppInfo : MonoBehaviour
    {
        /************************************************属性与变量命名************************************************/
        [SerializeField]
        private string channel;
        public string Channel { get { return this.channel; } }
        [SerializeField]
        private string version;
        public string Version { get { return this.version; } }
        [SerializeField]
        private int versionCode;
        public int VersionCode { get { return this.versionCode; } }
        public static AppInfo Instance { get; private set; }
        /************************************************Unity方法与事件***********************************************/
        private void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        private void OnEnable()
        {
            this.ReadConfig();
        }
        /************************************************自 定 义 方 法************************************************/
        [ContextMenu("1-读取配置")]
        private void ReadConfig()
        {
#if UNITY_EDITOR && UNITY_ANDROID
            this.version = UnityEditor.PlayerSettings.bundleVersion;
            this.versionCode = UnityEditor.PlayerSettings.Android.bundleVersionCode;
#endif
        }
        [ContextMenu("2-保存配置")]
        private void SaveConfig()
        {
#if UNITY_EDITOR && UNITY_ANDROID
            //Application.version = this.version;
            UnityEditor.PlayerSettings.bundleVersion = this.version;
            UnityEditor.PlayerSettings.Android.bundleVersionCode = this.versionCode;
#endif
        }
    }
}
