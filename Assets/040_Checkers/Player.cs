using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Checkers game;
    private int player;
    private Piece selectedPiece;
    private Vector3 mousePos;
    private float yOffset = 0.3f;

    // Start is called before the first frame update
    void Start()
    {
        game = FindObjectOfType<Checkers>();
        player = GetComponentInChildren<Piece>().player;
    }

    

    


    
}
