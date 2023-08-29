using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class PositionEvent : BaseEventData
{
    public Vector3 position;
    public PositionEvent(Vector3 position) : base(EventSystem.current)
    {
        this.position = position;
    }
}

public interface IPositionReceiver : IEventSystemHandler
{
    void OnReceivePosition(Vector3 position);
}

public class EnemyComms : MonoBehaviour, IPositionReceiver
{

    [SerializeField]
    [Range(1, 10)]
    private float communicationRange = 5f;
    [SerializeField]
    private float communicationDelay = 2f;
    
    private UnityEvent<Vector3> newPositionEvent;
    private EnemyState ecStatus;
    private bool isTriggeredOnce;
    private EnemyController ec;

    private float counter;

    public void OnReceivePosition(Vector3 position)
    {
        ec.ForceTargetVisible = true;
        ec.CurrentPosition = position;
    }

    private void Awake()
    {
        ec = gameObject.GetComponent<EnemyController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        newPositionEvent = new UnityEvent<Vector3>();
        ecStatus = ec.CurrentState;
        isTriggeredOnce = false;
    }

    // Update is called once per frame
    void Update()
    {
        ecStatus = ec.CurrentState;
        if (ecStatus == EnemyState.triggered)
        {
            if (!isTriggeredOnce)
            {
                counter += Time.deltaTime;
                if (counter >= communicationDelay)
                {
                    isTriggeredOnce = true;
                    counter = 0;
                    ExecuteEvents.Execute<IPositionReceiver>(gameObject, null, (x, y) => x.OnReceivePosition(ec.CurrentPosition));
                }
            }
        } else
        {
            isTriggeredOnce = false;
        }
    }
    
}
