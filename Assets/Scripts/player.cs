using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlayerController : MonoBehaviour
{
    private AudioSource source;

    public AudioClip[] stomps;


    //Movement variables

    // Camera Rotation
    public float mouseSensitivity = 2f;
    private float verticalRotation = 0f;
    //private Transform cameraTransform;

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

    [SerializeField] private GameObject head;
    [SerializeField] private GameObject body;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        source = GetComponent<AudioSource>();

        //cameraTransform = Camera.main.transform;
        //cameraTransform.rotation = body.transform.rotation;


        // Hides the mouse
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

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

        if (Input.GetKeyDown("tab"))
        {

            attack();
        }

    }

    void FixedUpdate()
    {
        MovePlayer();

        ApplyJumpPhysics();
    }

    private void LateUpdate()
    {
        RotateCamera();

    }

    void RotateCamera()
    {
        float horizontalRotation = Input.GetAxis("Mouse X") * mouseSensitivity;
        transform.Rotate(0, horizontalRotation, 0);

        verticalRotation -= Input.GetAxis("Mouse Y") * mouseSensitivity;

        verticalRotation = Mathf.Clamp(verticalRotation, -35f, 45f);

        head.transform.localRotation = Quaternion.Euler(0, 0, verticalRotation);
        //cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);


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
        }
        else if (rb.linearVelocity.y > 0)
        {
            // Rising: Change multiplier to make player reach peak of jump faster
            rb.linearVelocity += Vector3.up * Physics.gravity.y * ascendMultiplier * Time.fixedDeltaTime;
        }
    }



    void attack()
    {
        Vector3 movement = ((transform.right * moveHorizontal) + transform.forward * moveForward).normalized;
        Vector3 targetVelocity = movement * moveSpeed * 2;
        Vector3 velocity = rb.linearVelocity;
        velocity.x = targetVelocity.x;
        velocity.z = 0f;
        rb.linearVelocity = velocity;


        GameObject dashing = null;

        if (dashing == null)
        {

            dashing = Instantiate(dash, parent:body.transform, false );

            Destroy(dashing, 2f);
        }

    }

}