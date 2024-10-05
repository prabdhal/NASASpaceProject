using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public float gravityScale = 2f; // Gravity strength
    public float rotationSpeed = 10f; // Speed for smooth rotation
    public Transform planetCenter; // The center of the planet (used to calculate gravity direction)

    private Transform camera;
    private Rigidbody rb;
    private bool isGrounded;

    void Start()
    {
        camera = Camera.main.transform;
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // Prevent the player from tipping over

        // Hide the cursor and lock it to the center of the screen
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        MoveInput();
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }

        ApplyGravity();
        AlignToGroundNormal();
    }

    private void MoveInput()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // Get the forward and right directions of the camera
        Vector3 forward = camera.transform.forward;
        Vector3 right = camera.transform.right;

        // Flatten the forward and right vectors to only consider the X and Z axes
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        // Calculate the movement direction relative to the camera
        Vector3 moveDirection = forward * moveVertical + right * moveHorizontal;

        if (moveDirection.sqrMagnitude > 0.01f) // Only rotate if there is input
        {
            RotatePlayer(moveDirection); // Rotate the player relative to the camera
        }

        // Apply the movement
        Vector3 velocity = moveDirection * moveSpeed;
        velocity.y = rb.velocity.y; // Maintain the current vertical velocity
        rb.velocity = velocity;
    }

    private void RotatePlayer(Vector3 moveDirection)
    {
        // Calculate the target rotation based on the movement direction, ensuring the player's "up" stays aligned with the ground normal
        Vector3 groundNormal = (transform.position - planetCenter.position).normalized; // Ground normal to maintain upright position
        Quaternion targetRotation = Quaternion.LookRotation(moveDirection, groundNormal); // Use ground normal as up direction

        // Smoothly rotate the player to face the movement direction
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

    private void ApplyGravity()
    {
        if (planetCenter != null)
        {
            // Calculate the direction from the player to the center of the planet
            Vector3 directionToPlanet = (planetCenter.position - transform.position).normalized;

            // Apply a force towards the planet's center (simulating gravity)
            Vector3 gravityForce = directionToPlanet * gravityScale;
            rb.AddForce(gravityForce, ForceMode.Acceleration);
        }
    }

    private void AlignToGroundNormal()
    {
        if (planetCenter != null)
        {
            // Calculate the ground normal (the direction from the player to the planet's center)
            Vector3 groundNormal = (transform.position - planetCenter.position).normalized;

            // Create the target rotation to align the player's "up" with the ground normal
            Quaternion targetRotation = Quaternion.FromToRotation(transform.up, groundNormal) * transform.rotation;

            // Smoothly rotate the player to stand upright relative to the ground
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z); // Set vertical velocity directly
    }

    private void FixedUpdate()
    {
        // Apply custom gravity
        ApplyGravity();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the player is grounded
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // Set grounded to false when leaving the ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
