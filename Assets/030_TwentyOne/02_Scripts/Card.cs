using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Card : MonoBehaviour
{
    public GameObject card; // 3D model
    public string cardName; // Ace, Jack, Queen, King, 2, 3...
    public int value; // score point
    public Suit suit; // Clubs, Diamonds, Hearts, Spades

    public TextMeshPro cardText;

    public enum Suit { Clubs, Diamonds, Hearts, Spades };
}