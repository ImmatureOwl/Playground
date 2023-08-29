using UnityEngine;

public class CameraMotion : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 0;

    float xRotate = Input.GetAxis("Mouse Y");
    float yRotate = Input.GetAxis("Mouse X");
    float hMove = Input.GetAxis("Horizontal");
    float vMove = Input.GetAxis("Vertical");

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(hMove, 0, vMove) * moveSpeed * Time.deltaTime;
    }
}
