using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
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
    private bool isValidMove = false;
    private Piece[,] pieces;
    private Piece selectedPiece;
    public Material highlightMaterial;

    private float squareSize = 1.0f;
    private GameObject[,] squares; // 2D array that stores references to each square on the checker board

    private GameObject crown;

    public List<GameObject> validMoves;

    private void Start()
    {
        squareSize = 8.0f / boardSize;
        gameInfoTxt.text = "Press NEW GAME button to start checker game.";

        GenerateBoard();
        InitGame();
        UpdateUI();
        newGame.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // If the player clicked on a piece
                if (hit.collider.CompareTag("Piece"))
                {
                    GameObject selectedObj = hit.collider.gameObject;
                    int xHit = Mathf.RoundToInt(hit.point.x);
                    int yHit = Mathf.RoundToInt(hit.point.z);
                    selectedPiece = pieces[xHit, yHit];
                    selectedPiece.isSelected = true;

                    // Check if it's the current player's turn
                    if (selectedPiece.player != currentPlayer || IsGameOver()) return;

                    // Check if the player clicked on a valid move
                    if (Mouse.current.leftButton.wasPressedThisFrame)
                    {                        
                        if (Physics.Raycast(ray, out hit))
                        {
                            // If the player clicked on a square
                            if (hit.collider.CompareTag("Square"))
                            {
                                GameObject square = hit.collider.gameObject;
                                for (int row = 0; row < boardSize; row++)
                                {
                                    for (int col = 0; col < boardSize; col++)
                                    {
                                        if (squares[row,col] = square)
                                        {
                                            int xPos = row;
                                            int yPos = col;

                                            // If the square is a valid move for the selected piece
                                            if (IsValidMove(xPos, yPos))
                                            {
                                                // Move the piece to the selected square
                                                MovePiece(selectedPiece, xPos, yPos);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                selectedPiece.isSelected = false;
            }
        }
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
                    GameObject checker = Instantiate(checkerPrefabs[0], checkerPos, Quaternion.identity);
                    checker.transform.SetParent(playerOne.transform);
                    Piece piece = checker.GetComponent<Piece>();
                    piece.player = 1;
                    piece.row = row;
                    piece.col = col;
                    pieces[row, col] = piece;
                    

                    // Find and Hide the crown object as a child of this checker
                    Transform crownTransform = checker.transform.Find("Crown");
                    if (crownTransform != null)
                    {
                        crown = crownTransform.gameObject;
                        crown.SetActive(false);
                    }

                    // Increment player one's piece count
                    pieceCount[0]++;
                }
                else if (row > 4 && (row + col) % 2 != 0)
                {
                    GameObject checker = Instantiate(checkerPrefabs[1], checkerPos, Quaternion.identity);
                    checker.transform.SetParent(playerTwo.transform);
                    Piece piece = checker.GetComponent<Piece>();
                    piece.player = 2;
                    piece.row = row;
                    piece.col = col;
                    pieces[row, col] = piece;
                    

                    // Find and Hide the crown object as a child of this checker
                    crown = checker.transform.Find("Crown").gameObject;
                    crown.SetActive(false);

                    // Increment player two's piece count
                    pieceCount[1]++;
                }
            }
        }
    }
    
    void UpdateUI()
    {
        // Update the current player text.
        currentTurnTxt.text = "Current Turn: " + ((currentPlayer == 1) ? "Player 1" : "Player 2");

        // Update the piece count text.
        p1PieceCountTxt.text = "Player - 1  pieces: " + pieceCount[0];
        p2PieceCountTxt.text = "Player - 2 pieces: " + pieceCount[1];
    }

    void InitGame()
    {
        currentPlayer = 1;
        hasKilled = false;
        DeselectPiece();
    }

    public bool IsValidMove(int row, int col)
    {
        // Check if the move is within the bounds of the board
        if (row < 0 || row >= boardSize || col < 0 || col >= boardSize)
        {
            return false;
        }

        // Check if the target square is unoccupied
        Piece targetPiece = pieces[row, col];
        if (targetPiece == null)
        {
            return true;
        }

        // Check if the target piece is an opponent's piece
        if (targetPiece.player != currentPlayer)
        {
            // Check if the capture move is possible
            int targetRow = row + (row - selectedPiece.row);
            int targetCol = col + (col - selectedPiece.col);
            if (targetRow < 0 || targetRow >= boardSize || targetCol < 0 || targetCol >= boardSize)
            {
                return false;
            }

            // Check if the square beyond the opponent's piece is unoccupied
            Piece jumpTarget = pieces[targetRow, targetCol];
            if (jumpTarget == null)
            {
                return true;
            }
        }

        return false;
    }

    public void SelectPiece(Piece piece)
    {
        // Check if the piece belongs to the current player
        if (piece.player != currentPlayer)
        {
            return;
        }

        // Deselect the previously selected piece
        if (selectedPiece != null)
        {
            selectedPiece.isSelected = false;
        }

        // Select the new piece
        selectedPiece = piece;
        selectedPiece.isSelected = true;

        // Find and show valid moves for this piece
        FindValidMoves();
    }

    void FindValidMoves()
    {
        validMoves = new List<GameObject>();

        // Check for regular moves
        if (selectedPiece.isCrowned == true)
        {
            CheckDirection(selectedPiece.row + 1, selectedPiece.col + 1);
            CheckDirection(selectedPiece.row - 1, selectedPiece.col + 1);
            CheckDirection(selectedPiece.row + 1, selectedPiece.col - 1);
            CheckDirection(selectedPiece.row - 1, selectedPiece.col - 1);
        }
        else
        {
            int direction = (selectedPiece.player == 1) ? 1 : -1;
            CheckDirection(selectedPiece.row + direction, selectedPiece.col + 1);
            CheckDirection(selectedPiece.row + direction, selectedPiece.col - 1);
        }

        // Check for capture moves
        if (hasKilled)
        {
            foreach (GameObject move in validMoves)
            {
                int row = Mathf.RoundToInt(move.transform.position.x / squareSize);
                int col = Mathf.RoundToInt(move.transform.position.z / squareSize);

                if (Mathf.Abs(selectedPiece.row - row) == 2 && Mathf.Abs(selectedPiece.col - col) == 2)
                {
                    int captureRow = selectedPiece.row + (row - selectedPiece.row) / 2;
                    int captureCol = selectedPiece.col + (col - selectedPiece.col) / 2;
                    Piece capturedPiece = pieces[captureRow, captureCol];
                    if (capturedPiece != null && capturedPiece.player != currentPlayer)
                    {
                        // Remove the captured piece from the board and update the piece count
                        Destroy(capturedPiece.gameObject);
                        pieces[captureRow, captureCol] = null;
                        pieceCount[capturedPiece.player - 1]--;
                        hasKilled = true;
                    }
                }
            }
        }

        // Show valid moves
        foreach (GameObject move in validMoves)
        {
            move.SetActive(true);
        }
    }

    void CheckDirection(int row, int col)
    {
        int targetRow = row + (row - selectedPiece.row);
        int targetCol = col + (col - selectedPiece.col);
        if (targetRow < 0 || targetRow >= boardSize || targetCol < 0 || targetCol >= boardSize)
        {
            return;
        }

        Piece targetPiece = pieces[targetRow, targetCol];

        // If the target square is unoccupied, add it to the valid moves list
        if (targetPiece == null)
        {
            validMoves.Add(squares[targetRow, targetCol]);
            return;
        }

        // If the target square is occupied by an opponent's piece, check if we can capture it
        if (targetPiece.player != currentPlayer)
        {
            int captureRow = targetRow + (targetRow - row);
            int captureCol = targetCol + (targetCol - col);

            // Check if the capture move is within the bounds of the board
            if (captureRow < 0 || captureRow >= boardSize || captureCol < 0 || captureCol >= boardSize)
            {
                return;
            }

            // Check if the square beyond the opponent's piece is unoccupied
            Piece capturePiece = pieces[captureRow, captureCol];
            if (capturePiece == null)
            {
                // This is a valid capture move, so add the target square to the valid moves list
                validMoves.Add(squares[targetRow, targetCol]);
            }
        }
    }

    public List<Vector2Int> GetValidMoves(Piece piece)
    {
        List<Vector2Int> validMoves = new List<Vector2Int>();

        if (piece.isCrowned == true)
        {
            // Check in all four directions
            CheckDirection(selectedPiece.row - 1, selectedPiece.col - 1);
            CheckDirection(selectedPiece.row - 1, selectedPiece.col + 1);
            CheckDirection(selectedPiece.row + 1, selectedPiece.col - 1);
            CheckDirection(selectedPiece.row + 1, selectedPiece.col + 1);
        }
        else
        {
            // Check in the forward direction for player 1, and the backward direction for player 2
            int forward = (currentPlayer == 1) ? 1 : -1;

            // Check in the two diagonal directions
            CheckDirection(selectedPiece.row + forward, selectedPiece.col - 1);
            CheckDirection(selectedPiece.row + forward, selectedPiece.col + 1);
        }

        // Check for capture moves
        List<Vector2Int> captureMoves = new List<Vector2Int>();
        if (hasKilled == true)
        {
            // Check in all four directions
            CheckDirection(selectedPiece.row - 1, selectedPiece.col - 1);
            CheckDirection(selectedPiece.row - 1, selectedPiece.col + 1);
            CheckDirection(selectedPiece.row + 1, selectedPiece.col - 1);
            CheckDirection(selectedPiece.row + 1, selectedPiece.col + 1);
        }

        if (captureMoves.Count > 0)
        {
            validMoves = captureMoves;
        }

        return validMoves;
    }


    public bool IsGameOver()
    {
        // Check if any player has no pieces remaining
        if (pieceCount[0] == 0 || pieceCount[1] == 0)
        {
            isGameOver = true;
            return true;
        }

        // Check if the current player has any valid moves
        List<Vector2Int> moves = new List<Vector2Int>();
        foreach (Piece piece in pieces)
        {
            if (piece == null || piece.player != currentPlayer) continue;
            moves.AddRange(GetValidMoves(piece));
        }

        if (moves.Count == 0)
        {
            isGameOver = true;
            return true;
        }

        return false;
    }


    public void MovePiece(Piece piece, int row, int col)
    {
        if (selectedPiece != null)
        {
            if (IsValidMove(row, col))
            {
                // Move the selected piece to the new square
                Vector3 checkerPos = new Vector3(row * squareSize, 0.2f, col * squareSize);
                piece.transform.position = checkerPos;
                pieces[piece.row, piece.col] = null;
                piece.row = row;
                piece.col = col;
                pieces[row, col] = piece;

                CheckForCrown(piece);
            }

            // Check if the piece has captured an enemy piece
            if (Mathf.Abs(row - piece.row) == 2)
            {
                hasKilled = true;
                int killRow = (piece.row + row) / 2;
                int killCol = (piece.col + col) / 2;
                Piece killedPiece = pieces[killRow, killCol];
                pieceCount[killedPiece.player - 1]--;
                Destroy(killedPiece.gameObject);
                pieces[killRow, killCol] = null;
                // Check if there are any more valid moves for this piece
                if (GetValidMoves(selectedPiece).Count > 0 && piece.isCrowned == false)
                {
                    SelectPiece(selectedPiece);
                    return;
                }
            }

            // End the turn if the piece didn't capture an enemy piece or there are no more valid moves
            if (hasKilled == false || GetValidMoves(selectedPiece).Count == 0 || piece.isCrowned == true)
            {
                EndTurn();
            }
            else
            {
                SelectPiece(selectedPiece);
            }
        }
    }

    void EndTurn()
    {
        // Deselect the current piece
        DeselectPiece();

        // Check if the game is over
        if (pieceCount[0] == 0 || pieceCount[1] == 0)
        {
            isGameOver = true;
            gameInfoTxt.text = "Game Over! Player " + currentPlayer + " Wins!";
            newGame.gameObject.SetActive(true);
            return;
        }

        // Switch to the next player
        currentPlayer = (currentPlayer == 1) ? 2 : 1;
        hasKilled = false;

        // Update the UI
        UpdateUI();
    }

    void CheckForCrown(Piece piece)
    {
        if ((piece.player == 1 && piece.row == boardSize - 1) || (piece.player == 2 && piece.row == 0))
        {
            piece.isCrowned = true;
        }
    }

    void CheckForWin()
    {
        if (pieceCount[currentPlayer - 1] == 0)
        {
            isGameOver = true;
            gameInfoTxt.text = "Player " + currentPlayer + " has lost all pieces. Game over!";
        }
        else if (GetValidMoves(selectedPiece).Count == 0)
        {
            isGameOver = true;
            gameInfoTxt.text = "Player " + currentPlayer + " has no valid moves. Game over!";
        }
    }

    public void RestartGame()
    {
        // Destroy all checkers and squares
        foreach (Transform child in playerOne.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in playerTwo.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in this.transform)
        {
            Destroy(child);
        }

        // Reset the game variables
        pieceCount[0] = 0;
        pieceCount[1] = 0;
        pieces = new Piece[boardSize, boardSize];
        currentPlayer = 1;
        hasKilled = false;
        isGameOver = false;

        // Regenerate the board
        GenerateBoard();

        // Update the UI
        UpdateUI();
    }

    public void DeselectPiece()
    {
        if (selectedPiece != null)
        {
            selectedPiece = null;
        }
        UnhighlightAllSquares();
    }

    public void HighlightSquare(int row, int col)
    {
        GameObject square = squares[row, col];
        Renderer squareRenderer = square.GetComponent<Renderer>();
        squareRenderer.material = highlightMaterial;
        validMoves.Add(square);
    }

    public void UnhighlightAllSquares()
    {
        foreach (GameObject square in validMoves)
        {
            Renderer squareRenderer = square.GetComponent<Renderer>();
            squareRenderer.material = (square.GetComponent<Square>().isDark) ? darkSquarePrefab.GetComponent<Renderer>().material : lightSquarePrefab.GetComponent<Renderer>().material;
        }
        validMoves.Clear();
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
