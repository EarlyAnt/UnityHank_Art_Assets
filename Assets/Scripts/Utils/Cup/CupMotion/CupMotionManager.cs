using Cup.Utils.android;
using CupSdk.CupMotion.Listener;
using System.Threading;
using UnityEngine;

namespace Cup.Utils.CupMotion
{
    public class CupMotionManager
    {
        private static event PoseChange PoseListenerEvent = delegate(int pose){};

        private static event NormalPoseTimingReport NormalPoseTimingReportEvent = delegate(){};

        private static event Shake ShakeEvent = delegate(){};

        private static event TiltStatusChange TiltStatusChangeEvent = delegate(int status){};

        private static bool isStart = false;

        private static PoseListener mPostListener = new PoseListener();
        private static ShakeListener mShakeListener = new ShakeListener();

        private static TiltListener mTiltListener = new TiltListener();

        private static CupSdk.CupMotion.CupMotion mCupMotion;

        public static void start(){
            if(!isStart){
                isStart = true;
                Debug.Log("<><>CupMotionManager start<><>");

                mPostListener.setListener(onPoseChange,onNormalPoseTimingReport);
                mShakeListener.setListener(onShake);
                mTiltListener.setListener(onTiltStatusChange);
                mCupMotion = new CupSdk.CupMotion.CupMotion();

                mCupMotion.registerPoseListener(mPostListener);
                mCupMotion.registerShakeListener(mShakeListener);
                mCupMotion.registerTiltListener(mTiltListener);

                AndroidJavaClass LooperClass = new AndroidJavaClass("android.os.Looper");
                AndroidJavaObject mainLooper = LooperClass.CallStatic<AndroidJavaObject>("getMainLooper");

                AndroidJavaObject handler = new AndroidJavaObject("android.os.Handler",mainLooper);
                
                bool status = mCupMotion.startMonitor(handler);

                Debug.Log("<><>start status<><>"+status);
            }
        }

        public static void stop(){
            mCupMotion.stopMonitor();
            isStart = false;
        }


        public static void addPoseChangeListener(PoseChange poseChange){
            PoseListenerEvent += poseChange;
        }

        public static void removePoseChangeListener(PoseChange poseChange){
            PoseListenerEvent -= poseChange;
        }

        private static void onPoseChange(int pose){
            Debug.Log("<><>CupMotionManager onPoseChange<><>");

            Debug.Log("<><>" + Thread.CurrentThread.Name);
            /*Debug.unityLogger.Log("CLD", Thread.CurrentThread.Name);*/

            Loom.QueueOnMainThread(() =>
            {
                Debug.Log("<><>" + Thread.CurrentThread.Name);
                /*Debug.unityLogger.Log("CLD", Thread.CurrentThread.Name);*/
                PoseListenerEvent(pose);
            });
        }

        public static void addNormalPoseTimingReportListener(NormalPoseTimingReport normalPoseTimingReport){
            NormalPoseTimingReportEvent += normalPoseTimingReport;
        }

        public static void removeNormalPoseTimingReportListener(NormalPoseTimingReport normalPoseTimingReport){
            NormalPoseTimingReportEvent -= normalPoseTimingReport;
        }


        private static void onNormalPoseTimingReport(){
            //Debug.Log("<><>CupMotionManager onNormalPoseTimingReport<><>");
            Loom.QueueOnMainThread(() =>
            {
                NormalPoseTimingReportEvent();
            });
        }

        public static void addShakeListener(Shake shake){
            ShakeEvent += shake;
        }

        public static void removeShakeListener(Shake shake){
            ShakeEvent -= shake;
        }

        private static void onShake(){
            Debug.Log("<><>CupMotionManager onShake<><>");
            Loom.QueueOnMainThread(() =>
            {
                ShakeEvent();
            });
        }

        public static void addTiltStatusChangeEventListener(TiltStatusChange tiltStatusChange){
            TiltStatusChangeEvent += tiltStatusChange;
        }

        public static void removeTiltStatusChangeEventListener(TiltStatusChange tiltStatusChange){
            TiltStatusChangeEvent -= tiltStatusChange;
        }

        private static void onTiltStatusChange(int status){
            Debug.Log("<><>CupMotionManager onTiltStatusChange<><>");
            Loom.QueueOnMainThread(() => {
                TiltStatusChangeEvent(status);
            }); 
            
        }
    }
}