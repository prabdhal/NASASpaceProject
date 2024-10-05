using UnityEngine;

public class PlanetRotate : MonoBehaviour
{
    public float rotationSpeed = 10f; // Speed of rotation

    void Update()
    {
        // Rotate the planet around its own axis
        transform.Rotate(Vector3.right, rotationSpeed * Time.deltaTime);
    }
}
