using Spine.Unity;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PopEffect : MonoBehaviour
{
    /************************************************属性与变量命名************************************************/
    [SerializeField]
    private bool autoPlay = true;
    [SerializeField]
    private AnimationCurve curve;
    [SerializeField, Range(0.01f, 10f)]
    private float rangeRate = 1;
    [SerializeField, Range(0.01f, 100f)]
    private float speedRate = 1;
    /************************************************Unity方法与事件***********************************************/
    void Start()
    {
    }
    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Space))
        {
            this.StopAllCoroutines();
            this.StartCoroutine(this.Play());
        }
#endif
    }
    void OnEnable()
    {
        if (this.autoPlay)
        {
            this.StopAllCoroutines();
            this.StartCoroutine(this.Play());
        }
    }
    /************************************************自 定 义 方 法************************************************/
    IEnumerator Play()
    {
        RectTransform rectTransform = this.GetComponent<RectTransform>();
        if (rectTransform == null) yield break;

        float time = 0;
        while (time < this.speedRate)
        {
            rectTransform.localScale = Vector3.one * this.GetEvaluate(time);
            time += Time.deltaTime * this.speedRate;
            yield return null;
        }
    }
    float GetEvaluate(float time)
    {
        float evaluate = this.curve.Evaluate(time);
        evaluate = 1 - (1 - evaluate) * this.rangeRate;
        return evaluate;
    }
    [ContextMenu("预设效果1: 小大")]
    void DefaultEffect_1()
    {
        this.curve = new AnimationCurve();
        this.curve.AddKey(new Keyframe() { time = 0f, value = 1f });
        this.curve.AddKey(new Keyframe() { time = 0.5f, value = 0.8f });
        this.curve.AddKey(new Keyframe() { time = 1f, value = 1f });
    }
    [ContextMenu("预设效果2: 大小大")]
    void DefaultEffect_2()
    {
        this.curve = new AnimationCurve();
        this.curve.AddKey(new Keyframe() { time = 0f, value = 1f });
        this.curve.AddKey(new Keyframe() { time = 0.2f, value = 1.1f });
        this.curve.AddKey(new Keyframe() { time = 0.4f, value = 0.8f });
        this.curve.AddKey(new Keyframe() { time = 0.6f, value = 1.05f });
        this.curve.AddKey(new Keyframe() { time = 0.8f, value = 0.9f });
        this.curve.AddKey(new Keyframe() { time = 1f, value = 1f });
    }
}