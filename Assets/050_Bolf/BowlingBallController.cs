using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

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
    public GameObject bowlingBall;
    public List<GameObject> pins = new List<GameObject>();

    private Vector3 bowlingBallPos;
    private List<Vector3> pinsPos = new List<Vector3>();
    private Quaternion bowlingBallRot;
    private List<Quaternion> pinsRot = new List<Quaternion>();
    private int _score = 0; // The player's score

    private bool isLaunched = false; // Indicates whether the ball has been launched
    private Rigidbody ballRigidbody; // The Rigidbody component of the ball
    
    private int _pinsKnockedDown = 0; // The number of pins knocked down by the ball or other pins
    public bool _secondRound = false;
    private bool _endGame = false;
    private int _firstScore = 0;


    // Start is called before the first frame update
    void Start()
    {
        ballRigidbody = GetComponent<Rigidbody>();
        //arrowTransform = transform.GetChild(0);

        MenuBtn.onClick.AddListener(() => DoSceneTransition());
        rePlayBtn.onClick.AddListener(() => ReplayGame());
        MenuBtn.gameObject.SetActive(false);
        rePlayBtn.gameObject.SetActive(false);

        bowlingBallPos = bowlingBall.transform.position;
        bowlingBallRot = bowlingBall.transform.rotation;
        for (int i = 0; i < 10; i++)
        {
            pinsPos.Add(pins[i].transform.position);
            pinsRot.Add(pins[i].transform.rotation);
        }


    }

    // Update is called once per frame
    void Update()
    {
        if (!isLaunched)
        {
            resultText.text = "Press SPACE key to roll the ball.";

            // Rotate the arrow back and forth between -45 and 45 degrees (y-axis)
            float rotationAngle = 45f * Mathf.Sin(Time.time * arrowRotationSpeed);
            arrowTransform.localRotation = Quaternion.Euler(0f, rotationAngle, 0f);
            if (_secondRound)
            {
                _endGame = true;
                resultText.text = "Round2! Press SPACE key to roll the ball.";
            }

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

            if (transform.position.y < gutterHeight && _endGame == false)
            {
                _secondRound = true;
                isLaunched = false;

                arrowTransform.gameObject.SetActive(true);
                ResetGameObject();
            }
            else if (transform.position.y < gutterHeight && _endGame == true)
            {
                MenuBtn.gameObject.SetActive(true);
                rePlayBtn.gameObject.SetActive(true);
            }
            
        }
    }

    int CountFallenPins()
    {
        _pinsKnockedDown = 0;

        foreach (Transform child in pinHolder.transform)
        {
            //Pin pin = child.GetComponent<Pin>();
            if (child.GetComponent<Pin>()._done == true)
            {
                _pinsKnockedDown++;
            }
        }
        Debug.Log("_pinsKnockedDown:" + _pinsKnockedDown);
        return _pinsKnockedDown;
    }

    void CountScore()
    {
        if (!_secondRound)
        {
            _firstScore = 0;
        }

        CountFallenPins();
        _score = _firstScore;

        if (_pinsKnockedDown == 10)
        {
            // all pins have fallen down, it's a strike
            _score += 10;
            resultText.text = "STRIKE!";
        }
        else if (_pinsKnockedDown != 0 && _pinsKnockedDown != pinHolder.transform.childCount)
        {
            // count the number of pins knocked down
            _score += _pinsKnockedDown;
            resultText.text = _pinsKnockedDown + " pins knocked down!";
        }
        else if (_pinsKnockedDown == 0 && transform.position.y < gutterHeight)
        {
            // If the ball has fallen off  the ledge, reset the score and update the UI
            _score += 0;
            _pinsKnockedDown = 0;
            resultText.text = "GUTTER";
        }

        scoreText.text = "SCORE: " + _score;

        if (!_secondRound)
        {
            _firstScore = _score;
        }
        
    }

    private void DoSceneTransition()
    {
        SceneManager.LoadScene("MenuScene");
    }

    void ResetGameObject()
    {
        //bowlingBallTransform = bowlingBallStartTransform;
        bowlingBall.transform.position = bowlingBallPos;
        bowlingBall.transform.rotation = bowlingBallRot;
        bowlingBall.GetComponent<Rigidbody>().velocity = Vector3.zero;
        bowlingBall.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

        for (int i = 0; i < pins.Count; i++)
        {
            pins[i].transform.position = pinsPos[i];
            pins[i].transform.rotation = pinsRot[i];
            pins[i].GetComponent<Pin>()._done = false;
            pins[i].GetComponent<Rigidbody>().velocity = Vector3.zero;
            pins[i].GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }
    }

    void ReplayGame()
    {
        _score = 0;
        _firstScore = 0;
        isLaunched = false;
        _pinsKnockedDown = 0;
        _secondRound = false;
        _endGame = false;

        ResetGameObject();
    }
}
