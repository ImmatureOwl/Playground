using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    idle,
    alert,
    triggered,
    searching,
    moving_on,
    next_cp,
    last
}

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private Material idleMat = null;
    [SerializeField]
    private Material triggeredMat = null;
    [SerializeField]
    private Material alertMat = null;
    [SerializeField]
    [Range(0.001f, 10)]
    private float rotationSpeed = 0.001f;
    [SerializeField]
    private float speed = 1;
    [SerializeField]
    [Range(0, 1)]
    [Tooltip("Speed multiplier for certain states that require slower movement")]
    private float speedPenality = 0.7f;
    [SerializeField]
    [Tooltip("Minimum distance between the enemy and its target while it's being chased")]
    private float safeDistance = 1.5f;
    [SerializeField]
    [Tooltip("Minimum distance between the enemy and its target while searching")]
    private float minimumSafeDistance = 0.7f;
    [SerializeField]
    [Tooltip("Time required for the target to stay in view for the enemy to jump in the Triggered state")]
    private float detectionDelay = 1;
    [SerializeField]
    [Tooltip("Time required for the target to stay out of view for the enemy to leave the Searching state")]
    private float maxSearchTime = 1;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    [Tooltip("used for patrolling enemies")]
    private CPManager cpManager;

    private Vector3 targetLastKnownPosition;
    private Quaternion defaultLookingDirection;
    bool playerVisible = false;
    private Transform target = null;
    private PlayerScanner[] scanners;
    private MeshRenderer enemyMR;
    private EnemyState currentState = EnemyState.last;
    private Vector3 startPos;
    private Vector3 nextPos;
    private NavMeshAgent nMAgent;
    private bool forceTargetVisible;

    public EnemyState CurrentState
    {
        get { return currentState; }
        set
        {
            if (currentState != value)
            {
                currentState = value;
                SetMaterial(value);
            }
        }
    }

    public Transform Target
    {
        get { return target; }
    }

    public Transform CurrentCPTransform
    {
        get { return cpManager.CurrentCP; }
    }
    public PlayerScanner[] Scanners
    {
        get { return scanners; }
    }

    public bool IsTargetVisible
    {
        get 
        {
            if (ForceTargetVisible) return true;
            return playerVisible; 
        }
    }

    public float RotationSpeed
    {
        get { return rotationSpeed; }
    }

    public float Speed
    {
        get
        {
            switch (currentState)
            {
                case EnemyState.searching:
                    return speed * speedPenality;
                default:
                    return speed;
            }
        }
    }

    public Vector3 TargetLastKnownPosition
    {
        get { return targetLastKnownPosition; }
    }

    public Quaternion DefaultLookingDirection
    {
        get { return defaultLookingDirection; }
        set { defaultLookingDirection = value; }
    }

    public Vector3 StartPosition
    {
        get { return startPos; }
    }

    /// <summary>
    /// calling this property will change the Vector3 value to the next stored value.
    /// If you intend to read the current checkpoint position, use <b>CurrentPosition</b> instead
    /// </summary>
    public Vector3 NextPosition
    {
        get 
        {
            if (cpManager == null) return Vector3.zero;
            return nextPos = cpManager.NextCP.position;
        }
    }

    public Vector3 CurrentPosition
    {
        get { return nextPos; }
        set { nextPos = value; }
    }

    public float SafeDistance
    {
        get
        {
            switch (currentState)
            {
                case EnemyState.triggered:
                    return safeDistance;
                default:
                    return minimumSafeDistance;
            }
        }
    }

    public NavMeshAgent NMAgent
    {
        get { return nMAgent; }
    }

    public bool ForceTargetVisible
    {
        get { return forceTargetVisible; }
        set 
        { 
            forceTargetVisible = true;
            playerVisible = true;
        }
    }

    private void SetMaterial(EnemyState state)
    {
        switch (state)
        {
            case EnemyState.idle:
                enemyMR.material = idleMat;
                break;
            case EnemyState.alert:
                enemyMR.material = alertMat;
                break;
            case EnemyState.triggered:
                enemyMR.material = triggeredMat;
                break;
            case EnemyState.searching:
                enemyMR.material = alertMat;
                break;
            case EnemyState.moving_on:
                enemyMR.material = idleMat;
                break;
            default:
                enemyMR.material = idleMat;
                break;
        }
    }

    private void Awake()
    {
        nMAgent = gameObject.GetComponent<NavMeshAgent>();
        scanners = gameObject.GetComponents<PlayerScanner>();
        GameObject[] objs = FindObjectsOfType<GameObject>();
        foreach (var obj in objs)
        {
            if (obj.tag == "Player")
            {
                target = obj.transform;
                break;
            }
        }
        if (target != null)
        {
            foreach (var scanner in scanners)
            {
                scanner.SetTarget(target);
            }
        }
        if (animator == null) return;
    }

    void Start()
    {
        defaultLookingDirection = transform.rotation;
        enemyMR = gameObject.GetComponent<MeshRenderer>();
        CurrentState = EnemyState.idle;
        animator.SetBool("PlayerVisible", false);
        animator.SetFloat("DetectionDelay", detectionDelay);
        animator.SetFloat("MaxSearchTime", maxSearchTime);
        animator.SetFloat("TargetDistance", 0);
        startPos = transform.position;
        nextPos = startPos;
    }

    void Update()
    {
        playerVisible = false;
        if (target == null) return;

        foreach (var scanner in scanners)
        {
            if (scanner.TargetVisible)
            {
                playerVisible = true;
                targetLastKnownPosition = target.position;
            }
        }
    }

    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-\\
    //                                                  EDITOR                                                         \\
    //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-\\

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, targetLastKnownPosition);
    }
}