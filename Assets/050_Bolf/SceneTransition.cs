using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class SceneTransition : MonoBehaviour
{
    public Button transitionBtn;
    public TMP_InputField usernameInput;

    // Start is called before the first frame update
    void Start()
    {
        transitionBtn.onClick.AddListener(() => DoSceneTransition());
    }

    // Update is called once per frame
    void Update()
    {
        FindObjectOfType<AppData>().Username = usernameInput.text;
    }

    private void DoSceneTransition()
    {
        SceneManager.LoadScene("BolfScene");
    }
}
