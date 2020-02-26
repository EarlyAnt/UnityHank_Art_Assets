using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorTest : MonoBehaviour
{
    public Animator Animator;
    public List<AnimationConfig> AnimationConfigs;

    void Start()
    {
    }

    void Update()
    {
        foreach (AnimationConfig config in this.AnimationConfigs)
        {
            if (Input.GetKeyDown(config.KeyCode))
            {
                this.Animator.Play(config.Animation);
                //this.Animator.CrossFade(config.Animation, 0.2f);
            }
        }
    }
}

[System.Serializable]
public class AnimationConfig
{
    public KeyCode KeyCode;
    public string Animation;
}
