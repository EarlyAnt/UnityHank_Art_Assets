using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 页面背景快照操作工具
/// </summary>
public class ViewSnapShot : MonoBehaviour
{
    /************************************************属性与变量命名************************************************/
    public enum Directions { Up = 0, Down = 1 }
    public static ViewSnapShot Instance { get; private set; }
    [SerializeField]
    private Image imageBox;
    [SerializeField, Range(0f, 5f)]
    private float moveDuration = 1f;
    private RectTransform rectTransform;
    private RectTransform RectTransform
    {
        get
        {
            if (this.rectTransform == null)
                this.rectTransform = this.imageBox.GetComponent<RectTransform>();
            return this.rectTransform;
        }
    }
    /************************************************Unity方法与事件***********************************************/
    private void Awake()
    {
        Instance = this;
    }
    /************************************************自 定 义 方 法************************************************/
    public void SetViewSnapShot(Sprite backgroundImage, Directions direction)
    {
        this.RectTransform.DOLocalMoveY(0f, 0f);
        this.imageBox.enabled = false;
        this.imageBox.sprite = backgroundImage;        
        this.imageBox.enabled = true;
        if (direction == Directions.Up)
        {
            this.RectTransform.DOLocalMoveY(320f, this.moveDuration).onComplete = () =>
            {
                this.imageBox.enabled = false;
                this.imageBox.sprite = null;
                Resources.UnloadUnusedAssets();
            };
        }
        else
        {            
            this.RectTransform.DOLocalMoveY(-320f, this.moveDuration).onComplete = () =>
            {
                this.imageBox.enabled = false;
                this.imageBox.sprite = null;
                Resources.UnloadUnusedAssets();
            };
        }
    }
}