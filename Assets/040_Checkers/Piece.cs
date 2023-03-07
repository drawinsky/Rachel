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
    public bool isSelected = false;
    public bool isCrowned = false;
}
