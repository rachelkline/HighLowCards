using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardStack : MonoBehaviour
{
    List <int> cards;
    public bool isGameDeck;
    public bool HasCards
    {
        get { return cards != null && cards.Count > 0; }
    }

    //set up an event handler to deal with removed cards
    public event CardEventHandler CardRemoved;
    public event CardEventHandler CardAdded;

    public int CardCount
    {
        get
        {
            if(cards == null)
            {
                return 0;
            }
            else
            {
                return cards.Count;
            }
        }
    }
    
    //gives us public access to our cards list
    //we don't want to allow anyone to change this private list of cards
    public IEnumerable<int> GetCards()
    {
        foreach(int i in cards)
        {
            yield return i;
        }
    }

    public int Pop()
    {
        int temp = cards[0];
        cards.RemoveAt(0);

        if(CardRemoved != null)
        {
            CardRemoved(this, new CardEventArgs(temp));
        }
        return temp;
    }

    public void Push(int card)
    {
        cards.Add(card);

        if(CardAdded != null)
        {
            CardAdded(this, new CardEventArgs(card));
        }
    }

    public void RemoveCards()
    {
        cards.Clear();
    }


    public int CardValue()
    {
        int total = 0;
        foreach(int card in GetCards())
        {
            //     INDEX    VALUE
            //      0       2
            //      1       3
            //i have the index, so I need to calculate the value
            //use modulus to get remainder
            int cardRank = card % 13;
            //Add 2 because the card rank is exactly 2 below the card values
            total = cardRank += 2;
        }


        return total;
    }

    public void CreateDeck()
    {
        cards.Clear();
     
        for(int i = 0; i < 52; i++)
        {
            cards.Add(i);
        }

        //cursor goes from the back of the cards and the random number shrinks as you go through the cards
        int n = cards.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            //create a temporary value so these 'cards' can be swapped
            int temp = cards[k];
            cards[k] = cards[n];
            cards[n] = temp;
        }
    }
    
    void Awake()
    {
        cards = new List<int>();
        if(isGameDeck)
        {
         CreateDeck();   
        }
        
    }

}
