using UnityEngine;

namespace CupSdk.BaseSdk.Touch
{
    public class GestureListener : AndroidJavaProxy
    {
        public GestureListener() : base("com.bowhead.sheldon.touch.TouchBar$GestureListener"){}

        public void onSwipeUpward(){
            if(mSwipeUpward != null)
                mSwipeUpward();
        }

        public void onSwipeDownward(){
            if(mSwipeDownward != null)
                mSwipeDownward();
        }

        private SwipeUpward mSwipeUpward;
        private SwipeDownward mSwipeDownward;
        public void addCallback(SwipeUpward swipeUpward, SwipeDownward swipeDownward){
            mSwipeUpward = swipeUpward;
            mSwipeDownward = swipeDownward;
        }

    }

    
    public delegate void SwipeUpward();

    public delegate void SwipeDownward();
    

}