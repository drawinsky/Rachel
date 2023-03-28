using UnityEngine;

public class RotateObjectZ : MonoBehaviour
{
    public float rotationSpeed = 10f; // The speed of rotation in degrees per second

    void Update()
    {
        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime); // Rotate the object around the Y axis
    }
}
