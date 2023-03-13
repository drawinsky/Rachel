using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppBootstrap : MonoBehaviour
{
    public AppData appDataPrefab;

    private void Awake()
    {
        if (FindObjectOfType<AppData>() == null)
        {
            Instantiate(appDataPrefab);
        }

        Destroy(gameObject);
    }

   
}
