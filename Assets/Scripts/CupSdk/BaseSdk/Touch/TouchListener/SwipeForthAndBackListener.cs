using UnityEngine;

namespace CupSdk.BaseSdk.Touch
{
    public class SwipeForthAndBackListener : AndroidJavaProxy
    {
        public SwipeForthAndBackListener() : base("com.bowhead.sheldon.touch.TouchBar$SwipeForthAndBackListener"){}

        public void onSwipeForthAndBack(){
            if(mSwipeForthAndBack != null)
                mSwipeForthAndBack();
        }

        private SwipeForthAndBack mSwipeForthAndBack;
        public void addCallback(SwipeForthAndBack swipeForthAndBack){
            mSwipeForthAndBack = swipeForthAndBack;
        }
    }

    
    public delegate void SwipeForthAndBack();
    
}