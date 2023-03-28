using UnityEngine;

public class RotateObjectY : MonoBehaviour
{
    public float rotationSpeed = 10f; // The speed of rotation in degrees per second

    void Update()
    {
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f); // Rotate the object around the Y axis
    }
}
