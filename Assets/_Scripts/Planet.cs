using UnityEngine;

public class Planet : GlobalMonoBehaviour
{
    public float gravity;

    private void Awake()
    {
        Global = FindAnyObjectByType<Global>();
    }

    // Apply a gravitational pull towards this planet to the player, and set player's body upright to it
    void FixedUpdate()
    {
        Vector3 gravityDirection = (Global.playerRb.transform.position - transform.position).normalized;
        Vector3 playerUp = Global.playerRb.transform.up;

        Global.playerRb.AddForce(gravityDirection * -gravity);
        Global.playerRb.transform.rotation = Quaternion.FromToRotation(playerUp, gravityDirection) * Global.playerRb.transform.rotation;
    }
}