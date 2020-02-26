using UnityEngine;

namespace CupSdk.BaseSdk.Touch 
{
    public class SwipeUpAndDownListener : AndroidJavaProxy
    {
        public SwipeUpAndDownListener() : base("com.bowhead.sheldon.touch.TouchBar$SwipeUpAndDownListener"){}

        public void onSwipeUpAndDown(){
            if(mSwipeUpAndDown != null)
                mSwipeUpAndDown();
        }

        private SwipeUpAndDown mSwipeUpAndDown;
        public void addCallback(SwipeUpAndDown swipeUpAndDown){
            mSwipeUpAndDown = swipeUpAndDown;
        }
    }


    public delegate void SwipeUpAndDown();
    
}