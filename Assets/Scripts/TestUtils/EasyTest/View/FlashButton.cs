using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Hank.EasyTest
{
    /// <summary>
    /// 配饰类型按钮
    /// </summary>
    public class FlashButton : MonoBehaviour
    {
        /************************************************属性与变量命名************************************************/
        [SerializeField]
        private Operations operation;
        [SerializeField]
        private Image borderBox;//边框
        [SerializeField, Range(0f, 10f)]
        private float flashDuration = 0.5f;//闪烁周期
        private Tween tween;
        public Operations Operation { get { return this.operation; } }
        /************************************************Unity方法与事件***********************************************/

        /************************************************自 定 义 方 法************************************************/
        //设置按钮状态
        public void SetStatus(bool selected)
        {
            if (this.tween != null)
                this.tween.Kill();

            this.borderBox.enabled = selected;
            if (selected) this.Flash(0f);
            Debug.LogFormat("<><FlashButton.SetStatus>Button: {0}, Status: {1}", this.operation, selected);
        }
        //边框闪烁
        private void Flash(float endValue)
        {
            this.tween = this.borderBox.DOFade(endValue, this.flashDuration);
            this.tween.onComplete = () => this.Flash(1 - endValue);
        }
        [ContextMenu("设置图片组件")]
        private void FindImage()
        {
            this.borderBox = this.GetComponent<Image>();
        }
    }

    public enum Operations
    {
        Pet = 0,
        Animation = 1,
        Dress = 2,
        Accessory = 3,
        Suit = 4,
        Sprite = 5
    }
}