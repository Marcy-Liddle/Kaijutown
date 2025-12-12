using UnityEngine;

public class playerMovement : MonoBehaviour
{

    //Movement variables

    // Camera Rotation
    public float mouseSensitivity = 2f;
    private float verticalRotation = 0f;
    private Transform cameraTransform;

    //Grounded movement:
    private Rigidbody rb;
    public float moveSpeed = 5f;
    private float moveHorizontal;
	private float moveForward;

    //Jumping physics
    public float jumpForce = 10f;
    public float fallMultiplier = 2.5f; // Multiplies gravity when falling down
    public float ascendMultiplier = 2f; // Multiplies gravity for ascending to peak of jump



    [SerializeField] private float m_RayDistance = 10.0f;

    private bool m_RayHit = false;
    private Vector3 m_HitPoint = Vector3.zero;
    private Vector3 m_HitNormal = Vector3.zero;
    private bool m_Grounded = false;

    public GameObject explode;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; //stops the cyclinder from rotating and getting stuck on the floor when trying to move
        cameraTransform = Camera.main.transform;

        // Hides the mouse
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
		moveHorizontal = Input.GetAxisRaw("Horizontal");
		moveForward = Input.GetAxisRaw("Vertical");

        RotateCamera();

        if (Input.GetButtonDown("Jump"))
        {

            jump();
        }

        if (Input.GetKeyDown("Fire"))
        {
            DoRaycast();
        }

    }

    void FixedUpdate()
    {
        MovePlayer();
        ApplyJumpPhysics();
    }

    void MovePlayer()
    {

        Vector3 movement = ((transform.right * moveHorizontal) + transform.forward * moveForward).normalized;
        Vector3 targetVelocity = movement * moveSpeed;

        // Apply movement to the Rigidbody
        Vector3 velocity = rb.linearVelocity;
        velocity.x = targetVelocity.x;
        velocity.z = targetVelocity.z;
        rb.linearVelocity = velocity;

        

    }

    void RotateCamera()
    {
        float horizontalRotation = Input.GetAxis("Mouse X") * mouseSensitivity;
        transform.Rotate(0, horizontalRotation, 0);

        verticalRotation -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
    }

    void jump() 
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z); // Initial burst for the jump

        
    }

    void ApplyJumpPhysics()
    {
        if (rb.linearVelocity.y < 0)
        {
            // Falling: Apply fall multiplier to make descent faster
            rb.linearVelocity += Vector3.up * Physics.gravity.y * fallMultiplier * Time.fixedDeltaTime;
        } // Rising
        else if (rb.linearVelocity.y > 0)
        {
            // Rising: Change multiplier to make player reach peak of jump faster
            rb.linearVelocity += Vector3.up * Physics.gravity.y * ascendMultiplier * Time.fixedDeltaTime;
        }
    }

    void DoRaycast()
    {
        RaycastHit hitInfo; //gives us information about what we hit (if anything)
        Ray ray = new Ray(transform.position, -transform.up);

        //Do the raycast. Store the information in hitInfo
        m_RayHit = Physics.Raycast(ray, out hitInfo, m_RayDistance);

        if (m_RayHit)
        {
            m_HitPoint = hitInfo.point;     //Store the position that our ray collided with the object
            m_HitNormal = hitInfo.normal;   //Store the surface normal of the object
            m_Grounded = Vector3.Dot(Vector3.up, hitInfo.normal) > 0.5f; //Bit of a magic number here. Just trust me on this one.

            Instantiate(explode, m_HitPoint, transform.rotation);

        }

    }



    private void OnDrawGizmos()
    {
        if (m_RayHit)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, m_HitPoint);

            Gizmos.color = Color.green;
            Gizmos.DrawSphere(m_HitPoint, 0.125f);

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(m_HitPoint, m_HitPoint + m_HitNormal);
        }
        else
        {
            //Draw a line to where the ray ends
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position - transform.up * m_RayDistance);

            Gizmos.DrawSphere(transform.position - transform.up * m_RayDistance, 0.125f);
        }
    }
}