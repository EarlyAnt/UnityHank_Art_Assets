using UnityEngine;

namespace CupSdk.CupMotion.Listener
{
    public class PoseListener : AndroidJavaProxy
    {
        public PoseListener() : base("com.bowhead.sheldon.motion.CupMotion$PoseListener"){}

        public void onPoseChange(int pose){
            if(mPoseChange != null){
                Loom.QueueOnMainThread(()=>{
                    mPoseChange(pose);
                });
            }
            
        }
        public void normalPoseTimingReport(){
            if(mPoseChange != null){
                Loom.QueueOnMainThread(()=>{
                    mNormalPoseTimingReport();
                });
            }
            
        }

        private PoseChange mPoseChange;
        private NormalPoseTimingReport mNormalPoseTimingReport;
        public void setListener(PoseChange poseChange,NormalPoseTimingReport normalPoseTimingReport){
            mPoseChange = poseChange;
            mNormalPoseTimingReport = normalPoseTimingReport;
        }
    }


    public delegate void PoseChange(int pose);

    public delegate void NormalPoseTimingReport();
}