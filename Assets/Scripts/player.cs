using System.Collections;
using Unity.VisualScripting;
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

    //Dash movement variables
    public GameObject dash;
    GameObject dashing = null;
    [SerializeField] private float dashBoost = 2.5f;
    [SerializeField] private float dashDuration = 1f;

    [SerializeField] private GameObject head;
    [SerializeField] private GameObject body;

    // Camera Rotation
    public float mouseSensitivity = 2f;
    private float verticalRotation = 0f;
  
    //Simple use of a state machine
    enum moveState
    {
        normal,
        jumping,
        dashing
    }
    moveState moving = moveState.normal;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //gets the audioSource component from the player object
        source = GetComponent<AudioSource>();

        //gets the player's rigidbody component for physics & movement
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; //stops the player from rotating and getting stuck on the floor when trying to move 

        // Hides the mouse from view
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }


    // Update is called once per frame
    void Update()
    {
        //get movement input from player
        moveHorizontal = Input.GetAxisRaw("Horizontal");
        moveForward = Input.GetAxisRaw("Vertical");
        
        //Lets the player jump if they are not currently jumpung/dashing
        if (Input.GetButtonDown("Jump")  & moving == moveState.normal)
        {
            moving = moveState.jumping;
            jump();
        }

        //Lets the player use a Dash attack if they are currently moving and not jumping/dashing
        if (Input.GetKeyDown("tab") & moving == moveState.normal & rb.linearVelocity.z != 0)
        {
            moving = moveState.dashing;
            StartCoroutine(attack());
        }

    }


    void FixedUpdate()
    {
        //Checks player state
        switch(moving)
        { 
            case moveState.dashing:

                break;

            default:
                MovePlayer();
                ApplyJumpPhysics();
                break;

        }
        

        
    }

    private void LateUpdate()
    {
        RotateCamera();

    }

    void RotateCamera()
    {
        //Rotates player using mouse input
        float horizontalRotation = Input.GetAxis("Mouse X") * mouseSensitivity;
        transform.Rotate(0, horizontalRotation, 0);

        verticalRotation -= Input.GetAxis("Mouse Y") * mouseSensitivity;

        verticalRotation = Mathf.Clamp(verticalRotation, -35f, 45f);

        head.transform.localRotation = Quaternion.Euler(0, 0, verticalRotation);
        


    }

    void MovePlayer()
    { 
        Vector3 movement = (transform.right * moveHorizontal) + (transform.forward * moveForward).normalized;
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
        }
        else if (rb.linearVelocity.y > 0)
        {
            // Rising: Change multiplier to make player reach peak of jump faster
            rb.linearVelocity += Vector3.up * Physics.gravity.y * ascendMultiplier * Time.fixedDeltaTime;
        }
        else //if not jumping or falling from jump, change state back to normal
        {
            moving = moveState.normal;
        }
    }


    //Coroutine
    IEnumerator attack()
    {
        //Burst of speed.
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, rb.linearVelocity.y, (rb.linearVelocity.z * moveSpeed * dashBoost));

        //if there is no dash object created, 
        if (dashing == null)
        {
            //create one as child of the player body object,
            dashing = Instantiate(dash, parent:body.transform, false );

            //and return here after the duration.
            yield return new WaitForSeconds(dashDuration);

            //Destroy the dash object (setting back to null) and set state back to normal.
            Destroy(dashing);
            moving = moveState.normal;
        }
        else { yield return null; }

        
    }

}