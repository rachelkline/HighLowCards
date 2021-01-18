using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugChangeCard : MonoBehaviour
{
    CardFlipper flipper;
    CardModel cardModel;
    int cardIndex = 0;
    
    public GameObject card;

    void Awake()
    {
        cardModel = card.GetComponent<CardModel>();
        flipper = card.GetComponent<CardFlipper>();
    }

    void OnGUI()
    {
        if(GUI.Button(new Rect(10,10, 200, 56), "Hit me!"))
        {
            if(cardIndex >= cardModel.faces.Length)
            {
                cardIndex = 0;
                flipper.FlipCard(cardModel.faces[cardModel.faces.Length - 1], cardModel.cardBack, -1);
            }
            else
            {
                if(cardIndex > 0)
                {
                    //if the model is greater than 0, take the previous card and pass in the new card index and then add 1
                    flipper.FlipCard(cardModel.faces[cardIndex - 1], cardModel.faces[cardIndex], cardIndex);
                }
                else
                //if it is less than 0 start w/ the card back
                {
                    flipper.FlipCard(cardModel.cardBack, cardModel.faces[cardIndex], cardIndex);
                }

                cardIndex++; 
            }
        

        }
    }
}
