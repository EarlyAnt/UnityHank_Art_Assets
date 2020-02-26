using System.Collections;
using UnityEngine;

/// <summary>
/// 图标呼吸闪烁动效组件
/// </summary>
[RequireComponent(typeof(BubbleView))]
public class FlashIcon : MonoBehaviour
{
    /************************************************属性与变量命名************************************************/
    [SerializeField]
    private BubbleView bubbleView;//导航栏
    [SerializeField, Range(1f, 5f)]
    private float enlarge = 1.2f;//放大幅度
    [SerializeField, Range(0f, 5f)]
    private float duration = 2f;//动效时长
    [SerializeField]
    private AnimationCurve curve;//动画曲线
    private Coroutine effectCoroutine;//动效协程
    /************************************************Unity方法与事件***********************************************/
    private void Awake()
    {
        this.bubbleView.OnItemChange += this.OnItemChanged;
        if (this.bubbleView != null)
            this.OnItemChanged(this.bubbleView.CurrentIcon);
    }
    private void OnDestroy()
    {
        this.bubbleView.OnItemChange -= this.OnItemChanged;
        this.StopAllCoroutines();
    }
    /************************************************自 定 义 方 法************************************************/
    [ContextMenu("设置默认曲线")]
    private void SetDefaultCurve()
    {
        this.curve = new AnimationCurve();
        this.curve.AddKey(0, 0);
        this.curve.AddKey(0.5f, 1);
        this.curve.AddKey(1, 0);
    }
    //当切换图标时
    private void OnItemChanged(RectTransform rectTransform)
    {
        if (this.effectCoroutine != null)
            this.StopCoroutine(this.effectCoroutine);
        if (this.gameObject.activeInHierarchy)
            this.effectCoroutine = this.StartCoroutine(this.PlayEffect(rectTransform));
    }
    //播放动效
    private IEnumerator PlayEffect(RectTransform rectTransform)
    {
        float time = 0;
        float offset = this.enlarge - 1;
        float duration = this.duration == 0 ? 1 : this.duration;
        while (this.gameObject.activeInHierarchy)
        {
            while (time < duration)
            {
                rectTransform.localScale = Vector3.one + (Vector3.one * offset * this.curve.Evaluate(time / duration));
                time += Time.deltaTime;
                yield return null;
            }
            time = 0;
        }
    }
}
