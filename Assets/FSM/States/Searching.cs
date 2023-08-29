using UnityEngine;

public class Searching : StateMachineBehaviour
{
    private Quaternion targetRotation;

    private EnemyController ec;
    private float counter;


    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (ec == null) ec = animator.gameObject.GetComponent<EnemyController>();
        ec.CurrentState = EnemyState.searching;
        counter = animator.GetFloat("MaxSearchTime");
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        counter -= Time.deltaTime;
        animator.SetFloat("SearchCounter", counter);

        targetRotation = Quaternion.LookRotation(ec.TargetLastKnownPosition - ec.transform.position, Vector3.up);
        ec.transform.rotation = Quaternion.Slerp(ec.transform.rotation, targetRotation, Time.deltaTime * ec.RotationSpeed);

        animator.SetBool("PlayerVisible", ec.IsTargetVisible);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetFloat("SearchCounter", animator.GetFloat("MaxSearchTime"));
        animator.SetFloat("TargetDistance", 1);
    }
}
