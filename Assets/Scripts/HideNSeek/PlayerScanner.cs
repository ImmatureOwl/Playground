using UnityEngine;

public class PlayerScanner : MonoBehaviour
{
    //private float rotationCounter = 0;

    [SerializeField]
    [Range(0, 15)]
    private float detectDistance = 0;

    [SerializeField]
    [Range(0, 360)]
    private float detectAngle = 0;

    private Transform target;
    private Vector3 TargetLastKnownPosition;

    public bool TargetVisible
    {
        get { return IsTargetVisible(); }
    }

    public Vector3 TargetPosition
    {
        get { return TargetLastKnownPosition; }
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    private bool IsTargetVisible()
    {
        if (target == null) return false;

        Vector3 distToPlayer = target.transform.position - transform.position;
        if (distToPlayer.magnitude <= detectDistance)
        {
            float angle = Mathf.Rad2Deg * (float)Mathf.Acos(Vector3.Dot(transform.forward, distToPlayer.normalized));
            //Debug.Log("Angle: " + angle);
            if (angle <= detectAngle * 0.5f || angle.ToString() == "NaN")
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, target.transform.position - transform.position, out hit)
                    && hit.collider.gameObject == target.gameObject)
                {
                    TargetLastKnownPosition = hit.collider.transform.position;
                    return true;
                }
            }
        }
        return false;
    }

    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-\\
    //                                                  EDITOR                                                         \\
    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-\\

    private void OnDrawGizmos()
    {

        if (target != null && TargetVisible)
        {
            Gizmos.color = Color.red;
        }
        else
        {
            Gizmos.color = Color.white;
        }
        Vector3 tempVector = transform.forward * detectDistance;
        Vector3 endPos;
        for (float i = -1; i <= 1; i += 0.05f)
        {
            Quaternion rotation = Quaternion.Euler(0, detectAngle * 0.5f * i, 0);
            Vector3 rotationVector = rotation * tempVector;
            endPos = transform.position + rotationVector;
            RaycastHit hit;
            if (Physics.Raycast(transform.position, endPos - transform.position, out hit, detectDistance, 3))
            {
                endPos = hit.point;
            }


            Gizmos.DrawLine(transform.position, endPos);
        }
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + tempVector);


    }
}
