using UnityEngine;
using UnityEngine.Events;

public class Global : MonoBehaviour
{
    // Global references for easy access
    public ModeManager modeManager;
    public PlayerController player;
    public Rigidbody playerRb;
    public SpaceshipController spaceship;
    public PlanetManager planetManager; 
    public Planet currentPlanet;
    public PlayerCamera playerCamera;
    public ShipCamera shipCamera;

    // Global events for separation of concerns
    public UnityEvent OnPlayerMode = new();
    public UnityEvent OnSpaceshipMode = new();
}
