using UnityEngine;

public class Planet : MonoBehaviour
{
    public MeshRenderer renderer;
    public float gravity = 10f; // Gravity strength
    public Rigidbody rb;

    void Start()
    {
        rb.useGravity = false; // Disable default gravity
        rb.constraints = RigidbodyConstraints.FreezeRotation; // Prevent default rotation
    }

    void FixedUpdate()
    {
        Vector3 gravityDirection = (rb.transform.position - transform.position).normalized;
        Vector3 playerUp = rb.transform.up;

        // Apply gravity force toward the planet
        rb.AddForce(gravityDirection * -gravity);

        // Rotate the player to align with the planet's surface
        Quaternion targetRotation = Quaternion.FromToRotation(playerUp, gravityDirection) * rb.transform.rotation;
        rb.transform.rotation = Quaternion.Slerp(rb.transform.rotation, targetRotation, 50 * Time.deltaTime);
    }
}