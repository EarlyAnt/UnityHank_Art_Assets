using CupSdk.BaseSdk.Touch;
using UnityEngine;

namespace Cup.Utils.Touch
{
    public class TouchLeftStaticManager
    {
         private static TouchListenerManager leftTouchListenerManager = new TouchListenerManager();

        private static event SwipeUpward SwipeUpwardEvent = delegate(){};
        private static event SwipeDownward SwipeDownwardEvent = delegate(){};

        private static bool isStart = false;

        
        
        public static void start(){
            if(!isStart){
                leftTouchListenerManager.setGestureListener(onSwipeUpward,onSwipeDownward);
                leftTouchListenerManager.setExtraGestureListener(onSwipeForward,onSwipeBackward);
                leftTouchListenerManager.setSwipeUpAndDownListener(onSwipeUpAndDown);
                leftTouchListenerManager.setSwipeForthAndBackListener(onSwipeForthAndBack);
                leftTouchListenerManager.setTapListener(onTap);
                leftTouchListenerManager.setDoubleTapListener(onDoubleTap);
                TouchManager.setLeftTouchListenerManager(leftTouchListenerManager.getManager());

                isStart = true;
            }
            
        }

        /*
        _______________________________________________________________________________________
         */

        public static void addSwipeUpwardListener(SwipeUpward swipeUpward){
            Debug.Log("<><>left addSwipeUpwardListener<><>");
            SwipeUpwardEvent += swipeUpward;
        }

        public static void removeSwipeUpwardListener(SwipeUpward swipeUpward){
            Debug.Log("<><>left removeSwipeUpwardListener<><>");
            SwipeUpwardEvent -= swipeUpward;
        }

        public static void addSwipeDownwardListener(SwipeDownward swipeDownward){
            Debug.Log("<><>left addSwipeDownwardListener<><>");
            SwipeDownwardEvent += swipeDownward;
        }

        public static void removeSwipeDownwardListener(SwipeDownward swipeDownward){
            Debug.Log("<><>left removeSwipeDownwardListener<><>");
            SwipeDownwardEvent -= swipeDownward;
        }

        private static void onSwipeUpward(){
            Debug.Log("<><>left onSwipeUpward<><>");
            Loom.QueueOnMainThread(() =>
            {
                SwipeUpwardEvent();
            });
        }

        private static void onSwipeDownward(){
            Debug.Log("<><>left onSwipeDownward<><>");
            Loom.QueueOnMainThread(() =>
            {
                SwipeDownwardEvent();
            });
        }
        /*
        _______________________________________________________________________________________
         */

         private static event SwipeForward SwipeForwardEvent = delegate(){};
         private static event SwipeBackward SwipeBackwardEvent = delegate(){};

         public static void addSwipeForwardListener(SwipeForward swipeForward){
             Debug.Log("<><>left addSwipeForwardListener<><>");
             SwipeForwardEvent += swipeForward;
         }

         public static void removeSwipeForwardListener(SwipeForward swipeForward){
             Debug.Log("<><>left removeSwipeForwardListener<><>");
             SwipeForwardEvent -= swipeForward;
         }

         private static void onSwipeForward(){
             Debug.Log("<><>left onSwipeForward<><>");
            Loom.QueueOnMainThread(() =>
            {
                SwipeForwardEvent();
            });
         }
         public static void addSwipeBackwardListener(SwipeBackward swipeBackward){
             Debug.Log("<><>left addSwipeBackwardListener<><>");
             SwipeBackwardEvent += swipeBackward;
         }

         public static void removeSwipeBackwardListener(SwipeBackward swipeBackward){
             Debug.Log("<><>left removeSwipeBackwardListener<><>");
             SwipeBackwardEvent -= swipeBackward;
         }

         private static void onSwipeBackward(){
             Debug.Log("<><>left onSwipeBackward<><>");
            Loom.QueueOnMainThread(() =>
            {
                SwipeBackwardEvent();
            });
         }

         /*
        _______________________________________________________________________________________
         */

         private static event SwipeUpAndDown SwipeUpAndDownEvent = delegate(){};

         public static void addSwipeUpAndDownListener(SwipeUpAndDown swipeUpAndDown){
             Debug.Log("<><>left addSwipeUpAndDownListener<><>");
             SwipeUpAndDownEvent += swipeUpAndDown;
         }
         public static void removeSwipeUpAndDownListener(SwipeUpAndDown swipeUpAndDown){
             Debug.Log("<><>left removeSwipeUpAndDownListener<><>");
             SwipeUpAndDownEvent -= swipeUpAndDown;
         }

         private static void onSwipeUpAndDown(){
             Debug.Log("<><>left onSwipeUpAndDown<><>");
            Loom.QueueOnMainThread(() =>
            {
                SwipeUpAndDownEvent();
            });
         }

         /*
        _______________________________________________________________________________________
         */

         private static event SwipeForthAndBack SwipeForthAndBackEvent = delegate(){};

         public static void addSwipeForthAndBackListener(SwipeForthAndBack swipeForthAndBack){
             Debug.Log("<><>left addSwipeForthAndBackListener<><>");
             SwipeForthAndBackEvent += swipeForthAndBack;
         }
         public static void removeSwipeForthAndBackListener(SwipeForthAndBack swipeForthAndBack){
             Debug.Log("<><>left removeSwipeForthAndBackListener<><>");
             SwipeForthAndBackEvent -= swipeForthAndBack;
         }

         private static void onSwipeForthAndBack(){
             Debug.Log("<><>left onSwipeForthAndBack<><>");
            Loom.QueueOnMainThread(() =>
            {
                SwipeForthAndBackEvent();
            });
         }

         /*
        _______________________________________________________________________________________
         */

         private static event Tap TapEvent = delegate(){};

         public static void addTapListener(Tap tap){
             Debug.Log("<><>left addTapListener<><>");
             TapEvent += tap;
         }
         public static void removeTapListener(Tap tap){
             Debug.Log("<><>left removeTapListener<><>");
             TapEvent -= tap;
         }

         private static void onTap(){
             Debug.Log("<><>left onTap<><>");
            Loom.QueueOnMainThread(() =>
            {
                TapEvent();
            });
         }

         /*
        _______________________________________________________________________________________
         */

         private static event DoubleTap DoubleTapEvent = delegate(){};

         public static void addDoubleTapListener(DoubleTap doubleTap){
             Debug.Log("<><>left addDoubleTapListener<><>");
             DoubleTapEvent += doubleTap;
         }
         public static void removeDoubleTapListener(DoubleTap doubleTap){
             Debug.Log("<><>left removeDoubleTapListener<><>");
             DoubleTapEvent -= doubleTap;
         }

         private static void onDoubleTap(){
             Debug.Log("<><>left onDoubleTap<><>");
            Loom.QueueOnMainThread(() =>
            {
                DoubleTapEvent();
            });
         }
    }
    
}