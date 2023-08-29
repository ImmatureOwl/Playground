using UnityEngine;

public class MovingOn : StateMachineBehaviour
{
    private EnemyController ec;
    private Vector3 targetPos;
    private Quaternion targetRotation;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (ec == null) ec = animator.gameObject.GetComponent<EnemyController>();
        targetPos = ec.CurrentPosition;
        ec.CurrentState = EnemyState.moving_on;
        ec.NMAgent.speed = ec.Speed;
        ec.NMAgent.SetDestination(targetPos);
        ec.NMAgent.isStopped = false;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float distance = (targetPos - ec.transform.position).magnitude;
        animator.SetBool("PlayerVisible", ec.IsTargetVisible);
        animator.SetFloat("TargetDistance", distance);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ec.NMAgent.isStopped = true;
    }
}
