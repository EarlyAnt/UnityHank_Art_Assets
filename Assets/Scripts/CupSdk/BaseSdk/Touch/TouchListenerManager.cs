using UnityEngine;

namespace CupSdk.BaseSdk.Touch
{
    public class TouchListenerManager
    {
        private AndroidJavaObject javaTouchListenerManager;
        public  TouchListenerManager(){
            javaTouchListenerManager = new AndroidJavaObject("com.bowhead.hank.gesturedetectorutils.BaseTouchListenerManager");
        }

        public void setGestureListener(SwipeUpward swipeUpward, SwipeDownward swipeDownward){
            GestureListener gestureListener = new GestureListener();
            gestureListener.addCallback(swipeUpward,swipeDownward);
            javaTouchListenerManager.Call("setGestureListener",gestureListener);
        }

        public void setExtraGestureListener(SwipeForward swipeForward, SwipeBackward swipeBackward){
            ExtraGestureListener extraGestureListener = new ExtraGestureListener();
            extraGestureListener.addCallback(swipeForward,swipeBackward);
            javaTouchListenerManager.Call("setExtraGestureListener",extraGestureListener);
        }

        public void setSwipeUpAndDownListener(SwipeUpAndDown swipeUpAndDown){
            SwipeUpAndDownListener swipeUpAndDownListener = new SwipeUpAndDownListener();
            swipeUpAndDownListener.addCallback(swipeUpAndDown);
            javaTouchListenerManager.Call("setSwipeUpAndDownListener",swipeUpAndDownListener);
        }

        public void setSwipeForthAndBackListener(SwipeForthAndBack swipeForthAndBack){
            SwipeForthAndBackListener swipeForthAndBackListener = new SwipeForthAndBackListener();
            swipeForthAndBackListener.addCallback(swipeForthAndBack);
            javaTouchListenerManager.Call("setSwipeForthAndBackListener",swipeForthAndBackListener);
        }

        public void setTapListener(Tap tap){
            TapListener tapListener = new TapListener();
		    tapListener.addCallback(tap);
            javaTouchListenerManager.Call("setTapListener",tapListener);
        }

        public void setDoubleTapListener(DoubleTap doubleTap){
            DoubleTapListener doubleTapListener = new DoubleTapListener();
		    doubleTapListener.addCallback(doubleTap);
            javaTouchListenerManager.Call("setDoubleTapListener",doubleTapListener);
        }

        public AndroidJavaObject getManager(){
            return javaTouchListenerManager;
        }
    }
}