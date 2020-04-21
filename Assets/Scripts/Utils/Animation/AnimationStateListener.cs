using UnityEngine;

public class AnimationStateListener : StateMachineBehaviour
{
    [SerializeField]
    private string animationName;
    public string AnimationName { get { return this.animationName; } }
    [SerializeField]
    private bool exitAnimation;
    public bool ExitAnimation { get { return this.exitAnimation; } }
    public System.Action<string, bool> AnimationExit;
    public System.Action<Animator, AnimatorStateInfo, int> AnimationStateExit;

    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    if (stateInfo.length == stateInfo.normalizedTime)
    //        Debug.LogFormat("<><AnimationStateListener.OnStateUpdate>stateInfo.normalizedTime: {0}, stateInfo.length: {1}", stateInfo.normalizedTime, stateInfo.length);
    //}

    //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.LogFormat("<><AnimationStateListener.OnStateExit>stateInfo.normalizedTime: {0}, stateInfo.length: {1}", stateInfo.normalizedTime, stateInfo.length);

        if (this.AnimationExit != null)
        {
            Debug.LogWarningFormat("<><AnimationStateListener.OnStateExit>AnimationExit, animationName: {0}, exitAnimation: {1}", this.animationName, this.exitAnimation);
            this.AnimationExit(this.animationName, this.exitAnimation);
        }

        if (this.AnimationStateExit != null)
        {
            Debug.LogWarningFormat("<><AnimationStateListener.OnStateExit>AnimationStateExit, animator: {0}, stateInfo: {1}, layerIndex: {2}", animator, stateInfo, layerIndex);
            this.AnimationStateExit(animator, stateInfo, layerIndex);
        }
    }

    //OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    Debug.LogFormat("<><AnimationStateListener.OnStateMove>stateInfo.normalizedTime: {0}, stateInfo.length: {1}", stateInfo.normalizedTime, stateInfo.length);
    //}

    //OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}
}
