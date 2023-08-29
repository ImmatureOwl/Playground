using UnityEngine;

public class Idle : StateMachineBehaviour
{
    private EnemyController ec;
    private Quaternion targetRotation;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (ec == null) ec = animator.gameObject.GetComponent<EnemyController>();
        ec.CurrentState = EnemyState.idle;
        animator.SetFloat("DelayCounter", animator.GetFloat("DetectionDelay"));
        animator.SetFloat("SearchCounter", animator.GetFloat("MaxSearchTime"));
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        targetRotation = ec.DefaultLookingDirection;
        ec.transform.rotation = Quaternion.Slerp(ec.transform.rotation, targetRotation, Time.deltaTime * ec.RotationSpeed);
        animator.SetBool("PlayerVisible", ec.IsTargetVisible);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
