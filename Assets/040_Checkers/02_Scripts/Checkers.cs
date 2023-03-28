using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class Checkers : MonoBehaviour
{
    public GameObject checkerBoard, lightSquarePrefab, darkSquarePrefab; // Checker board elements
    public Piece checkerPrefabOne, checkerPrefabTwo;
    public GameObject playerOne, playerTwo;
    public TextMeshProUGUI currentTurnTxt, p1PieceCountTxt, p2PieceCountTxt, gameInfoTxt;
    public Button newGame;
    public int boardSize = 8; // Checker board size
    private int currentPlayer = 1;
    private int[] pieceCount = new int[2];
    private bool hasKilled = false;
    private bool isGameOver = false;
    private Piece[,] pieces;

    public Material highlightMaterial, darkMaterial, lightMeterial;

    private int squareSize = 1;
    private GameObject[,] squares; // 2D array that stores references to each square on the checker board

    private GameObject crown;

    public List<GameObject> validMoves;
    
    private void Start()
    {
        gameInfoTxt.text = "";
        newGame.onClick.AddListener(RestartGame);

        GenerateBoard();
        InitGame();
        UpdateUI();
        newGame.gameObject.SetActive(false);
    }

    private Piece selectedPiece = null;
    private Piece getPiece = null;
    private RaycastHit hitPiece;

    // Update is called once per frame
    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            bool didHitSomething = Physics.Raycast(ray, out RaycastHit hit);
            hitPiece = hit;
            Debug.Log("HitScrene");
            

            if (didHitSomething == true)
            {
                Debug.Log("HitSth");

                // If selected piece, place it
                if (selectedPiece != null)
                {
                    int xHit = Mathf.RoundToInt(hit.point.x);
                    int zHit = Mathf.RoundToInt(hit.point.z);
                    
                    Debug.Log("selectedPiece != null");
                    MovePiece(selectedPiece, xHit, zHit);
                    //selectedPiece.SetPosition(xHit, zHit);
                    selectedPiece = null;
                }
                // If not, select a piece
                else
                {
                    getPiece = hit.collider.gameObject.GetComponent<Piece>();
                    SelectPiece(getPiece);
                    

                }

                Debug.DrawRay(hit.point, Vector3.up, Color.cyan, 2);
            }

            Debug.DrawRay(ray.origin, ray.direction * 10, Color.yellow, 2);

        }

    }

    void GenerateBoard()
    {
        squares = new GameObject[boardSize, boardSize];
        pieces = new Piece[boardSize, boardSize];

        // Instatiate the light and dark square prefabs as child objects of the board
        for (int row = 0; row < boardSize; row++)
        {
            for (int col = 0; col < boardSize; col++)
            {
                // Calculate the position of the square

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
                Vector3 squarePos = new Vector3(row, 0, col);
                GameObject square = Instantiate(squarePrefab, squarePos, Quaternion.identity);
                square.transform.SetParent(checkerBoard.transform);


                // Store the square reference in a 2D array
                squares[row, col] = square;

                // Instatiate players' checkers on the dark squares
                if (row < 3 && (row + col) % 2 != 0)
                {
                    // player 1's checker
                    Piece checker = Instantiate(checkerPrefabOne);
                    checker.transform.SetParent(playerOne.transform);
                    checker.SetPosition(row, col);
                    checker.player = 1;
                    pieces[row, col] = checker;
                    

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
                    // player 2's checker
                    Piece checker = Instantiate(checkerPrefabTwo);
                    checker.transform.SetParent(playerTwo.transform);
                    checker.SetPosition(row, col);
                    checker.player = 2;
                    pieces[row, col] = checker;

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
            Debug.Log("getPiece.player = " + getPiece.player);
            Debug.Log("currentPlayer = " + currentPlayer);
            return;
        }
        if (selectedPiece != null)
        {
            Debug.Log($"selected piece at {selectedPiece.row}, {selectedPiece.col}");
            selectedPiece.isSelected = false;
        }

        // Select the new piece
        selectedPiece = piece;
        selectedPiece.isSelected = true;
        Debug.Log(selectedPiece);
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
        //foreach (GameObject move in validMoves)
        //{
        //    move.SetActive(true);
        //}

        
    }

    void CheckDirection(int row, int col)
    {
        //int targetRow = row + (row - selectedPiece.row);
        //int targetCol = col + (col - selectedPiece.col);
        int targetRow = row ;
        int targetCol = col ;
        Debug.Log($"target square at {targetRow}, {targetCol}");
        if (targetRow < 0 || targetRow >= boardSize || targetCol < 0 || targetCol >= boardSize)
        {
            return;
        }

        Piece targetPiece = pieces[targetRow, targetCol];

        // If the target square is unoccupied, add it to the valid moves list
        if (targetPiece == null)
        {
            Debug.Log("a valid move");
            validMoves.Add(squares[targetRow, targetCol]);

            HighlightSquare(squares[targetRow, targetCol]);
            return;
        }

        // If the target square is occupied by an opponent's piece, check if we can capture it
        if (targetPiece.player != currentPlayer)
        {
            int captureRow = row + (row - selectedPiece.row);
            int captureCol = col + (col - selectedPiece.col);
            //int captureRow = targetRow + (targetRow - row);
            //int captureCol = targetCol + (targetCol - col);
            Debug.Log($"target square at {targetRow}, {targetCol}");
            Debug.Log($"selected piece at {selectedPiece.row}, {selectedPiece.col}");
            Debug.Log($"capture square at {captureRow}, {captureCol}");

            // Check if the capture move is within the bounds of the board
            if (captureRow < 0 || captureRow >= boardSize || captureCol < 0 || captureCol >= boardSize)
            {
                return;
            }
            
            // Check if the square beyond the opponent's piece is unoccupied
            Piece capturePiece = pieces[captureRow, captureCol];
            if (capturePiece == null)
            {
                Debug.Log("a valid capture move");
                // This is a valid capture move, so add the target square to the valid moves list
                validMoves.Add(squares[captureRow, captureCol]);
                HighlightSquare(squares[captureRow, captureCol]);
                return;
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


    public void MovePiece(Piece piece, int hitRow, int hitCol)
    {
        if (selectedPiece != null)
        {
            int preSelectRow = selectedPiece.row;
            int preSelectCol = selectedPiece.col;
            //if (IsValidMove(hitRow, hitCol))
            //{
            // Move the selected piece to the new square
            //piece.SetPosition(hitRow, hitCol);
            //pieces[piece.row, piece.col] = null;
            //pieces[hitRow, hitCol] = piece;

            //CheckForCrown(piece);
            //}
            for (int i = 0; i < validMoves.Count; i++)
            {
                if (validMoves[i] == squares[hitRow, hitCol])
                {
                    // Move the selected piece to the new square
                    pieces[piece.row, piece.col] = null;
                    pieces[hitRow, hitCol] = piece;
                    piece.SetPosition(hitRow, hitCol);
                    CheckForCrown(piece);
                }
            }
            

            //Check if the piece has captured an enemy piece
            Debug.Log(Mathf.Abs(hitRow - preSelectRow));
            if (Mathf.Abs(hitRow - preSelectRow) == 2)
            {
                Debug.Log("captured an enemy piece");
                hasKilled = true;
                int killRow = (preSelectRow + hitRow) / 2;
                int killCol = (preSelectCol + hitCol) / 2;
                Piece killedPiece = pieces[killRow, killCol];
                pieceCount[killedPiece.player - 1]--;
                Destroy(killedPiece.gameObject);

                // Check if there are any more valid moves for this piece
                if (GetValidMoves(selectedPiece).Count > 0 && piece.isCrowned == false)
                {
                    SelectPiece(getPiece);
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
                SelectPiece(getPiece);
            }
        }
    }

    void EndTurn()
    {
        if (selectedPiece.isCrowned == true)
        {
            if (selectedPiece.player == 1 && selectedPiece.row == 0)
            {
                isGameOver = true;
                gameInfoTxt.text = "Game Over! Player " + currentPlayer + " Wins!";
                newGame.gameObject.SetActive(true);
                return;
            }
            else if (selectedPiece.player == 2 && selectedPiece.row == boardSize - 1)
            {
                isGameOver = true;
                gameInfoTxt.text = "Game Over! Player " + currentPlayer + " Wins!";
                newGame.gameObject.SetActive(true);
                return;
            }
            // Deselect the current piece
            DeselectPiece();
        }

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
            // Find and Show the crown object as a child of this checker
            Transform crownTransform = piece.transform.Find("Crown");
            if (crownTransform != null)
            {
                crown = crownTransform.gameObject;
                crown.SetActive(true);
            }
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
        foreach (Transform child in checkerBoard.transform)
        {
            Destroy(child.gameObject);
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
        InitGame();
        // Update the UI
        UpdateUI();
        gameInfoTxt.text = "";

        newGame.gameObject.SetActive(false);
    }

    public void DeselectPiece()
    {
        if (selectedPiece != null)
        {
            selectedPiece = null;
        }
        UnhighlightAllSquares();
    }

    public void HighlightSquare(GameObject square)
    {
        //GameObject square = squares[row, col];
        //Renderer squareRenderer = square.GetComponent<Renderer>();
        //squareRenderer.material = highlightMaterial;
        //validMoves.Add(square);

        // Get the child object by name
        Transform childObject = square.transform.Find("Cube");

        // Set the material of the child object
        Renderer childRenderer = childObject.GetComponent<Renderer>();
        childRenderer.material = highlightMaterial;
    }

    public void UnhighlightAllSquares()
    {
        foreach (GameObject square in validMoves)
        {
            
            // Get the child object by name
            Transform childObject = square.transform.Find("Cube");

            // Set the material of the child object
            Renderer childRenderer = childObject.GetComponent<Renderer>();
            childRenderer.material = darkMaterial;

            //if (square == darkSquarePrefab)
            //{
                //childRenderer.material = darkMaterial;
            //}
            //else if (square == lightSquarePrefab)
            //{
                //childRenderer.material = lightMeterial;
            //}
            //Renderer squareRenderer = square.GetComponent<Renderer>();
            //squareRenderer.material = (square.GetComponent<Square>().isDark) ? darkSquarePrefab.GetComponent<Renderer>().material : lightSquarePrefab.GetComponent<Renderer>().material;
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
