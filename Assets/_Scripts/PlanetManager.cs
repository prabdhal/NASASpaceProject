using UnityEngine;

public class PlanetManager : MonoBehaviour
{
    public GameObject planetPrefab;  // The Planet prefab
    public int numberOfPlanets = 10; // Number of planets to generate
    public Vector2 distanceBetweenPlanets = new Vector2(200f, 500f); // Minimum distance between planets
    public Vector2 scaleRange = new Vector2(1f, 5f); // Range of scale for the planets
    public Vector2 gravityRange = new Vector2(0.2f, 20f); // Range of gravity for the planets
    public Vector3 startingPosition = Vector3.zero; // Starting position for the first planet

    void Start()
    {
        GeneratePlanets();
    }

    void GeneratePlanets()
    {
        for (int i = 0; i < numberOfPlanets; i++)
        {
            // Instantiate a new planet from the prefab
            GameObject newPlanet = Instantiate(planetPrefab, Vector3.zero, Quaternion.identity);

            // Randomize scale
            float randomScale = Random.Range(scaleRange.x, scaleRange.y);
            newPlanet.transform.localScale = Vector3.one * randomScale;

            // Randomize color
            Renderer renderer = newPlanet.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = new Color(Random.value, Random.value, Random.value);
            }

            // Randomize gravity
            Planet planetScript = newPlanet.GetComponent<Planet>();
            if (planetScript != null)
            {
                planetScript.gravity = Random.Range(gravityRange.x, gravityRange.y);
            }

            // Calculate the next position, randomizing it slightly around a base distance
            var distance = Random.Range(distanceBetweenPlanets.x, distanceBetweenPlanets.y);
            newPlanet.transform.position = Random.onUnitSphere * distance;

            if (i == 0)
            {
                planetScript.enabled = true;
                PlayerController player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
                player.planet = planetScript.transform;
                player.transform.position = planetScript.transform.position - new Vector3(0, randomScale/2, 0);
            }
            else
            {
                planetScript.enabled = false;
            }            
        }

    }
}
