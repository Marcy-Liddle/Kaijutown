using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlayerController : MonoBehaviour
{
    private AudioSource source;

    public AudioClip[] stomps;
    //Movement variables
    //Grounded movement:
    private Rigidbody rb;
    public float moveSpeed = 10f;
    private float moveHorizontal;
	private float moveForward;

    //Jumping physics
    public float jumpForce = 10f;
    public float fallMultiplier = 2.5f; // Multiplies gravity when falling down
    public float ascendMultiplier = 2f; // Multiplies gravity for ascending to peak of jump

    public GameObject dash;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        source = GetComponent<AudioSource>();
     

        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; //stops the player from rotating and getting stuck on the floor when trying to move
        

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

        if (Input.GetKeyDown("enter"))
        {
            dashingAttack();
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
        
        //if player is moving (and no other stepping sound is playing), play a random stomp sound effect
        if ((velocity.x != 0 || velocity.z != 0) & source.isPlaying == false)
        {
            AudioClip step = stomps[(int)Random.Range(0, stomps.Length)];
            source.clip = step;
            source.PlayOneShot(step, 1.0f);
        
        }   
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

    void dashingAttack()
    {
        float temp = moveSpeed;
        moveSpeed = 20f;   
        dashing(moveSpeed, temp);
    }
   
    IEnumerator dashing(float moveSpeed, float temp)
    {
        Vector3 movement = ((transform.right * moveHorizontal) + transform.forward * moveForward).normalized;
        Vector3 targetVelocity = movement * moveSpeed;

        // Apply movement to the Rigidbody
        Vector3 velocity = rb.linearVelocity;
        velocity.x = targetVelocity.x;
        velocity.z = targetVelocity.z;
        rb.linearVelocity = velocity;

        rb.linearVelocity = velocity;
        Instantiate(dash, transform.position, transform.rotation);
        yield return new WaitForSeconds(2f);
        Destroy(dash);  
        
        
    }
    }
