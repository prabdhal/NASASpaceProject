using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;

    private Rigidbody rb;
    private Transform cameraTransform;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        // Get input for movement
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Calculate the direction relative to the camera's forward and right
        Vector3 cameraForward = Vector3.ProjectOnPlane(cameraTransform.forward, transform.up).normalized;
        Vector3 cameraRight = Vector3.ProjectOnPlane(cameraTransform.right, transform.up).normalized;

        Vector3 moveDirection = (cameraForward * vertical + cameraRight * horizontal).normalized;

        if (moveDirection.magnitude > 0.1f)
        {
            // Move the player
            rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.deltaTime);

            // Rotate the player to face the direction of movement
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection, transform.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}