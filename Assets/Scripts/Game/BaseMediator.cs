using Cup.Utils.CupMotion;
using Cup.Utils.Touch;
using Gululu.Events;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using UnityEngine;
using UnityEngine.Assertions;

namespace Gululu
{
    /// <summary>
    /// 控制器基类
    /// </summary>
    public class BaseMediator : EventMediator
    {
        private static List<MainKeys> lockKeys = new List<MainKeys>();
        /// <summary>
        /// 是否注册View
        /// </summary>
        protected bool registerView = true;
        /// <summary>
        /// 视图枚举
        /// </summary>
        public TopViewHelper.ETopView View { get; protected set; }
        /// <summary>
        /// 是否已注册过
        /// </summary>
        protected bool Registered { get; private set; }
        /// <summary>
        /// 是否已注册过输入监听
        /// </summary>
        protected bool InputRegistered { get; private set; }
        /// <summary>
        /// 控制器注册(入口)
        /// </summary>
        public override void OnRegister()
        {
            base.OnRegister();
            this.RegisterInput();
            TopViewHelper.Instance.AddListener(this.OnTopViewChanged);
            if (registerView)
                TopViewHelper.Instance.AddTopView(this.View);
            FlurryUtil.LogTimeEvent(this.View);
            this.dispatcher.UpdateListener(true, MainEvent.CloseView, this.OnReceiveCloseMessage);
            this.Registered = true;
        }
        /// <summary>
        /// 控制器取消注册(出口)
        /// </summary>
        public override void OnRemove()
        {
            base.OnRemove();
            this.UnregisterInput();
            if (this.registerView)
                TopViewHelper.Instance.RemoveView(this.View);
            TopViewHelper.Instance.RemoveListener(this.OnTopViewChanged);
            FlurryUtil.EndTimedEvent(this.View);
            this.dispatcher.UpdateListener(false, MainEvent.CloseView, this.OnReceiveCloseMessage);
            this.Registered = false;
        }
        /// <summary>
        /// 注册输入
        /// </summary>
        protected virtual void RegisterInput()
        {
            if (this.InputRegistered) return;
#if !UNITY_EDITOR && UNITY_ANDROID
            CupMotionManager.start();
            CupMotionManager.addShakeListener(this._CupShake);//摇晃
            CupMotionManager.addPoseChangeListener(this._CupPoseChanged);//姿势
            CupMotionManager.addTiltStatusChangeEventListener(this._CupTiltStatusChanged);//倾斜            

            CupKeyEventHookStaticManager.addPowerKeyPressCallBack(this._PowerClick);
            if (!this.Registered)
                CupKeyEventHookStaticManager.addPowerKeyLongPressCallBack(this._PowerHold);

            TouchLeftStaticManager.start();
            TouchLeftStaticManager.addTapListener(this._CupTapLeft);
            TouchLeftStaticManager.addDoubleTapListener(this._CupDoubleTapLeft);
            TouchLeftStaticManager.addSwipeUpwardListener(this._CupLeftSwipeUp);
            TouchLeftStaticManager.addSwipeDownwardListener(this._CupLeftSwipeDown);
            TouchLeftStaticManager.addSwipeUpAndDownListener(this._CupLeftSwipeUpDown);

            TouchRightStaticManager.start();
            TouchRightStaticManager.addTapListener(this._CupTapRight);
            TouchRightStaticManager.addDoubleTapListener(this._CupDoubleTapRight);
            TouchRightStaticManager.addSwipeUpwardListener(this._CupRightSwipeUp);
            TouchRightStaticManager.addSwipeDownwardListener(this._CupRightSwipeDown);
            TouchRightStaticManager.addSwipeUpAndDownListener(this._CupRightSwipeUpDown);
#else
            dispatcher.UpdateListener(true, MainKeys.PowerClick, this._PowerClick);
            if (!this.Registered)
                dispatcher.UpdateListener(true, MainKeys.PowerHold, this._PowerHold);
            dispatcher.UpdateListener(true, MainKeys.CupShake, this._CupShake);
            dispatcher.UpdateListener(true, MainKeys.CupTiltLeft, this._CupTiltLeft);
            dispatcher.UpdateListener(true, MainKeys.CupTiltRight, this._CupTiltRight);
            dispatcher.UpdateListener(true, MainKeys.CupKeepFlat, this._CupKeepFlat);
            dispatcher.UpdateListener(true, MainKeys.CupTapLeft, this._CupTapLeft);
            dispatcher.UpdateListener(true, MainKeys.CupTapRight, this._CupTapRight);
            dispatcher.UpdateListener(true, MainKeys.CupDoubleTapLeft, this._CupDoubleTapLeft);
            dispatcher.UpdateListener(true, MainKeys.CupDoubleTapRight, this._CupDoubleTapRight);
            dispatcher.UpdateListener(true, MainKeys.CupLeftSwipeUp, this._CupLeftSwipeUp);
            dispatcher.UpdateListener(true, MainKeys.CupLeftSwipeDown, this._CupLeftSwipeDown);
            dispatcher.UpdateListener(true, MainKeys.CupLeftSwipeUpDown, this._CupLeftSwipeUpDown);
            dispatcher.UpdateListener(true, MainKeys.CupRightSwipeUp, this._CupRightSwipeUp);
            dispatcher.UpdateListener(true, MainKeys.CupRightSwipeDown, this._CupRightSwipeDown);
            dispatcher.UpdateListener(true, MainKeys.CupRightSwipeUpDown, this._CupRightSwipeUpDown);
#endif
            this.InputRegistered = true;
        }
        /// <summary>
        /// 取消注册输入
        /// </summary>
        protected virtual void UnregisterInput()
        {
#if !UNITY_EDITOR && UNITY_ANDROID
            CupMotionManager.removeShakeListener(this._CupShake);//摇晃
            CupMotionManager.removePoseChangeListener(this._CupPoseChanged);//姿势
            CupMotionManager.removeTiltStatusChangeEventListener(this._CupTiltStatusChanged);//倾斜            

            CupKeyEventHookStaticManager.removePowerKeyPressCallBack(this._PowerClick);
            if (this.View != TopViewHelper.ETopView.eMainView)
                CupKeyEventHookStaticManager.removePowerKeyLongPressCallBack(this._PowerHold);

            TouchLeftStaticManager.removeTapListener(this._CupTapLeft);
            TouchLeftStaticManager.removeDoubleTapListener(this._CupDoubleTapLeft);
            TouchLeftStaticManager.removeSwipeUpwardListener(this._CupLeftSwipeUp);
            TouchLeftStaticManager.removeSwipeDownwardListener(this._CupLeftSwipeDown);
            TouchLeftStaticManager.removeSwipeUpAndDownListener(this._CupLeftSwipeUpDown);

            TouchRightStaticManager.removeTapListener(this._CupTapRight);
            TouchRightStaticManager.removeDoubleTapListener(this._CupDoubleTapRight);
            TouchRightStaticManager.removeSwipeUpwardListener(this._CupRightSwipeUp);
            TouchRightStaticManager.removeSwipeDownwardListener(this._CupRightSwipeDown);
            TouchRightStaticManager.removeSwipeUpAndDownListener(this._CupRightSwipeUpDown);
#else
            dispatcher.UpdateListener(false, MainKeys.PowerClick, this._PowerClick);
            if (this.View != TopViewHelper.ETopView.eMainView)
                dispatcher.UpdateListener(false, MainKeys.PowerHold, this._PowerHold);
            dispatcher.UpdateListener(false, MainKeys.CupShake, this._CupShake);
            dispatcher.UpdateListener(false, MainKeys.CupTiltLeft, this._CupTiltLeft);
            dispatcher.UpdateListener(false, MainKeys.CupTiltRight, this._CupTiltRight);
            dispatcher.UpdateListener(false, MainKeys.CupKeepFlat, this._CupKeepFlat);
            dispatcher.UpdateListener(false, MainKeys.CupTapLeft, this._CupTapLeft);
            dispatcher.UpdateListener(false, MainKeys.CupTapRight, this._CupTapRight);
            dispatcher.UpdateListener(false, MainKeys.CupDoubleTapLeft, this._CupDoubleTapLeft);
            dispatcher.UpdateListener(false, MainKeys.CupDoubleTapRight, this._CupDoubleTapRight);
            dispatcher.UpdateListener(false, MainKeys.CupLeftSwipeUp, this._CupLeftSwipeUp);
            dispatcher.UpdateListener(false, MainKeys.CupLeftSwipeDown, this._CupLeftSwipeDown);
            dispatcher.UpdateListener(false, MainKeys.CupLeftSwipeUpDown, this._CupLeftSwipeUpDown);
            dispatcher.UpdateListener(false, MainKeys.CupRightSwipeUp, this._CupRightSwipeUp);
            dispatcher.UpdateListener(false, MainKeys.CupRightSwipeDown, this._CupRightSwipeDown);
            dispatcher.UpdateListener(false, MainKeys.CupRightSwipeUpDown, this._CupRightSwipeUpDown);
#endif
            this.InputRegistered = false;
        }
        /// <summary>
        /// 判断某个键的操作是否被锁定
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        private static bool IsLockedKey(MainKeys key)
        {
            if (lockKeys != null && lockKeys.Contains(key))
                return true;
            else
                return false;
        }
        /// <summary>
        /// 锁定除了参数中的键之外的所有键的操作
        /// </summary>
        /// <param name="ignoreKeys"></param>
        public static void LockKeysIgnore(List<MainKeys> ignoreKeys)
        {
            if (ignoreKeys != null && ignoreKeys.Count > 0)
            {
                lockKeys = new List<MainKeys>();
                foreach (MainKeys key in Enum.GetValues(typeof(MainKeys)))
                {
                    if (!ignoreKeys.Contains(key))
                        lockKeys.Add(key);
                }
                //PrintAllLockKeys();
            }
            else
            {
                Debug.LogError("<><BaseMediator.LockKeysIgnore>Parameter error, ignoreKeys is null or empty");
            }
        }
        /// <summary>
        /// 解锁所有键的操作
        /// </summary>
        public static void UnlockAllKeys()
        {
            if (lockKeys != null)
                lockKeys.Clear();
        }
        /// <summary>
        /// 打印所有锁定的键
        /// </summary>
        public static void PrintAllLockKeys()
        {
            if (lockKeys != null && lockKeys.Count > 0)
            {
                StringBuilder strbKeys = new StringBuilder();
                foreach (MainKeys key in lockKeys)
                {
                    strbKeys.AppendLine(string.Format("LockKey: {0}", key));
                }
                Debug.LogFormat("<><BaseMediator.PrintAllLockKeys>\n{0}", strbKeys.ToString());
            }
            else Debug.Log("<><BaseMediator.PrintAllLockKeys>None key is locked");
        }
        /// <summary>
        /// 当收到关闭页面的消息时
        /// </summary>
        /// <param name="evt"></param>
        private void OnReceiveCloseMessage(IEvent evt)
        {
            if (evt != null && evt.data != null && (TopViewHelper.ETopView)evt.data == this.View)
                this.OnClose(false);
        }
        /// <summary>
        /// 当关闭页面时
        /// </summary>
        /// <param name="evt">消息参数</param>
        protected virtual void OnClose(bool initiative)
        {
        }

        #region 父类方法
        /// <summary>
        /// 电源键单击
        /// </summary>
        private void _PowerClick() { if (!IsLockedKey(MainKeys.PowerClick)) { FlurryUtil.LogEvent(this.View, MainKeys.PowerClick); this.PowerClick(); } }
        /// <summary>
        /// 电源键长按
        /// </summary>
        private void _PowerHold() { if (!IsLockedKey(MainKeys.PowerHold)) { FlurryUtil.LogEvent(this.View, MainKeys.PowerHold); this.PowerHold(); } }
        /// <summary>
        /// 水杯姿势变化
        /// </summary>
        private void _CupPoseChanged(IEvent message)
        {
            if (message == null || message.data == null)
            {
                Debug.LogError("BaseMediator received a message of Event, but it or it's data is null.");
                return;
            }
            else
            {
                int temp = 0;
                if (!int.TryParse(message.data.ToString(), out temp))
                {
                    Debug.LogError("BaseMediator received a message of Event, but it's data is not the type of int.");
                }
            }
            this._CupPoseChanged((int)message.data);
            Debug.LogFormat("===========================BaseMediator.CupPoseChanged{0}==============================", (int)message.data);
        }
        /// <summary>
        /// 水杯姿势变化
        /// </summary>
        private void _CupPoseChanged(int pose) { this.CupPoseChanged(pose); }
        /// <summary>
        /// 水杯姿势变化
        /// </summary>
        private void _CupTiltStatusChanged(IEvent message)
        {
            if (message == null || message.data == null)
            {
                Debug.LogError("BaseMediator received a message of Event, but it or it's data is null.");
                return;
            }
            else
            {
                int temp = 0;
                if (!int.TryParse(message.data.ToString(), out temp))
                {
                    Debug.LogError("BaseMediator received a message of Event, but it's data is not the type of int.");
                }
            }
            this._CupTiltStatusChanged((int)message.data);
            Debug.LogFormat("===========================BaseMediator.CupTiltStatusChanged{0}==============================", (int)message.data);
        }
        /// <summary>
        /// 水杯姿势变化
        /// </summary>
        private void _CupTiltStatusChanged(int pose)
        {
            switch (pose)
            {
                case 6:
                    this._CupTiltLeft();
                    break;
                case 7:
                    this._CupTiltRight();
                    break;
                case 8:
                    this._CupKeepFlat();
                    break;
            }
        }
        /// <summary>
        /// 水杯左倾
        /// </summary>
        private void _CupTiltLeft() { if (!IsLockedKey(MainKeys.CupTiltLeft)) { FlurryUtil.LogEvent(this.View, MainKeys.CupTiltLeft); this.CupTiltLeft(); } }
        /// <summary>
        /// 水杯右倾
        /// </summary>
        private void _CupTiltRight() { if (!IsLockedKey(MainKeys.CupTiltRight)) { FlurryUtil.LogEvent(this.View, MainKeys.CupTiltRight); this.CupTiltRight(); } }
        /// <summary>
        /// 水杯平放
        /// </summary>
        private void _CupKeepFlat() { if (!IsLockedKey(MainKeys.CupKeepFlat)) { this.CupKeepFlat(); } }
        /// <summary>
        /// 水杯摇晃
        /// </summary>
        private void _CupShake() { if (!IsLockedKey(MainKeys.CupShake)) { FlurryUtil.LogEvent(this.View, MainKeys.CupShake); this.CupShake(); } }
        /// <summary>
        /// 水杯左侧拍一下
        /// </summary>
        private void _CupTapLeft() { if (!IsLockedKey(MainKeys.CupTapLeft)) { FlurryUtil.LogEvent(this.View, MainKeys.CupTapLeft); this.CupTapLeft(); } }
        /// <summary>
        /// 水杯右侧拍一下
        /// </summary>
        private void _CupTapRight() { if (!IsLockedKey(MainKeys.CupTapRight)) { FlurryUtil.LogEvent(this.View, MainKeys.CupTapRight); this.CupTapRight(); } }
        /// <summary>
        /// 水杯左侧拍两下
        /// </summary>
        private void _CupDoubleTapLeft() { if (!IsLockedKey(MainKeys.CupDoubleTapLeft)) { FlurryUtil.LogEvent(this.View, MainKeys.CupDoubleTapLeft); this.CupDoubleTapLeft(); } }
        /// <summary>
        /// 水杯右侧拍两下
        /// </summary>
        private void _CupDoubleTapRight() { if (!IsLockedKey(MainKeys.CupDoubleTapRight)) { FlurryUtil.LogEvent(this.View, MainKeys.CupDoubleTapRight); this.CupDoubleTapRight(); } }
        /// <summary>
        /// 水杯左侧上划
        /// </summary>
        private void _CupLeftSwipeUp() { if (!IsLockedKey(MainKeys.CupLeftSwipeUp)) { FlurryUtil.LogEvent(this.View, MainKeys.CupLeftSwipeUp); this.CupLeftSwipeUp(); } }
        /// <summary>
        /// 水杯左侧下划
        /// </summary>
        private void _CupLeftSwipeDown() { if (!IsLockedKey(MainKeys.CupLeftSwipeDown)) { FlurryUtil.LogEvent(this.View, MainKeys.CupLeftSwipeDown); this.CupLeftSwipeDown(); } }
        /// <summary>
        /// 水杯左侧上下来回滑动
        /// </summary>
        private void _CupLeftSwipeUpDown() { if (!IsLockedKey(MainKeys.CupLeftSwipeUpDown)) { FlurryUtil.LogEvent(this.View, MainKeys.CupLeftSwipeUpDown); this.CupLeftSwipeUpDown(); } }
        /// <summary>
        /// 水杯右侧上划
        /// </summary>
        private void _CupRightSwipeUp() { if (!IsLockedKey(MainKeys.CupRightSwipeUp)) { FlurryUtil.LogEvent(this.View, MainKeys.CupRightSwipeUp); this.CupRightSwipeUp(); } }
        /// <summary>
        /// 水杯右侧下划
        /// </summary>
        private void _CupRightSwipeDown() { if (!IsLockedKey(MainKeys.CupRightSwipeDown)) { FlurryUtil.LogEvent(this.View, MainKeys.CupRightSwipeDown); this.CupRightSwipeDown(); } }
        /// <summary>
        /// 水杯右侧上下来回滑动
        /// </summary>
        private void _CupRightSwipeUpDown() { if (!IsLockedKey(MainKeys.CupRightSwipeUpDown)) { FlurryUtil.LogEvent(this.View, MainKeys.CupRightSwipeUpDown); this.CupRightSwipeUpDown(); } }
        #endregion

        #region 子类重写方法
        /// <summary>
        /// 电源键单击
        /// </summary>
        protected virtual void PowerClick() { }
        /// <summary>
        /// 电源键长按
        /// </summary>
        protected virtual void PowerHold() { }
        /// <summary>
        /// 水杯姿势变化
        /// </summary>
        protected virtual void CupPoseChanged(int pose)
        {
        }
        /// <summary>
        /// 水杯左倾
        /// </summary>
        protected virtual void CupTiltLeft() { }
        /// <summary>
        /// 水杯右倾
        /// </summary>
        protected virtual void CupTiltRight() { }
        /// <summary>
        /// 水杯平放
        /// </summary>
        protected virtual void CupKeepFlat() { }
        /// <summary>
        /// 水杯摇晃
        /// </summary>
        protected virtual void CupShake() { }
        /// <summary>
        /// 水杯左侧拍一下
        /// </summary>
        protected virtual void CupTapLeft() { }
        /// <summary>
        /// 水杯右侧拍一下
        /// </summary>
        protected virtual void CupTapRight() { }
        /// <summary>
        /// 水杯左侧拍两下
        /// </summary>
        protected virtual void CupDoubleTapLeft() { }
        /// <summary>
        /// 水杯右侧拍两下
        /// </summary>
        protected virtual void CupDoubleTapRight() { }
        /// <summary>
        /// 水杯左侧上划
        /// </summary>
        protected virtual void CupLeftSwipeUp() { }
        /// <summary>
        /// 水杯左侧下划
        /// </summary>
        protected virtual void CupLeftSwipeDown() { }
        /// <summary>
        /// 水杯左侧上下来回滑动
        /// </summary>
        protected virtual void CupLeftSwipeUpDown() { }
        /// <summary>
        /// 水杯右侧上划
        /// </summary>
        protected virtual void CupRightSwipeUp() { }
        /// <summary>
        /// 水杯右侧下划
        /// </summary>
        protected virtual void CupRightSwipeDown() { }
        /// <summary>
        /// 水杯右侧上下来回滑动
        /// </summary>
        protected virtual void CupRightSwipeUpDown() { }
        /// <summary>
        /// 当TopView发生改变时(暂停和回复对输入信号的接收)
        /// </summary>
        /// <param name="topView"></param>
        protected virtual void OnTopViewChanged(string operate, TopViewHelper.ETopView topView)
        {
            UnityEngine.Debug.LogFormat("<><BaseMediator.OnTopViewChanged>View: {0}, Operate: {1}, TopView: {2}", this.View, operate, topView);
            if (this.View == TopViewHelper.ETopView.eMainMenu || (topView == this.View && !TopViewHelper.Instance.IsOpenedView(TopViewHelper.ETopView.eMainMenu)))
                this.RegisterInput();
            else if (this.View != topView || TopViewHelper.Instance.IsOpenedView(TopViewHelper.ETopView.eMainMenu))
                this.UnregisterInput();
        }
        #endregion
    }
}

