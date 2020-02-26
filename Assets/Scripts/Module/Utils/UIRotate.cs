using System.Collections;
using UnityEngine;

/// <summary>
/// UI旋转组件
/// </summary>
public class UIRotate : MonoBehaviour
{
    /************************************************属性与变量命名************************************************/
    [SerializeField]
    private float angle;//旋转角度
    [SerializeField]
    private RectTransform target;//旋转对象
    /************************************************Unity方法与事件***********************************************/
    private void Awake()
    {
        if (this.target == null)
            this.target = this.GetComponent<RectTransform>();
    }
    private void Update()
    {
        if (this.target != null)
            this.target.Rotate(0, 0, this.angle, Space.Self);
    }
    /************************************************自 定 义 方 法************************************************/
}
