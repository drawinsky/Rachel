using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GenerateCards : MonoBehaviour
{
    public GameObject[] clubCards = new GameObject[13];
    public GameObject[] diamondCards = new GameObject[13];
    public GameObject[] heartCards = new GameObject[13];
    public GameObject[] spadeCards = new GameObject[13];
    public List<Card> AllCards = new List<Card>();
    

    public GameObject deck;



    public Card cardPrefab; // use for creating new Cards

    //

    // Start is called before the first frame update
    void Start()
    {
        // Add all cards to list
        // Loop through each suit
        for (int s = 0; s < 4; s++)
        {
            Card.Suit suit = (Card.Suit)s;

            // Loop through each rank
            for (int r = 0; r < 13; r++)
            {
                int value = r + 1;
                string cardName = "";
                switch (value)
                {
                    case 1:
                        cardName = "Ace";
                        value = 11;
                        break;
                    case 11:
                        cardName = "Jack";
                        value = 10;
                        break;
                    case 12:
                        cardName = "Queen";
                        value = 10;
                        break;
                    case 13:
                        cardName = "King";
                        value = 10;
                        break;
                    default:
                        cardName = value.ToString();
                        break;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
