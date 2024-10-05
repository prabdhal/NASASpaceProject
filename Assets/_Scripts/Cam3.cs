using UnityEngine;

public class Cam3 : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    public Vector3 offset;   // Local space offset relative to the player

    void LateUpdate()
    {
        if (player == null)
        {
            return;
        }

        // Set the position and rotation of the camera to match the player's, with an offset
        transform.position = player.TransformPoint(offset);  // Apply local space offset
        transform.rotation = player.rotation;                // Match player's rotation
    }
}
