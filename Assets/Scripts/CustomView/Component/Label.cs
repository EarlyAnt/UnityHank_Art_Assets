using DG.Tweening;
using Gululu.Util;
using System;
using UnityEngine;
using UnityEngine.UI;

//配饰图标上显示的标签
[Serializable]
public abstract class Label
{
    [SerializeField]
    protected Image image;
    private RectTransform imageTransform;
    private Tween tween;
    private float sizeRate;
    private float lerpTime;
    public bool Visible { get; private set; }

    public void Initialize(float sizeRate, float lerpSpeed)
    {
        this.sizeRate = sizeRate;
        this.lerpTime = lerpSpeed;
    }

    public void SetStatus(bool visible, bool parent = false)
    {
        this.Visible = visible;
        if (this.image != null)
        {
            if (parent && this.image.transform.parent != null)
                this.image.transform.parent.gameObject.SetActive(visible);
            else if (!parent)
                this.image.gameObject.SetActive(visible);
        }
    }

    public void SetScale(Vector3 scale, bool immediately = false)
    {
        if (this.imageTransform == null)
            this.imageTransform = this.image.GetComponent<RectTransform>();

        if (this.tween != null)
            this.tween.Kill();

        if (immediately)
            this.tween = this.imageTransform.DOScale(scale, this.lerpTime);
        else
            this.imageTransform.localScale = scale;
    }

    public void SetIcon(Sprite iconSprite)
    {
        if (this.image != null)
            this.image.sprite = iconSprite;
    }
}
//配饰图标上显示的标签
[Serializable]
public class TextLabel : Label
{
    [SerializeField]
    private Text text;
    public Text Text
    {
        get { return this.text; }
    }

    public void SetText(string content)
    {
        if (this.text != null)
            this.text.text = content;
    }
}
//配饰图标上显示的标签
[Serializable]
public class ImageLabel : Label
{
    [SerializeField]
    private Image text;

    public void SetText(Sprite textSprite)
    {
        if (this.text != null)
            this.text.sprite = textSprite;
    }
}