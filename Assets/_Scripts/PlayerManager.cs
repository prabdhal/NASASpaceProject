using System.Collections;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    #region Singleton
    public static PlayerManager Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    private PlayerCamera playerCamera;
    private ShipCamera shipCamera;

    [SerializeField] private PlayerController player;
    [SerializeField] private SpaceshipController spaceship;

    [SerializeField] private LayerMask planetLayer;

    private void Start()
    {
        playerCamera = FindAnyObjectByType<PlayerCamera>();
        shipCamera = FindAnyObjectByType<ShipCamera>();
    }

    public void SetAsPlayer(Planet planet, Vector3 spawnPoint)
    {
        player.SetPosition(spawnPoint);
        player.gameObject.SetActive(true);
        spaceship.DisableSpaceship();
        shipCamera.gameObject.SetActive(false);
        playerCamera.gameObject.SetActive(true);

        if (planet != null)
        {
            PlanetManager.Instance.EnablePlanet(planet);
            player.planet = planet.transform;
        }

        //ShootRaycast(player.transform.position);
        StartCoroutine(DelayRay());
    }

    private IEnumerator DelayRay()
    {
         yield return new WaitForSeconds(1f);

        ShootRaycast();
    }

    private void ShootRaycast()
    {
        Debug.Log($"player.transform.up .y: " + player.transform.up.y);

        Vector3 origin = player.transform.position + new Vector3(player.transform.right.x * 5f, player.transform.up.y * 2, 0);
        RaycastHit hit;
        Vector3 dir = -player.transform.up * 10f;

        if (Physics.Raycast(origin, dir, out hit, 10f))
        {
            Vector3 hitPoint = hit.point; // Get the hit point on the sphere
            Vector3 hitNormal = hit.normal; // Get the normal at the hit point

            Debug.DrawRay(hitPoint, hitNormal, Color.red, 100f);

            // Move the cylinder to the hit point on the sphere
            spaceship.SetPosition(hitPoint + new Vector3(0, player.transform.up.y/2, 0));

            // Create a forward direction that's tangent to the hit normal (but in reverse)
            Vector3 forwardDirection = Vector3.ProjectOnPlane(spaceship.transform.forward, hitNormal).normalized;

            // Reverse the forward direction
            forwardDirection = -forwardDirection;

            // Set rotation to face the reversed forward direction, and maintain 90-degree rotation on X-axis
            Quaternion rotation = Quaternion.LookRotation(forwardDirection, hitNormal);

            // Adjust for the 90-degree rotation on the X-axis
            spaceship.transform.rotation = rotation * Quaternion.Euler(90f, 0f, 0f);
        }
    }

    public void SetAsSpaceship()
    {
        //spaceship.SetPosition(spawnPosition);
        player.gameObject.SetActive(false);
        spaceship.EnableSpaceship();
        playerCamera.gameObject.SetActive(false);
        shipCamera.gameObject.SetActive(true);

        PlanetManager.Instance.DisablePlanet();
    }
}
