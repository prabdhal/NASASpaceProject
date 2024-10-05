using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    // Public variable to assign the target object
    public Transform target;

    // Offset from the target
    public Vector3 offset = new Vector3(0, 3, -5);

    // Speed at which the camera will interpolate its position
    public float smoothSpeed = 0.125f;

    private void LateUpdate()
    {
        if (target == null) return;

        // Get the target's forward direction, ignoring the y-axis rotation
        Vector3 targetForward = target.forward;

        // Normalize the forward vector (so we only care about the direction and not the magnitude)
        targetForward.y = 0; // Keep the y component out so we only care about horizontal rotation
        targetForward.Normalize();

        // Calculate the desired position only taking into account the forward/backward position
        Vector3 desiredPosition = target.position + targetForward * offset.z + Vector3.up * offset.y;

        // Smoothly interpolate the camera's position for smoother movement
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Set the camera's position to the smoothed position
        transform.position = smoothedPosition;

        // Make the camera look at the target
        transform.LookAt(target.position + Vector3.up * 1.5f); // Optional: Adjust to look at the target's head area
    }
}
