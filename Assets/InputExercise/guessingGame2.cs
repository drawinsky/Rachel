using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class guessingGame2 : MonoBehaviour
{
    // A reference to the text object
    public TextMeshProUGUI textGameObject;
    public TMP_InputField inputName;
    public TMP_InputField inputAge;

    public void MyFunction()
    {
        textGameObject.text = "Hello " + inputName.text + " , you are " + inputAge.text + " years old.";


        Debug.Log("HELLO!!!!!!");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
