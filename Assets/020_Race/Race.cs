using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Race : MonoBehaviour
{
    public GameObject player1, player2;
    public GameObject GrassTerrain, MudTerrain;
    public Button player1Button;
    public Button player2Button;
    public Button startGameButton;
    public Button newMapButton;
    public TMP_Text resultText;
    public float turnDuration = 1;

    private List<float> chanceToMoveList = new List<float>();
    private int playerGuess = 0;
    private bool gameOver = false;
    private int currentTurn1, currentTurn2;
    private Vector3 playerPos1 = new Vector3(-20, 0, -1);
    private Vector3 playerPos2 = new Vector3(-20, 5, -1);
    private Vector3 updatePlayerPos1;
    private Vector3 updatePlayerPos2;

    // Start is called before the first frame update
    void Start()
    {
        startGameButton.gameObject.SetActive(false);
        newMapButton.gameObject.SetActive(false);
        player1Button.gameObject.SetActive(true);
        player2Button.gameObject.SetActive(true);
        player1Button.onClick.AddListener(Player1Guess);
        player2Button.onClick.AddListener(Player2Guess);
        startGameButton.onClick.AddListener(StartRace);
        newMapButton.onClick.AddListener(GenerateNewMap);

        resultText.text = "Click a button to guess which player will win the game.";

        GenerateNewMap();

    }

    // Generate the map
    private void GenerateNewMap()
    {
        // Delete all the current Terrain tiles
        GameObject[] terrainTiles = GameObject.FindGameObjectsWithTag("Terrain");
        for (int i = 0; i < terrainTiles.Length; i++)
        {
            Destroy(terrainTiles[i]);
            Debug.Log("Destroy(terrainTiles[i]) " + i);
        }


        for (int i = 0; i < 11; i++)
        {
            int terrain = Random.Range(0, 2);
            if (terrain == 0)
            {
                Instantiate(GrassTerrain, new Vector3(-15 + (i * 5), 2, 0), Quaternion.identity);
                Debug.Log(i + " Instantiate: GrassTerrain");
                float tileChaneceToMove = GrassTerrain.GetComponent<Tile>().ChanceToMove;
                chanceToMoveList.Add(tileChaneceToMove);
            }
            else
            {
                Instantiate(MudTerrain, new Vector3(-15 + (i * 5), 2, 0), Quaternion.identity);
                Debug.Log(i + " Instantiate: MudTerrain");
                float tileChaneceToMove = MudTerrain.GetComponent<Tile>().ChanceToMove;
                chanceToMoveList.Add(tileChaneceToMove);
            }
            Debug.Log(i + " chanceToMove: " + chanceToMoveList[i]);
        }
    }

    // Player 1 guess
    private void Player1Guess()
    {
        startGameButton.gameObject.SetActive(true);
        newMapButton.gameObject.SetActive(true);
        resultText.text = "You have guessed Player 1 will win.";
        playerGuess = 1;
    }

    // Player 2 guess
    private void Player2Guess()
    {
        startGameButton.gameObject.SetActive(true);
        newMapButton.gameObject.SetActive(true);
        resultText.text = "You have guessed Player 2 will win.";
        playerGuess = 2;
    }

    private void StartRace()
    {
        startGameButton.gameObject.SetActive(false);
        newMapButton.gameObject.SetActive(false);
        player1Button.gameObject.SetActive(false);
        player2Button.gameObject.SetActive(false);

        player1.transform.position = playerPos1;
        player2.transform.position = playerPos2;
        gameOver = false;
        StartCoroutine(RaceGame());
    }

    private IEnumerator RaceGame()
    {
        yield return new WaitForSeconds(turnDuration);

        Vector3 updatePlayerPos1 = new Vector3(playerPos1.x + 5, playerPos1.y, playerPos1.z);
        Vector3 updatePlayerPos2 = new Vector3(playerPos2.x + 5, playerPos2.y, playerPos2.z);
        player1.transform.position = updatePlayerPos1;
        player2.transform.position = updatePlayerPos2;

        yield return new WaitForSeconds(turnDuration);

        currentTurn1 = 0;
        currentTurn2 = 0;

        while (!gameOver)
        {
            GameUpdate();
            Debug.Log($"Turn {currentTurn1}");
            Debug.Log($"Turn {currentTurn2}");

            yield return new WaitForSeconds(turnDuration);

            

            if (currentTurn1 == 11)
            {
                gameOver = true;
                if (playerGuess == 1)
                {
                    resultText.text = "Player 1 win! You win!";
                }
                else
                {
                    resultText.text = "Player 1 win! Try again QQ";
                }
            }

            if (currentTurn2 == 11)
            {
                gameOver = true;
                if (playerGuess == 2)
                {
                    resultText.text = "Player 2 win! You win!";
                }
                else
                {
                    resultText.text = "Player 2 win! Try again QQ";
                }
            }
        }
        if (gameOver == true)
        {
            player1Button.gameObject.SetActive(true);
            player2Button.gameObject.SetActive(true);
        }

    }

    private void GameUpdate()
    {
        float random1 = Random.Range(0f, 1f);
        float random2 = Random.Range(0f, 1f);

        if (random1 <= chanceToMoveList[currentTurn1])
        {
            updatePlayerPos1 = new Vector3(playerPos1.x + (currentTurn1 + 2) * 5, playerPos1.y, playerPos1.z);
            player1.transform.position = updatePlayerPos1;
            //Debug.Log(chanceToMoveList[currentTurn1]);
            //Debug.Log(player1.transform.position);
            //Debug.Log(updatePlayerPos1);
            currentTurn1++;
        }
        if (random2 <= chanceToMoveList[currentTurn2])
        {
            updatePlayerPos2 = new Vector3(playerPos2.x + (currentTurn2 + 2) * 5, playerPos2.y, playerPos2.z);
            player2.transform.position = updatePlayerPos2;
            //Debug.Log(chanceToMoveList[currentTurn2]);
            //Debug.Log(player2.transform.position);
            //Debug.Log(updatePlayerPos2);
            currentTurn2++;
        }
    }
}
