using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlanetPawnController : MonoBehaviour
{
    [Range(0.001f, 10)]
    public float moveSpeed;
    [Range(1, 100)]
    public float rotateSpeed;
    [Range(1, 100)]
    public float jumpMultiplier;
    [Range(1, 100)]
    public float gravityForce = 1;
    public Transform planet;

    private Vector3 movement;
    private float verticalAxis, rotation;
    private Rigidbody rb;

    //jump
    private bool canJump;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }


    // Update is called once per frame
    void Update()
    {
        verticalAxis = Input.GetAxis("Vertical");
        rotation = Input.GetAxis("Horizontal");
        movement = new Vector3(0, 0, verticalAxis).normalized;

        if (Input.GetKeyDown(KeyCode.Space)) //jump button pressed
        {
            if (canJump)
            {
                rb.AddForce(transform.up * moveSpeed * jumpMultiplier);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Surface")
        {
            canJump = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Surface")
        {
            canJump = false;
        }
    }

    private void FixedUpdate()
    {
        //gravity calculation
        Vector3 upDirection = (transform.position - planet.position).normalized; //the up direction of the target relative to the planet center
        Vector3 playerUp = transform.up;
        rb.AddForce(-gravityForce * upDirection);

        //rotation calculation
        Quaternion playerRot = Quaternion.FromToRotation(playerUp, upDirection) * transform.rotation;
        Quaternion moveRot = Quaternion.Euler(0, rotation * rotateSpeed * Time.deltaTime, 0);
        transform.rotation = playerRot * moveRot;

        //movement
        rb.MovePosition(rb.position + transform.TransformDirection(movement) * moveSpeed * Time.deltaTime); // moves object toward direction
    }
}
