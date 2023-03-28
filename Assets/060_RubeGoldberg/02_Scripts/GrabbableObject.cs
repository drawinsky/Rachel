using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbableObject : MonoBehaviour
{
    public Material highlightMeterial;
    private Material defaultMeterial;

    private void Awake()
    {
        defaultMeterial = GetComponent<Renderer>().material;
    }

    public void SetHighLight(bool value)
    {
        GetComponent<Renderer>().material = value ? highlightMeterial : defaultMeterial;
    }
}
