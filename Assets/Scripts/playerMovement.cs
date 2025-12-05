using UnityEngine;

public class playerMovement : MonoBehaviour
{

    //Movement variables
    
    //Grounded movement:
    private Rigidbody rb;
    public float moveSpeed = 5f;
    private float moveHorizontal;
	private float moveForward;

    //Jumping physics
    public float jumpForce = 10f;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.freezeRotation = true; //stops the cyclinder from rotating and getting stuck on the floor when trying to move
    }

    // Update is called once per frame
    void Update()
    {
		moveHorizontal = Input.GetAxisRaw("Horizontal");
		moveForward = Input.GetAxisRaw("Vertical");

        

        if (Input.GetButtonDown("Jump"))
        {

            jump();
        }

        movePlayer();
	}

    void movePlayer()
    {

        Vector3 movement = ((transform.right * moveHorizontal) + transform.forward * moveForward).normalized;
        Vector3 targetVelocity = movement * moveSpeed;

        // Apply movement to the Rigidbody
        Vector3 velocity = rb.linearVelocity;
        velocity.x = targetVelocity.x;
        velocity.z = targetVelocity.z;
        rb.linearVelocity = velocity;
    }

    void jump() 
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z); // Initial burst for the jump
    }
}
