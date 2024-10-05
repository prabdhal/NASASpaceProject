using UnityEngine;

public class PlanetGravity : MonoBehaviour
{
    public Transform planet; // Assign the planet object in the inspector
    public float gravity = 10f; // Gravity strength

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false; // Disable default gravity
        rb.constraints = RigidbodyConstraints.FreezeRotation; // Prevent default rotation
    }

    void FixedUpdate()
    {
        Vector3 gravityDirection = (transform.position - planet.position).normalized;
        Vector3 playerUp = transform.up;

        // Apply gravity force toward the planet
        rb.AddForce(gravityDirection * -gravity);

        // Rotate the player to align with the planet's surface
        Quaternion targetRotation = Quaternion.FromToRotation(playerUp, gravityDirection) * transform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 50 * Time.deltaTime);        
    }
}