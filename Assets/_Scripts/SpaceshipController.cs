using System.Collections;
using UnityEngine;

public class SpaceshipController : MonoBehaviour
{
    // Movement variables
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float rotationSpeed = 100f;

    // Mouse rotation variables
    [SerializeField] private float mouseSensitivity = 2f;

    // Raycast variables
    [SerializeField] private float raycastDistance = 5f;
    [SerializeField] private LayerMask planetLayer;


    [SerializeField] private float switchBuffer = 2f;
    private float nextTimeSwitch;

    public bool isStationed = true;

    private void Update()
    {
        if (isStationed)
            return;

        HandleMovement();
        HandleMouseRotation();
    }

    private void HandleMovement()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(moveHorizontal, 0, moveVertical);
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.Self);
    }

    private void HandleMouseRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        transform.Rotate(Vector3.up, mouseX * rotationSpeed * Time.deltaTime, Space.World);
        transform.Rotate(Vector3.left, mouseY * rotationSpeed * Time.deltaTime, Space.Self);
    }

    private void ShootRaycast()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);

        if (Physics.Raycast(transform.position, forward, out RaycastHit hit, raycastDistance, planetLayer))
        {
            Debug.Log("Hit a planet: " + hit.collider.name);
            HandlePlanetHit(hit);
        }
    }

    private void HandlePlanetHit(RaycastHit hit)
    {
        Debug.Log("Planet hit: " + hit.transform.name);
        PlayerManager.Instance.SetAsPlayer(hit.transform.GetComponent<Planet>(), hit.point);
    }

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }

    public void EnableSpaceship()
    {
        isStationed = false;
        nextTimeSwitch = Time.time + switchBuffer;
    }

    public void DisableSpaceship()
    {
        isStationed = true;
        nextTimeSwitch = Time.time + switchBuffer;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player" && Time.time >= nextTimeSwitch)
        {
            PlayerManager.Instance.SetAsSpaceship();
        }
        if (other.gameObject.tag == "Planet" && Time.time >= nextTimeSwitch)
        {
            PlayerManager.Instance.SetAsPlayer(other.gameObject.GetComponent<Planet>(), other.contacts[0].point);
        }
    }
}
