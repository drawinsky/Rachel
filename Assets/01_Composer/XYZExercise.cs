using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class XYZExercise : MonoBehaviour
{
    public TextMeshProUGUI xyzText;
    public Button changeButton;
    public TMP_InputField xInput;
    public TMP_InputField yInput;
    public TMP_InputField zInput;

    private int xPosition, yPosition, zPosition;

    // Start is called before the first frame update
    void Start()
    {
        changeButton.onClick.AddListener(ChangePosition);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ChangePosition()
    {
        xPosition = int.Parse(xInput.text);
        yPosition = int.Parse(yInput.text);
        zPosition = int.Parse(zInput.text);

        Transform newPosition = xyzText.GetComponent<Transform>();
        newPosition.position = new Vector3(xPosition, yPosition, zPosition);
    }
}
