using UnityEngine;

public class Seeking : StateMachineBehaviour
{
    private EnemyController ec;
    private Vector3 targetPosition;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (ec == null) ec = animator.gameObject.GetComponent<EnemyController>();
        ec.NMAgent.speed = ec.Speed;
        ec.NMAgent.stoppingDistance = ec.SafeDistance;
        ec.NMAgent.isStopped = false;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        targetPosition = ec.TargetLastKnownPosition;
        
        Vector3 direction = (targetPosition - ec.transform.position).normalized;
        float distance = (targetPosition - ec.transform.position).magnitude;
        float travelAmount = ec.Speed * Time.deltaTime;

        if (distance >= ec.SafeDistance + travelAmount)
        {
            ec.NMAgent.SetDestination(targetPosition);
            ec.NMAgent.isStopped = false;
        } else
        {
            ec.NMAgent.isStopped = true;
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }
}
