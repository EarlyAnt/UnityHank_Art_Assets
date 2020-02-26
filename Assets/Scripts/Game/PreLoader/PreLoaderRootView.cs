using Cup.Utils.Screen;
using Gululu.Config;
using Gululu.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Gululu;
using Spine.Unity;
using Cup.Utils.android;
using CupSdk.BaseSdk.Battery;

namespace Hank.PreLoad
{
    public class PreLoaderRootView : MonoBehaviour
    {
        void Awake()
        {
            BatteryUtils.init();
            FlurryUtil.Init();
            AsyncActionHelper.Instance.init();
            FlurryUtil.SetUserId(CupBuild.getCupSn());
            FlurryUtil.LogEvent("Hank_Splashscreen_View");
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            GameObject objGlobalContextView = null; /*= GameObject.Find("GlobalContextView");
            if (objGlobalContextView == null)*/
            {
                objGlobalContextView = new GameObject();
                objGlobalContextView.name = "GlobalContextView";
                GlobalContextView globalContextView = objGlobalContextView.AddComponent<GlobalContextView>();
                GameObject.DontDestroyOnLoad(globalContextView);
            }
#if (UNITY_IOS || UNITY_ANDROID) && (!UNITY_EDITOR)
#if WETEST_SDK
            objGlobalContextView.AddComponent<WeTest.U3DAutomation.U3DAutomationBehaviour>();
            BuglyAgent.RegisterLogCallback(WeTest.U3DAutomation.CrashMonitor._OnLogCallbackHandler);
#endif
#endif

        }


    }
}
