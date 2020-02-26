using Hank;
using strange.extensions.mediation.impl;
using UnityEngine;
using UnityEngine.Assertions;

namespace Gululu
{
    public class BaseView : EventView
    {
        protected override void Start()
        {
            base.Start();
        }

        protected override void OnDestroy()
        {
            this.StopAllSound(true);
            base.OnDestroy();
        }

        public virtual void TestSerializeField()
        {
            Assert.IsTrue(true);
        }

        public virtual void Open()
        {
        }

        public virtual void Close()
        {
        }

        public virtual void Close(System.Action callback)
        {
            try
            {
                if (callback != null)
                    callback();
            }
            catch (System.Exception ex)
            {
                Debug.LogErrorFormat("<><BaseView.Close>Exception: {0}", ex.Message);
            }
        }

        public virtual void SetViewSnapShot()
        {
        }

        protected virtual void PlaySound(string soundType)
        {
            this.StopAllSound(false);
            SoundPlayer.GetInstance().PlaySoundType(soundType);
        }

        protected virtual void StopAllSound(bool exit)
        {
        }

        protected void SetCanvasLayer(bool front)
        {
            GameObject canvasObject = GameObject.FindGameObjectWithTag("MidCanvas");
            if (canvasObject == null) return;
            Canvas canvas = canvasObject.GetComponent<Canvas>();
            if (canvas == null) return;
            canvas.sortingOrder = front ? 3000 : 3;
        }
    }
}

