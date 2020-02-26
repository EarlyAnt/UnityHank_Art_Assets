using CupSdk.BaseSdk.Touch;
using UnityEngine;

namespace Cup.Utils.Touch
{
    public class TouchRightStaticManager
    {
        private static TouchListenerManager rightTouchListenerManager = new TouchListenerManager();

        private static event SwipeUpward SwipeUpwardEvent = delegate(){};
        private static event SwipeDownward SwipeDownwardEvent = delegate(){};

        private static bool isStart = false;

        
        
        public static void start(){
            if(!isStart){
                rightTouchListenerManager.setGestureListener(onSwipeUpward,onSwipeDownward);
                rightTouchListenerManager.setExtraGestureListener(onSwipeForward,onSwipeBackward);
                rightTouchListenerManager.setSwipeUpAndDownListener(onSwipeUpAndDown);
                rightTouchListenerManager.setSwipeForthAndBackListener(onSwipeForthAndBack);
                rightTouchListenerManager.setTapListener(onTap);
                rightTouchListenerManager.setDoubleTapListener(onDoubleTap);
                TouchManager.setRightTouchListenerManager(rightTouchListenerManager.getManager());

                isStart = true;
            }
            
        }

        /*
        _______________________________________________________________________________________
         */

        public static void addSwipeUpwardListener(SwipeUpward swipeUpward){
            Debug.Log("<><>right addSwipeUpwardListener<><>");
            SwipeUpwardEvent += swipeUpward;
        }

        public static void removeSwipeUpwardListener(SwipeUpward swipeUpward){
            Debug.Log("<><>right removeSwipeUpwardListener<><>");
            SwipeUpwardEvent -= swipeUpward;
        }

        public static void addSwipeDownwardListener(SwipeDownward swipeDownward){
            Debug.Log("<><>right addSwipeDownwardListener<><>");
            SwipeDownwardEvent += swipeDownward;
        }

        public static void removeSwipeDownwardListener(SwipeDownward swipeDownward){
            Debug.Log("<><>right removeSwipeDownwardListener<><>");
            SwipeDownwardEvent -= swipeDownward;
        }

        private static void onSwipeUpward(){
            Debug.Log("<><>right onSwipeUpward<><>");
            Loom.QueueOnMainThread(() =>
            {
                SwipeUpwardEvent();
            });
        }

        private static void onSwipeDownward(){
            Debug.Log("<><>right onSwipeDownward<><>");
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
             Debug.Log("<><>right addSwipeForwardListener<><>");
             SwipeForwardEvent += swipeForward;
         }

         public static void removeSwipeForwardListener(SwipeForward swipeForward){
             Debug.Log("<><>right removeSwipeForwardListener<><>");
             SwipeForwardEvent -= swipeForward;
         }

         private static void onSwipeForward(){
             Debug.Log("<><>right onSwipeForward<><>");
            Loom.QueueOnMainThread(() =>
            {
                SwipeForwardEvent();
            });
         }
         public static void addSwipeBackwardListener(SwipeBackward swipeBackward){
             Debug.Log("<><>right addSwipeBackwardListener<><>");
             SwipeBackwardEvent += swipeBackward;
         }

         public static void removeSwipeBackwardListener(SwipeBackward swipeBackward){
             Debug.Log("<><>right removeSwipeBackwardListener<><>");
             SwipeBackwardEvent -= swipeBackward;
         }

         private static void onSwipeBackward(){
             Debug.Log("<><>right onSwipeBackward<><>");
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
             Debug.Log("<><>right addSwipeUpAndDownListener<><>");
             SwipeUpAndDownEvent += swipeUpAndDown;
         }
         public static void removeSwipeUpAndDownListener(SwipeUpAndDown swipeUpAndDown){
             Debug.Log("<><>right removeSwipeUpAndDownListener<><>");
             SwipeUpAndDownEvent -= swipeUpAndDown;
         }

         private static void onSwipeUpAndDown(){
             Debug.Log("<><>onSwipeUpAndDown<><>");
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
             Debug.Log("<><>right addSwipeForthAndBackListener<><>");
             SwipeForthAndBackEvent += swipeForthAndBack;
         }
         public static void removeSwipeForthAndBackListener(SwipeForthAndBack swipeForthAndBack){
             Debug.Log("<><>right removeSwipeForthAndBackListener<><>");
             SwipeForthAndBackEvent -= swipeForthAndBack;
         }

         private static void onSwipeForthAndBack(){
             Debug.Log("<><>right onSwipeForthAndBack<><>");
            Loom.QueueOnMainThread(() =>
            {
                SwipeForthAndBackEvent();
            } );
         }

         /*
        _______________________________________________________________________________________
         */

         private static event Tap TapEvent = delegate(){};

         public static void addTapListener(Tap tap){
             Debug.Log("<><>right addTapListener<><>");
             TapEvent += tap;
         }
         public static void removeTapListener(Tap tap){
             Debug.Log("<><>right removeTapListener<><>");
             TapEvent -= tap;
         }

         private static void onTap(){
             Debug.Log("<><>right onTap<><>");
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
             Debug.Log("<><>right addDoubleTapListener<><>");
             DoubleTapEvent += doubleTap;
         }
         public static void removeDoubleTapListener(DoubleTap doubleTap){
             Debug.Log("<><>right removeDoubleTapListener<><>");
             DoubleTapEvent -= doubleTap;
         }

         private static void onDoubleTap(){
             Debug.Log("<><>right onDoubleTap<><>");
            Loom.QueueOnMainThread(() =>
            {
                DoubleTapEvent();
            });
         }
    }
}