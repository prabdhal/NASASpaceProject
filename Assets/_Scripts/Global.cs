using UnityEngine;

public class Global : MonoBehaviour
{
    public ModeManager modeManager;
    public PlayerController player;
    public Rigidbody playerRb;
    public SpaceshipController spaceship;
    public PlanetManager planetManager; 
    public Planet currentPlanet;

    public PlayerCamera playerCamera;
    public ShipCamera shipCamera;
}
