using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    // Highlight material for valid moves
    public Material highlightMaterial;

    public int player = -1; //player number
    public int row = -1; // piece location
    public int col = -1; // piece location
    public bool isSelected = false;
    public bool isCrowned = false;

    public void SetPosition(int newX, int newY)
    {
        row = newX;
        col = newY;

        transform.position = new Vector3(row, 0.2f, col);
    }
}
