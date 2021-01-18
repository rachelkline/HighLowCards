using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CardStack))]

public class CardStackView : MonoBehaviour
{
    CardStack deck;
    Dictionary<int, CardView> fetchedCards;
    int lastCount;

    public Vector3 start;
    public float cardOffset;
    public bool faceUp = false; 
    public bool reverseLayerOrder = false;
    public GameObject cardPrefab;
    //to display the cards fanned out vs on top of each other

    public void Toggle(int card, bool isFaceUp)
    {
        fetchedCards[card].IsFaceUp = isFaceUp;
    }

    public void ClearHand()
    {
        deck.RemoveCards();
        foreach(CardView view in fetchedCards.Values)
        {
            Destroy(view.Card);

        }
        fetchedCards.Clear();
    }

    void Start()
    {
        fetchedCards = new Dictionary<int, CardView>();
        deck = GetComponent<CardStack>();
        ShowCards();
        lastCount = deck.CardCount;

        deck.CardRemoved += deck_CardRemoved;
        deck.CardAdded += deck_CardAdded;
    }

    void deck_CardAdded(object sender, CardEventArgs e)
    {
        float co = cardOffset * deck.CardCount;
        Vector3 temp = start + new Vector3(co, 0f);
        AddCard(temp, e.CardIndex, deck.CardCount);
    }

    void deck_CardRemoved(object sender, CardEventArgs e)
    {
        //if it does contain the key, destroy the game object & remove that index
        //so it is not referenced again
        if(fetchedCards.ContainsKey(e.CardIndex))
        {
            Destroy(fetchedCards[e.CardIndex].Card);
            fetchedCards.Remove(e.CardIndex);
        }
    }

    //if the current count is different than previous, add all cards in show cards method
    void Update()
    {
        if (lastCount != deck.CardCount)
        {
            lastCount = deck.CardCount;
            ShowCards();
        }
    }
    
    //iterate through all the integers
    //create an instance for card prefab
    //keep adding to the vector position using card offset
    public void ShowCards()
    {
        int cardCount = 0;

        if(deck.HasCards)
        {

            foreach(int i in deck.GetCards())
            {
                float co = cardOffset * cardCount;
                Vector3 temp = start + new Vector3(co, 0f);
                AddCard(temp, i, cardCount);
                cardCount++;
           } 
        }  
    }

    void AddCard(Vector3 position, int cardIndex, int positionalIndex)
    {
        //if the stack is by default face up, check to see if card is face up
        
        if(fetchedCards.ContainsKey(cardIndex))
        {
            if(!faceUp)
            {
                CardModel model = fetchedCards[cardIndex].Card.GetComponent<CardModel>();
                model.ToggleFace(fetchedCards[cardIndex].IsFaceUp);
            }
            return;
        }

        GameObject cardCopy = (GameObject)Instantiate(cardPrefab);
        cardCopy.transform.position = position;

        CardModel cardModel = cardCopy.GetComponent<CardModel>();
        cardModel.cardIndex = cardIndex;
        cardModel.ToggleFace(faceUp);

        //ensure cards are rendering on top of each other
        SpriteRenderer spriteRenderer = cardCopy.GetComponent<SpriteRenderer>();
        if(reverseLayerOrder)
        {
            spriteRenderer.sortingOrder = 51 - positionalIndex;
        }
        else
        {
           spriteRenderer.sortingOrder = positionalIndex; 
        }
        

        //both the integer and the card object
        fetchedCards.Add(cardIndex, new CardView(cardCopy));

    }

}
