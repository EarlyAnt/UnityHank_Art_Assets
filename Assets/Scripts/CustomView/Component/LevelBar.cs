using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 等级框
/// </summary>
public class LevelBar : MonoBehaviour
{
    /************************************************属性与变量命名************************************************/
    [SerializeField]
    private Image frame;
    [SerializeField]
    private Image exp;
    [SerializeField]
    private Text level;
    [SerializeField]
    private List<LevelFrame> petLevelFrames;
    /************************************************Unity方法与事件***********************************************/

    /************************************************自 定 义 方 法************************************************/
    //设置等级边框
    public void SetFrame(string petName)
    {
        if (!string.IsNullOrEmpty(petName))
        {
            LevelFrame levelFrame = this.petLevelFrames.Find(t => t.PetName.ToUpper() == petName.ToUpper());
            if (levelFrame != null)
            {
                this.frame.sprite = levelFrame.FrameSprite;
            }
            else
            {
                Debug.LogErrorFormat("<><LevelBar.SetFrame>Can't find the pet frame, pet name is {0}", petName);
            }
        }
        else
        {
            Debug.LogErrorFormat("<><LevelBar.SetFrame>Parameter 'petName' is null");
        }
    }
    //设置经验
    public void SetExp(float exp)
    {
        exp = Mathf.Clamp(exp, 0, 1);
        this.exp.DOFillAmount(exp, 1);
    }
    //设置等级数值
    public void SetLevel(int level)
    {
        if (level >= 1)
        {
            this.level.text = level.ToString();
        }
    }
}

[Serializable]
public class LevelFrame
{
    public string PetName;
    public Sprite FrameSprite;
}