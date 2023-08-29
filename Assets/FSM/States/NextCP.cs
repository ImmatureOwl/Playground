using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextCP : StateMachineBehaviour
{
    EnemyController ec;
    Vector3 targetPosition;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (ec == null) ec = animator.gameObject.GetComponent<EnemyController>();
        ec.CurrentState = EnemyState.next_cp;
        targetPosition = ec.NextPosition;
        ec.DefaultLookingDirection = Quaternion.LookRotation(ec.CurrentCPTransform.forward, Vector3.up); 
        float distance = (targetPosition - ec.transform.position).magnitude;
        animator.SetFloat("TargetDistance", distance);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }
}
