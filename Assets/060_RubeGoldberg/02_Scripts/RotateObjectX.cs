using UnityEngine;

public class RotateObjectX : MonoBehaviour
{
    public float rotationSpeed = 10f; // The speed of rotation in degrees per second

    void Update()
    {
        transform.Rotate(rotationSpeed * Time.deltaTime, 0f, 0f); // Rotate the object around the Y axis
    }
}
