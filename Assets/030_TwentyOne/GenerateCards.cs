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
    public Transform playerHand, dealerHand;
    public Button startBtn, hitBtn, standBtn;
    public TextMeshProUGUI playerScoreText, dealerScoreText, winLoseText;

    public GameObject deck;
    private List<Card> allCards = new List<Card>();
    private List<Card> playerCards = new List<Card>();
    private List<Card> dealerCards = new List<Card>();
    private Vector3 deckCardPos = new Vector3(-0.7f, 0.9f, -0.15f);
    private Vector3 playerCardPos = new Vector3(-0.2f, 0.93f, -0.35f);
    private Vector3 dealerCardPos = new Vector3(-0.2f, 0.93f, -0.05f);
    private Vector3 faceDownRot = new Vector3(90, 0, 0);
    private Vector3 faceUpRot = new Vector3(-90, 0, 0);
    private Vector3 scaleCard = new Vector3(1.5f, 1.5f, 1.5f);
    private double cardHeight = 0.0015;
    private GameObject dealerFirstCard;
    private bool playerTurn;

    //

    // Start is called before the first frame update
    void Start()
    {
        winLoseText.gameObject.SetActive(false);
        startBtn.gameObject.SetActive(true);
        hitBtn.gameObject.SetActive(false);
        standBtn.gameObject.SetActive(false);
        startBtn.onClick.AddListener(StartGame);
        hitBtn.onClick.AddListener(Hit);
        standBtn.onClick.AddListener(Stand);

        AddCardsToList();
        ShuffleCardsInList();
        GenerateCardsInList();
    }


    // Add all cards to list
    void AddCardsToList()
    {
        // Loop through each suit
        for (int s = 0; s < 4; s++)
        {
            Card.Suit suit = (Card.Suit)s;

            // Loop through each rank
            for (int r = 0; r < 13; r++)
            {
                int value = r + 1;
                string cardName = "";

                // Add Card info
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

                Card newCard = new Card();
                newCard.cardName = cardName;
                newCard.value = value;
                newCard.suit = suit;

                switch (suit)
                {
                    case Card.Suit.Clubs:
                        newCard.card = clubCards[r];
                        break;
                    case Card.Suit.Diamonds:
                        newCard.card = diamondCards[r];
                        break;
                    case Card.Suit.Hearts:
                        newCard.card = heartCards[r];
                        break;
                    case Card.Suit.Spades:
                        newCard.card = spadeCards[r];
                        break;
                }

                allCards.Add(newCard);
                
                Debug.Log(newCard.cardName + " of " + newCard.suit);
            }
        }
    }

    // Shuffle all cards in list
    void ShuffleCardsInList()
    {
        for (int i = 0; i < allCards.Count; i++)
        {
            Card temp = allCards[i];
            int randomIndex = Random.Range(i, allCards.Count);
            allCards[i] = allCards[randomIndex];
            allCards[randomIndex] = temp;
        }
        
        // Debug
        for (int i = 0; i < allCards.Count; i++)
        {
            Debug.Log(allCards[i].cardName + " of " + allCards[i].suit);
        }
    }

    // Generate all card game objects in list
    void GenerateCardsInList()
    {
        for (int i = 0; i < allCards.Count; i++)
        {
            Vector3 newDeckCardPos = new Vector3(deckCardPos.x, (float)(deckCardPos.y + i * cardHeight), deckCardPos.z);
            GameObject newCardObj = Instantiate(allCards[i].card, newDeckCardPos, Quaternion.Euler(faceDownRot));
            newCardObj.transform.localScale = scaleCard;
            
        }
    }

    void StartGame() 
    {
        startBtn.gameObject.SetActive(false);
        DealCards();

    }
    Card DealCard()
    {
        Card newCard = allCards[0];
        allCards.RemoveAt(0);
        return newCard;
    }

    // Deal cards to player and dealer
    void DealCards()
    {
        for (int i = 0; i < 2; i++)
        {
            // Deal card to player
            Card playerCard = DealCard();
            playerCards.Add(playerCard);

            GameObject newplayerCardObj = Instantiate(playerCard.card, playerHand);
            newplayerCardObj.transform.position = new Vector3(playerCardPos.x + i * 0.1f, playerCardPos.y, playerCardPos.z);
            newplayerCardObj.transform.localScale = scaleCard;

            // Deal card to dealer
            Card dealerCard = DealCard();
            dealerCards.Add(dealerCard);
            GameObject newdealerCardObj = Instantiate(dealerCard.card, dealerHand);
            newdealerCardObj.transform.position = new Vector3(dealerCardPos.x + i * 0.1f, dealerCardPos.y, dealerCardPos.z);
            newdealerCardObj.transform.localScale = scaleCard;

            if (i == 0)
            {
                newdealerCardObj.transform.rotation = Quaternion.Euler(faceDownRot);
            }

            CheckForBust();
            CheckForBlackjack();
        }

        PlayerTurn();
    }



    // Player hits to receive another card
    void Hit() 
    {
        // Deal card to player
        Card playerCard = DealCard();
        playerCards.Add(playerCard);

        GameObject newplayerCardObj = Instantiate(playerCard.card, playerHand);
        newplayerCardObj.transform.position = new Vector3(playerCardPos.x + (playerCards.Count - 1) * 0.1f, playerCardPos.y, playerCardPos.z);
        newplayerCardObj.transform.localScale = scaleCard;

        CheckForBust();
        CheckForBlackjack();
    }

    void Stand()
    {
        DealerTurn();
    }

    void PlayerTurn()
    {
        playerTurn = true;
        hitBtn.gameObject.SetActive(true);
        standBtn.gameObject.SetActive(true);
    }

    void DealerTurn() 
    {
        playerTurn = false;
        hitBtn.gameObject.SetActive(false);
        standBtn.gameObject.SetActive(false);

        // Dealer draws cards until score is >= 17
        int score = CalculateScore(dealerCards);
        while (score < 17)
        {
            // Add a card to dealer's hand
            Card newCard = DealCard();
            dealerCards.Add(newCard);

            // Generate new card object and add to dealer's hand
            GameObject newdealerCardObj = Instantiate(newCard.card, dealerHand);
            newdealerCardObj.transform.localPosition = new Vector3(dealerCardPos.x + (dealerCards.Count - 1) * 0.1f, dealerCardPos.y, dealerCardPos.z);
            newdealerCardObj.transform.localScale = scaleCard;

            CheckForBust();
            CheckForBlackjack();
            score = CalculateScore(dealerCards);
        }

        CheckForWinner();
        startBtn.gameObject.SetActive(true);
    }

    // Calculate the score of a hand of cards
    int CalculateScore(List<Card> hand)
    {
        int score = 0;
        int aceCount = 0;

        foreach (Card card in hand)
        {
            score += card.value;

            if (card.cardName == "Ace")
            {
                aceCount++;
            }
        }

        // Change Ace value to 1 if score is over 21
        while (score > 21 && aceCount > 0)
        {
            score -= 10;
            aceCount--;
        }

        return score;
    }

    // Check if player or dealer has blackjack
    void CheckForBlackjack()
    {
        int playerScore = CalculateScore(playerCards);
        int dealerScore = CalculateScore(dealerCards);

        if (playerScore == 21 && dealerScore != 21)
        {
            winLoseText.gameObject.SetActive(true);
            winLoseText.text = "Player has blackjack!";
            Debug.Log("Player has blackjack!");
            EndRound();
            }
        else if (playerScore != 21 && dealerScore == 21)
        {
            winLoseText.gameObject.SetActive(true);
            winLoseText.text = "Dealer has blackjack!";
            Debug.Log("Dealer has blackjack!");
            EndRound();
        }
        else if (playerScore == 21 && dealerScore == 21)
        {
            winLoseText.gameObject.SetActive(true);
            winLoseText.text = "Both have blackjack!";
            Debug.Log("Both have blackjack!");
            EndRound();
        }
    }

    // Check if player or dealer busts
    void CheckForBust() 
    {
        int playerScore = CalculateScore(playerCards);
        int dealerScore = CalculateScore(dealerCards);
        Debug.Log("playerScore: " + playerScore + ", dealerScore: " + dealerScore);

        if (playerScore > 21)
        {
            winLoseText.gameObject.SetActive(true);
            winLoseText.text = "Player busts with a score of " + playerScore;
            Debug.Log("Player busts with a score of " + playerScore);
            EndRound();
        }
        else if (dealerScore > 21) 
        {
            winLoseText.gameObject.SetActive(true);
            winLoseText.text = "Dealer busts with a score of " + dealerScore;
            Debug.Log("Dealer busts with a score of " + dealerScore);
            EndRound();
        }
    }

    // Check who Won
    void CheckForWinner()
    {
        int playerScore = CalculateScore(playerCards);
        int dealerScore = CalculateScore(dealerCards);

        if (playerScore < 21 && dealerScore < 21)
        {
            if (playerScore > dealerScore)
            {
                winLoseText.gameObject.SetActive(true);
                winLoseText.text = "Player wins with a score of " + playerScore;
                Debug.Log("Player wins with a score of " + playerScore);
            }
            else if (playerScore < dealerScore)
            {
                winLoseText.gameObject.SetActive(true);
                winLoseText.text = "Dealer wins with a score of " + dealerScore;
                Debug.Log("Dealer wins with a score of " + dealerScore);
            }
            else
            {
                winLoseText.gameObject.SetActive(true);
                winLoseText.text = "It's a tie with a score of " + playerScore;
                Debug.Log("It's a tie with a score of " + playerScore);
            }
            EndRound();
        }
    }

    void EndRound()
    {
        GameObject newdealerCardObj = Instantiate(dealerCards[0].card, dealerHand);
        newdealerCardObj.transform.position = dealerCardPos;
        newdealerCardObj.transform.localScale = scaleCard;
    }
}
