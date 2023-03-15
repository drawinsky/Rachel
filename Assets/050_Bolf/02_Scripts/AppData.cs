using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppData : MonoBehaviour
{
    public string Username = "Default";
    // Start is called before the first frame update
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
