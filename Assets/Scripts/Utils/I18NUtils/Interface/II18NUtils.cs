﻿using Spine.Unity;
using UnityEngine.UI;

public interface II18NUtils
{
    void SetAnimation(SkeletonGraphic spine, string animationName, bool loop = true);
    void SetAnimation(SkeletonGraphic spine, string animationName, string defaultAnimation, bool loop = true);
    void SetText(Text textComponent, string text, bool setFont = false);
    void SetText(Text textComponent, string text, bool setFont = false, params object[] param);
    void SetFont(Text textComponent, string text);
}