using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private Cam3 camManager;

    public PlayerController player;
    public SpaceshipController spaceship;

    private void Start()
    {
        camManager = FindAnyObjectByType<Cam3>();
        SetAsPlayer();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            SetAsPlayer();
        if (Input.GetKeyDown(KeyCode.O))
            SetAsSpaceship();
    }

    private void SetAsPlayer()
    {
        player.gameObject.SetActive(true); 
        spaceship.gameObject.SetActive(false);
        camManager.player = player.transform;
    }

    private void SetAsSpaceship()
    {
        spaceship.gameObject.SetActive(true);
        player.gameObject.SetActive(false);
        camManager.player = spaceship.transform;
    }
}
