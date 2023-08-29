using UnityEngine;

public class Triggered : StateMachineBehaviour
{
    private EnemyController ec;

    private Transform target;
    private Vector3 targetValue; //used to store the target's distance from the object
    private Quaternion targetRotation;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (ec == null) ec = animator.gameObject.GetComponent<EnemyController>();
        ec.CurrentState = EnemyState.triggered;
        target = ec.Target;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!ec.IsTargetVisible)
        {
            animator.SetBool("PlayerVisible", false);
            return;
        }
        targetValue = new Vector3(target.position.x, target.position.y, target.position.z) - ec.transform.position;
        targetRotation = Quaternion.LookRotation(targetValue, Vector3.up);
        ec.transform.rotation = Quaternion.Slerp(ec.transform.rotation, targetRotation, Time.deltaTime * ec.RotationSpeed);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
