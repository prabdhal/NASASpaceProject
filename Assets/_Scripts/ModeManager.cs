using System.Collections;
using UnityEngine;

public class ModeManager : GlobalMonoBehaviour
{
    [SerializeField] private LayerMask planetLayer;

    private IEnumerator DelayRay()
    {
         yield return new WaitForSeconds(1f);
        ShootRaycast();
    }

    private void ShootRaycast()
    {
        Debug.Log($"player.transform.up .y: " + Global.player.transform.up.y);

        Vector3 origin = Global.player.transform.position + new Vector3(Global.player.transform.right.x * 5f, Global.player.transform.up.y * 2, 0);
        RaycastHit hit;
        Vector3 dir = -Global.player.transform.up * 10f;

        if (Physics.Raycast(origin, dir, out hit, 10f))
        {
            Vector3 hitPoint = hit.point; // Get the hit point on the sphere
            Vector3 hitNormal = hit.normal; // Get the normal at the hit point

            Debug.DrawRay(hitPoint, hitNormal, Color.red, 100f);

            // Move the cylinder to the hit point on the sphere
            Global.spaceship.SetPosition(hitPoint + new Vector3(0, Global.player.transform.up.y/2, 0));

            // Create a forward direction that's tangent to the hit normal (but in reverse)
            Vector3 forwardDirection = Vector3.ProjectOnPlane(Global.spaceship.transform.forward, hitNormal).normalized;

            // Reverse the forward direction
            forwardDirection = -forwardDirection;

            // Set rotation to face the reversed forward direction, and maintain 90-degree rotation on X-axis
            Quaternion rotation = Quaternion.LookRotation(forwardDirection, hitNormal);

            // Adjust for the 90-degree rotation on the X-axis
            Global.spaceship.transform.rotation = rotation * Quaternion.Euler(90f, 0f, 0f);
        }
    }

    public void SetAsPlayer(Planet planet, Vector3 spawnPoint)
    {
        Global.player.SetPosition(spawnPoint);
        Global.player.gameObject.SetActive(true);
        Global.spaceship.DisableSpaceship();
        Global.shipCamera.gameObject.SetActive(false);
        Global.playerCamera.gameObject.SetActive(true);

        if (planet != null)
        {
            Global.planetManager.EnablePlanet(planet);
            Global.player.planet = planet.transform;
        }

        //ShootRaycast(player.transform.position);
        StartCoroutine(DelayRay());
    }

    public void SetAsSpaceship()
    {
        //spaceship.SetPosition(spawnPosition);
        Global.player.gameObject.SetActive(false);
        Global.spaceship.EnableSpaceship();
        Global.playerCamera.gameObject.SetActive(false);
        Global.shipCamera.gameObject.SetActive(true);

        Global.planetManager.DisablePlanet();
    }
}
