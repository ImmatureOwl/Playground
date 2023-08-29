using UnityEngine;

public class CPFloatingPoint : MonoBehaviour
{

    [SerializeField]
    private AnimationCurve movementVelocity;

    [SerializeField]
    private float clkTime = 1;

    [SerializeField]
    private float amplitude = 1;

    private float counter = 0;

    private Vector3 pos;

    // Start is called before the first frame update
    void Start()
    {
        pos = transform.position;
        transform.position = new Vector3(pos.x, pos.y + movementVelocity.Evaluate(0), pos.z);
    }

    // Update is called once per frame
    void Update()
    {
        counter += Time.deltaTime;
        if (counter >= clkTime) counter = 0;
        transform.position = new Vector3(pos.x, pos.y + movementVelocity.Evaluate(counter / clkTime) * amplitude, pos.z);
    }
}
