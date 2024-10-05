using UnityEngine;

public class SpaceshipController : MonoBehaviour
{
    // Movement variables
    public float moveSpeed = 10f;
    public float rotationSpeed = 100f;

    // Mouse rotation variables
    public float mouseSensitivity = 2f;

    // Layer mask for the "Planet" layer

    public LayerMask planetLayer;

    void Update()
    {
        HandleMovement();
        HandleMouseRotation();

        // Shoot a raycast forward to check if it hits a "Planet"
        if (Input.GetMouseButtonDown(0)) // Left mouse button to shoot raycast
        {
            ShootRaycast();
        }
    }

    void HandleMovement()
    {
        // Get input for WASD movement
        float moveHorizontal = Input.GetAxis("Horizontal"); // A/D for left/right
        float moveVertical = Input.GetAxis("Vertical"); // W/S for forward/backward

        // Apply movement in local space
        Vector3 moveDirection = new Vector3(moveHorizontal, 0, moveVertical);
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.Self);
    }

    void HandleMouseRotation()
    {
        // Get mouse input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Rotate the spaceship based on mouse movement
        transform.Rotate(Vector3.up, mouseX * rotationSpeed * Time.deltaTime, Space.World);
        transform.Rotate(Vector3.left, mouseY * rotationSpeed * Time.deltaTime, Space.Self);
    }

    void ShootRaycast()
    {
        // Forward direction from the spaceship
        Vector3 forward = transform.TransformDirection(Vector3.forward);

        // Shoot a raycast from the spaceship's position forward
        RaycastHit hit;
        if (Physics.Raycast(transform.position, forward, out hit, Mathf.Infinity, planetLayer))
        {
            // If the raycast hits an object on the "Planet" layer, do something
            Debug.Log("Hit a planet: " + hit.collider.name);

            // Implement whatever action you want to happen upon hitting a planet
            HandlePlanetHit(hit.collider);
        }
    }

    void HandlePlanetHit(Collider planetCollider)
    {
        // Example of doing something when a planet is hit
        Debug.Log("Planet hit: " + planetCollider.name);
        // Add your custom logic here, like initiating a landing sequence, etc.
    }
}
