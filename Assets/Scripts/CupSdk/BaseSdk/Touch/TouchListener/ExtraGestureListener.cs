using UnityEngine;

namespace CupSdk.BaseSdk.Touch
{
    public class ExtraGestureListener : AndroidJavaProxy
    {
        public ExtraGestureListener() : base("com.bowhead.sheldon.touch.TouchBar$ExtraGestureListener"){}

        public void onSwipeForward(){
            if(mSwipeForward != null)
                mSwipeForward();
        }

        public void onSwipeBackward(){
            if(mSwipeBackward != null)
                mSwipeBackward();
        }
        private SwipeForward mSwipeForward;
        private SwipeBackward mSwipeBackward;
        public void addCallback(SwipeForward swipeForward, SwipeBackward swipeBackward){
            mSwipeForward = swipeForward;
            mSwipeBackward = swipeBackward;
        }
    }

    public delegate void SwipeForward();

    public delegate void SwipeBackward();
    
}