using System.Collections.Generic;
using UnityEngine;

public class PlanetManager : MonoBehaviour
{
    #region Singleton
    public static PlanetManager Instance;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(Instance);  
    }
    #endregion

    public GameObject planetPrefab;  // The Planet prefab
    public int numberOfPlanets = 10; // Number of planets to generate
    public Vector2 distanceBetweenPlanets = new Vector2(200f, 500f); // Minimum distance between planets
    public Vector2 scaleRange = new Vector2(1f, 5f); // Range of scale for the planets
    public Vector2 gravityRange = new Vector2(0.2f, 20f); // Range of gravity for the planets
    private Vector3 startingPosition = Vector3.zero; // Starting position for the first planet

    public List<Planet> Planets = new List<Planet>();
    public Planet ActivePlanet;
    public Rigidbody playerRb;
    
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

            planetScript.rb = playerRb;

            if (i == 0)
            {
                ActivePlanet = planetScript;
                planetScript.enabled = true;
                PlayerController player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
                PlayerManager.Instance.SetAsPlayer(planetScript, planetScript.transform.position - new Vector3(0, randomScale / 2, 0));
            }
            else
            {
                planetScript.enabled = false;
            } 
            Planets.Add(planetScript);
        }
    }

    public void EnablePlanet(Planet planet)
    {
        if (planet == null) return;

        planet.enabled = true;
        ActivePlanet = planet;
    }
    public void DisablePlanet()
    {
        ActivePlanet.enabled = false;
        ActivePlanet = null;
    }
}
