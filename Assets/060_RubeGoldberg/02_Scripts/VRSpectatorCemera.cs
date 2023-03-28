using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRSpectatorCemera : MonoBehaviour
{
    public Transform target;
    public float translateSpeed = 4, rotateSpeed = 4;


    // Using LateUpdate ensures all logic that controls the target
    // headset has executed.
    private void LateUpdate()
    {
        // Lerping (Linear IntERPolation)
        Vector3 targetPos = target.position;
        Quaternion targetRot = target.rotation;

        // Smoothly move an object from its current position to the target position over time with a specific speed
        transform.position = Vector3.Lerp(transform.position, targetPos, translateSpeed * Time.deltaTime);
        // "Slerp" = "Spherical" Lerp, makes angles do better.
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotateSpeed * Time.deltaTime);
    }
}
