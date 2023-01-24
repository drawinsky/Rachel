using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class guessingGame : MonoBehaviour
{
    // A reference to the text object
    public TextMeshProUGUI gameText;
    public TMP_InputField upperBoundInput;
    public Button startButton;
    public TMP_InputField guessInput;
    public Button guessButton;

    private int upperBound;
    private int randomNum;


    // Start is called before the first frame update
    void Start()
    {
        startButton.onClick.AddListener(StartGame);
        guessButton.onClick.AddListener(GuessNumber);
        gameText.text = "Guessing Game! Pick a upper number to start the game :)";

        upperBoundInput.gameObject.SetActive(true);
        startButton.gameObject.SetActive(true);
        guessInput.gameObject.SetActive(false);
        guessButton.gameObject.SetActive(false);

    }

    void StartGame() 
    {
        upperBoundInput.gameObject.SetActive(false);
        startButton.gameObject.SetActive(false);
        guessInput.gameObject.SetActive(true);
        guessButton.gameObject.SetActive(true);

        upperBound = int.Parse(upperBoundInput.text);
        randomNum = Random.Range(1, upperBound + 1);
        gameText.text = "Okay! The number is between 1 and " + upperBoundInput.text + ". Now take a guess!";
        // gameText.text = $"Okay! The number is between 1 and {upperBoundInput.text}. Now take a guess!";

        Debug.Log(randomNum);
    }

    void GuessNumber()
    {
        int guessNum = int.Parse(guessInput.text);
        if (guessNum > randomNum)
        {
            gameText.text = "Try a lower number";
        }
        else if (guessNum < randomNum)
        {
            gameText.text = "Try a higher number";
        }
        else
        {
            gameText.text = "Congratulations!";
            guessInput.gameObject.SetActive(false);
            guessButton.gameObject.SetActive(false);
        }

        guessInput.text = "";
    }

}