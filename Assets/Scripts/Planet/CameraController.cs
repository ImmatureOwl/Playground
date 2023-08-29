using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;

    public Vector3 offset;
    public Quaternion rotation;
    [Range(0, 1)]
    public float cameraSpeed;



    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (target == null) return; //check for target

        //movement
        Vector3 newPos = target.TransformPoint(offset);
        transform.position = Vector3.Slerp(transform.position, newPos, cameraSpeed);

        //rotation
        Quaternion newRot = target.rotation * rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, newRot, cameraSpeed);
    }
}
