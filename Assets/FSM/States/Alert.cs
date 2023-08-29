using UnityEngine;

public class Alert : StateMachineBehaviour
{
    private EnemyController ec;
    private float counter;


    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (ec == null) ec = animator.gameObject.GetComponent<EnemyController>();
        ec.CurrentState = EnemyState.alert;
        counter = animator.GetFloat("DetectionDelay"); ;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        counter -= Time.deltaTime;
        animator.SetFloat("DelayCounter", counter);

        animator.SetBool("PlayerVisible", ec.IsTargetVisible);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetFloat("DelayCounter", animator.GetFloat("DetectionDelay"));
    }
}
