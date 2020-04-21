using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateBehaviourTest : MonoBehaviour
{
    void Start()
    {
        Animator animator = this.GetComponent<Animator>();
        StateMachineBehaviour[] stateMachineBehaviours = animator.GetBehaviours<StateMachineBehaviour>();
        Debug.LogFormat("<><StateBehaviourTest.Start>stateMachineBehaviours: {0}", stateMachineBehaviours.Length);
        AnimationStateListener[] animationStateListeners = animator.GetBehaviours<AnimationStateListener>();
        Debug.LogFormat("<><StateBehaviourTest.Start>animationStateListeners: {0}", animationStateListeners.Length);
    }
}
