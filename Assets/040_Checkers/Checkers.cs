using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Checkers : MonoBehaviour
{
    public GameObject checkerBoard, lightSquarePrefab, darkSquarePrefab; // Checker board elements
    public GameObject[] checkerPrefabs = new GameObject[2];
    public GameObject playerOne, playerTwo;
    public TextMeshProUGUI currentTurnTxt, p1PieceCountTxt, p2PieceCountTxt, gameInfoTxt;
    public Button newGame;
    public int boardSize = 8; // Checker board size
    private int currentPlayer = 1;
    private int[] pieceCount = new int[2];
    private bool hasKilled = false;
    private bool isGameOver = false;
    private Piece[,] pieces;
    private Piece selectedPiece;

    private float squareSize = 1.0f;
    private GameObject[,] squares; // 2D array that stores references to each square on the checker board

    private bool isCrowned = false;
    private GameObject crown;

    public List<GameObject> validMoves;

    private void Start()
    {
        squareSize = 8.0f / boardSize;
        gameInfoTxt.text = "Press NEW GAME button to start checker game.";
        GenerateBoard();
        UpdateUI();
    }

    void GenerateBoard()
    {
        squares = new GameObject[boardSize, boardSize];

        // Instatiate the light and dark square prefabs as child objects of the board
        for (int row = 0; row < boardSize; row++)
        {
            for (int col = 0; col < boardSize; col++)
            {
                // Calculate the position of the square
                float xPos = row * squareSize;
                float zPos = col * squareSize;

                // Determine whether this square should be light or dark
                GameObject squarePrefab;
                if ((row + col) % 2 == 0)
                {
                    squarePrefab = lightSquarePrefab;
                }
                else
                {
                    squarePrefab = darkSquarePrefab;
                }
                //GameObject squarePrefab = ((row + col) % 2 == 0) ? lightSquarePrefab : darkSquarePrefab;

                // Instantiate aquare prefab and set its position
                Vector3 squarePos = new Vector3(xPos, 0, zPos);
                GameObject square = Instantiate(squarePrefab, squarePos, Quaternion.identity);
                square.transform.SetParent(transform);


                // Store the square reference in a 2D array
                squares[row, col] = square;

                // Instatiate players' checkers on the dark squares
                Vector3 checkerPos = new Vector3(xPos, 0.2f, zPos);
                if (row < 3 && (row + col) % 2 != 0)
                {
                    int player = 1;
                    GameObject checker = Instantiate(checkerPrefabs[0], checkerPos, Quaternion.identity);
                    checker.transform.SetParent(playerOne.transform);
                    Piece piece = checker.GetComponent<Piece>();
                    piece.player = player;
                    piece.row = row;
                    piece.col = col;
                    pieces[row, col] = piece;

                    // Find and Hide the crown object as a child of this checker
                    crown = checker.transform.Find("Crown").gameObject;
                    CheckCrown();

                    // Increment player one's piece count
                    pieceCount[0]++;
                }
                else if (row > 4 && (row + col) % 2 != 0)
                {
                    int player = 2;
                    GameObject checker = Instantiate(checkerPrefabs[1], checkerPos, Quaternion.identity);
                    checker.transform.SetParent(playerTwo.transform);
                    Piece piece = checker.GetComponent<Piece>();
                    piece.player = player;
                    piece.row = row;
                    piece.col = col;
                    pieces[row, col] = piece;

                    // Find and Hide the crown object as a child of this checker
                    crown = checker.transform.Find("Crown").gameObject;
                    CheckCrown();

                    // Increment player two's piece count
                    pieceCount[1]++;
                }
            }
        }
    }
    void InitGame()
    {
        currentPlayer = 1;
        hasKilled = false;
        DeselectPiece();
    }

    void DeselectPiece()
    {
        if (selectedPiece != null)
        {
            selectedPiece = null;
        }
    }

    void CheckCrown()
    {
        if (isCrowned == true)
        {
            crown.SetActive(true);
        }
        else
        {
            crown.SetActive(false);
        }
    }

    public bool IsCrowned()
    {
        return isCrowned;
    }

    void UpdateUI()
    {
        // Update the current player text.
        currentTurnTxt.text = "Current turn: Player - " + currentPlayer;

        // Update the piece count text.
        p1PieceCountTxt.text = "Player - 1  pieces: " + pieceCount[0];
        p2PieceCountTxt.text = "Player - 2 pieces: " + pieceCount[1];
    }

    void NextTurn()
    {
        // Switch to the other player.
        currentPlayer = (currentPlayer == 1) ? 2 : 1;

        // Update the UI to display the current player.
        UpdateUI();
    }

    void GameOver(int winner)
    {
        // Display a message indicating the winner or a draw.
        if (winner == 0)
        {
            gameInfoTxt.text = "Draw!";
        }
        else
        {
            gameInfoTxt.text = "Player " + winner + " wins!";
        }

        // Set the isGameOver flag to true to prevent further input.
        isGameOver = true;
    }

    void CheckForGameOver()
    {
        // Check if either player has no valid moves remaining or has lost all their pieces
        bool hasValidMovesP1 = false;
        bool hasValidMovesP2 = false;
        int p1PieceCount = 0;
        int p2PieceCount = 0;

        // Loop through all the pieces on the board
        for (int row = 0; row < boardSize; row++)
        {
            for (int col = 0; col < boardSize; col++)
            {
                GameObject piece = squares[row, col];
                if (piece != null)
                {
                    Piece pieceScript = piece.GetComponent<Piece>();
                    if (pieceScript != null)
                    {
                        // Check if the piece belongs to player 1 or player 2
                        if (pieceScript.player == 1)
                        {
                            p1PieceCount++;
                            // Check if the piece has any valid moves
                            if (pieceScript.HasValidMoves())
                            {
                                hasValidMovesP1 = true;
                            }
                        }
                        else if (pieceScript.player == 2)
                        {
                            p2PieceCount++;
                            // Check if the piece has any valid moves
                            if (pieceScript.HasValidMoves())
                            {
                                hasValidMovesP2 = true;
                            }
                        }
                    }
                }
            }
        }

        // Check if either player has lost all their pieces
        if (p1PieceCount == 0 || p2PieceCount == 0)
        {
            isGameOver = true;
        }

        // Check if either player has no valid moves remaining
        if (!hasValidMovesP1 || !hasValidMovesP2)
        {
            isGameOver = true;
        }

        // If the game is over, display a message indicating the winner or a draw
        if (isGameOver)
        {
            if (p1PieceCount == 0)
            {
                Debug.Log("Player 2 wins!");
            }
            else if (p2PieceCount == 0)
            {
                Debug.Log("Player 1 wins!");
            }
            else
            {
                Debug.Log("It's a draw!");
            }
        }
    }
}

//Game initialization:
//  Create a 2D array to represent the game board.
//  Set up the pieces on the board according to the rules of checkers.
//  Initialize variables to keep track of the current player, the number of pieces each player has, and whether the game is over.

//Mouse input:
//  Check for mouse clicks on the game board.
//  If the player clicks on a piece, highlight the piece and show valid moves by highlighting the squares where the piece can move.
//  If the player clicks on a valid move, move the piece to the new square and update the game state.

//Valid moves:
//  Check if a move is valid by checking if the destination square is empty or occupied by an enemy piece.
//  If the destination square is occupied by an enemy piece, check if the square beyond it is empty. If it is, remove the captured piece from the game board and update the player's piece count.
//  If a piece reaches the end of the board, crown the piece and update its movement rules.

//Game over:
//  Check if either player has no valid moves remaining or has lost all their pieces.
//  If the game is over, display a message indicating the winner or a draw.
