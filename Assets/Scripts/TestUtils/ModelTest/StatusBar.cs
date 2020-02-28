using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace ModelTest
{
    /// <summary>
    /// 状态栏
    /// </summary>
    public class StatusBar : MonoBehaviour
    {
        /************************************************属性与变量命名************************************************/
        public static StatusBar Instance { get; private set; }
        [SerializeField]
        private CanvasGroup rootObject;//根物体
        [SerializeField]
        private Image imageBox;//图边框
        [SerializeField]
        private Text textBox;//文本框
        [SerializeField, Range(0f, 3f)]
        private float linearDuration = 0.5f;//渐变时长
        [SerializeField, Range(0f, 10f)]
        private float remainSeconds = 2.5f;//停留时长
        /************************************************Unity方法与事件***********************************************/
        private void Awake()
        {
            Instance = this;
            this.rootObject.DOFade(0f, 0f);
        }
        /************************************************自 定 义 方 法************************************************/
        //显示文本
        public void ShowMessage(string text)
        {
            this.textBox.enabled = true;
            this.imageBox.enabled = false;
            this.textBox.text = text;
            this.Fade();
        }
        //显示图片
        public void ShowMessage(Sprite image)
        {
            this.textBox.enabled = false;
            this.imageBox.enabled = true;
            this.imageBox.sprite = image;
            this.Fade();
        }
        //淡入淡出
        private void Fade()
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(this.rootObject.DOFade(1f, this.linearDuration));
            sequence.AppendInterval(this.remainSeconds);
            sequence.Append(this.rootObject.DOFade(0f, this.linearDuration));
        }
    }
}
