using UnityEngine;

public class PlayerRotator : MonoBehaviour
{
    [SerializeField]
    private float rotationSpeed = 0;

    [SerializeField]
    private Transform child;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            float xRotate = Input.GetAxis("Mouse Y");
            float yRotate = Input.GetAxis("Mouse X");
            float zRotate = Input.GetAxis("Horizontal");

            var position = transform.position + Quaternion.AngleAxis(45, transform.up) * transform.forward;
            child.position = position;

            Quaternion rotation = Quaternion.Euler(new Vector3(xRotate, yRotate * -1, zRotate) * rotationSpeed * Time.deltaTime);
            this.transform.rotation = rotation * transform.rotation;


            //this.transform.SetPositionAndRotation(transform.position, rotation);

            //transform.Rotate(new Vector3(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0) * rotationSpeed * Time.deltaTime, Space.World);
            //transform.localEulerAngles += new Vector3(xRotate, yRotate, 0) * rotationSpeed * Time.deltaTime;    
        }

    }
}
