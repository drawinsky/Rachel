using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BowlingBallController : MonoBehaviour
{
    public float launchVelocity = 10f; // The velocity at which the ball is launched
    public float arrowRotationSpeed = 2f; // The speed at which the arrow rotates
    public Transform arrowTransform; // The Transform component of the arrow
    public float gutterHeight = -10f; // The y-position below which the ball is considered to have fallen off the ledge
    public TextMeshProUGUI scoreText; // The UI Text object used to display the score
    public TextMeshProUGUI resultText; // The UI Text object used to display the result (e.g. "STRIKE", "GUTTER", etc.)
    public Button rePlayBtn;
    public Button MenuBtn;
    public GameObject pinHolder;

    private bool isLaunched = false; // Indicates whether the ball has been launched
    private Rigidbody ballRigidbody; // The Rigidbody component of the ball
    private int _score = 0; // The player's score
    private int _pinsKnockedDown = 0; // The number of pins knocked down by the ball or other pins
    public bool _secondRound = false;


    // Start is called before the first frame update
    void Start()
    {
        ballRigidbody = GetComponent<Rigidbody>();
        //arrowTransform = transform.GetChild(0);

    }

    // Update is called once per frame
    void Update()
    {
        if (!isLaunched)
        {
            // Rotate the arrow back and forth between -45 and 45 degrees (y-axis)
            float rotationAngle = 45f * Mathf.Sin(Time.time * arrowRotationSpeed);
            arrowTransform.localRotation = Quaternion.Euler(0f, rotationAngle, 0f);

            // Launch the ball when the user presses SPACE key
            if (Input.GetKeyDown(KeyCode.Space))
            {
                transform.localRotation = arrowTransform.rotation;
                ballRigidbody.velocity = transform.forward * launchVelocity;
                arrowTransform.gameObject.SetActive(false);
                isLaunched = true;
            }
        }
        else
        {
            CountScore();
            
            if (_secondRound)
            {

            }
            
        }
    }

    int CountFallenPins()
    {
        foreach (Transform child in pinHolder.transform)
        {
            Pin pin = child.GetComponent<Pin>();
            if (pin.GetComponent<Pin>()._done)
            {
                _pinsKnockedDown++;
            }
        }

        return _pinsKnockedDown;
    }

    void CountScore()
    {
        CountFallenPins();

        if (_pinsKnockedDown == pinHolder.transform.childCount)
        {
            // all pins have fallen down, it's a strike
            _score += 10;
            resultText.text = "STRIKE!";
        }
        else if (_pinsKnockedDown != 0 && _pinsKnockedDown != pinHolder.transform.childCount)
        {
            // count the number of pins knocked down
            int knockedCount = _pinsKnockedDown - _score;
            _score += knockedCount;
            scoreText.text = knockedCount + " pins knocked down!";
        }
        else if (_pinsKnockedDown == 0 && transform.position.y < gutterHeight)
        {
            // If the ball has fallen off the ledge, reset the score and update the UI
            _score = 0;
            _pinsKnockedDown = 0;
            _secondRound = true;

            // Display the result text
            resultText.text = "GUTTER";
        }
    }

    void UpdateScoreUI()
    {
    
    }
}
