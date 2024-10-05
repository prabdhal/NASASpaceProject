using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player; // The target the camera follows (usually the player)
    public float distance; // Distance from the player
    public float height = 2.0f; // Height above the player
    public float mouseSensitivity = 4.0f; // Sensitivity for mouse input

    public float minAngleClamp;
    public float maxAngleClamp;

    private float currentAngleX; // Current camera angle around the X-axis
    private float currentAngleY; // Current camera angle around the Y-axis

    void Start()
    {
        // Initialize angles based on current rotation
        currentAngleX = transform.eulerAngles.x;
        currentAngleY = transform.eulerAngles.y;
    }

    void LateUpdate()
    {
        // Get mouse input
        currentAngleY += Input.GetAxis("Mouse X") * mouseSensitivity;
        currentAngleX -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        currentAngleX = Mathf.Clamp(currentAngleX, minAngleClamp, maxAngleClamp); // Limit vertical angle

        // Calculate the desired rotation
        Quaternion rotation = Quaternion.Euler(currentAngleX, currentAngleY, 0);

        // Calculate the target position based on the player position, rotation, and fixed distance
        Vector3 direction = new Vector3(0, 0, -distance); // Camera's distance from the player
        Vector3 rotatedDirection = rotation * direction; // Apply rotation to the direction vector

        // Set the desired position, ensuring the fixed distance
        Vector3 wantedPosition = player.position + new Vector3(0, height, 0) + rotatedDirection;

        // Follow and rotate to the desired wanted angle
        transform.position = wantedPosition;
        transform.rotation = rotation;
    }
}
