using UnityEngine;

namespace CupSdk.BaseSdk.Touch
{
    public class DoubleTapListener : AndroidJavaProxy
    {
        public DoubleTapListener() : base("com.bowhead.sheldon.touch.TouchBar$DoubleTapListener"){}

        public void onDoubleTap(){
            if (mDoubleTap != null)
                mDoubleTap();
        }

        private DoubleTap mDoubleTap;
        public void addCallback(DoubleTap doubleTap){
            mDoubleTap = doubleTap;
        }
    }

    public delegate void DoubleTap();
}