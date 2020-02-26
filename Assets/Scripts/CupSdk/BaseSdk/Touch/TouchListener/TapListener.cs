using UnityEngine;

namespace CupSdk.BaseSdk.Touch
{
    public class TapListener : AndroidJavaProxy
    {
        public TapListener() : base("com.bowhead.sheldon.touch.TouchBar$TapListener"){}

        public void onTap(){
            if(mTap != null)
                mTap();
        }

        private Tap mTap;
        public void addCallback(Tap tap){
            mTap = tap;
        }
    }



    public delegate void Tap();
    
}