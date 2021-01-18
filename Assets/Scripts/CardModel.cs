using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardModel : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    //contains all the data for the card
    public Sprite[] faces;
    public Sprite cardBack;


    //the card model will use the card index for the faces array
    //e.g. faces[cardIndex];
    public int cardIndex;

    public void ToggleFace(bool showFace)
    {
        if (showFace)
        {
            spriteRenderer.sprite = faces[cardIndex];
        }
        else
        {
            spriteRenderer.sprite = cardBack;
        }
    }

    //when the object awakes, grab the component attached to that object
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
}
