using UnityEngine;

public class AnimationsSFX : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Play footsteps sound effect
        AudioManager.Instance.PlayFootstepsSoundEffect();
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Stop footsteps sound effect
        AudioManager.Instance.StopFootstepsSoundEffect();
    }
}
