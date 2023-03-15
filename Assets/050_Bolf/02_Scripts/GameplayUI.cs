using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameplayUI : MonoBehaviour
{
    public TextMeshProUGUI usernameText;

    // Start is called before the first frame update
    void Start()
    {
        AppData appData = FindObjectOfType<AppData>();

        usernameText.text = appData.Username;
    }

}
