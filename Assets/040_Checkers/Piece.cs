using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    // Highlight material for valid moves
    public Material highlightMaterial;

    public int player; //player number
    public int row; // piece location
    public int col; // piece location
    private Checkers checkerBoard;


    void Start()
    {
        checkerBoard = FindObjectOfType<Checkers>(); // get the game board
    }

    // Set the piece's location on the board
    public void SetPosition(int row, int column)
    {
        this.row = row;
        this.col = column;
    }

    // Highlight the piece to show that it is selected
    public void Highlight()
    {
        GetComponent<Renderer>().material = highlightMaterial;
    }

    // Remove the highlight from the piece
    public void Unhighlight()
    {
        GetComponent<Renderer>().material = ;
    }


}
