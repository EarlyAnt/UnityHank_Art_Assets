using Gululu.LocalData.Agents;
using Gululu.Util;


namespace Gululu{
    

    public class BuglyUtil
    {

        [Inject]
        public ILocalChildInfoAgent mLocalChildInfoAgent{get;set;}
        public void Init()
        {
#if UNITY_ANDROID 
            // initBugly();
            buglySetup();
#endif
        }

        private void initBugly()
        {

            GuLog.Debug("initBugly");
            BuglyAgent.ConfigDebugMode(true);

#if UNITY_ANDROID
            BuglyAgent.InitWithAppId ("fced1ab5e5");
#endif
            BuglyAgent.EnableExceptionHandler();
        }


        private void buglySetup()
        {
            string account = mLocalChildInfoAgent.getChildSN();
            if (!string.IsNullOrEmpty(account))
            {
                BuglyAgent.SetUserId(account);
            }
        }

    }

   

}
