using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;
    public float jumpForce = 5f;

    private Rigidbody rb;
    private Vector3 moveDirection;
    private Transform cameraTransform;
    private bool isGrounded;
    public Transform planet; // Reference to the planet for gravity

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cameraTransform = Camera.main.transform; // Reference the camera
        rb.useGravity = false; // Disable default gravity
        rb.constraints = RigidbodyConstraints.FreezeRotation; // Prevent default rotation
    }

    void Update()
    {
        MoveInput();
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }
    }

    private void MoveInput()
    {
        // Get input for movement
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Get the forward and right directions of the camera
        Vector3 cameraForward = Vector3.ProjectOnPlane(cameraTransform.forward, transform.up).normalized;
        Vector3 cameraRight = Vector3.ProjectOnPlane(cameraTransform.right, transform.up).normalized;

        // Calculate movement direction relative to the camera
        moveDirection = (cameraForward * vertical + cameraRight * horizontal).normalized;

        if (moveDirection.magnitude > 0.1f)
        {
            // Move the player in the direction of movement
            Vector3 targetPosition = rb.position + moveDirection * moveSpeed * Time.deltaTime;
            rb.MovePosition(targetPosition);

            // Rotate the player to face the movement direction (keep gravity-aligned)
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection, transform.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void Jump()
    {
        rb.AddForce(transform.up * jumpForce, ForceMode.VelocityChange);
    }

    void FixedUpdate()
    {
        ApplyGravity();
    }

    private void ApplyGravity()
    {
        if (!planet)
        {
            return;
        }

        // Get the direction towards the center of the planet
        Vector3 gravityDirection = (planet.position - transform.position).normalized;
        Vector3 playerUp = transform.up;

        // Apply gravity force
        rb.AddForce(gravityDirection * 10f);

        // Rotate the player to align with the planet's surface
        Quaternion targetRotation = Quaternion.FromToRotation(playerUp, gravityDirection) * transform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Planet"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Planet"))
        {
            isGrounded = false;
        }
    }

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }
}
