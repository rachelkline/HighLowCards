using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardFlipper : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    CardModel model;

    public AnimationCurve scaleCurve;
    public float duration = 0.5f;
    
    //just getting the components we will use later on
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        model = GetComponent<CardModel>();
    }

    public void FlipCard(Sprite startImage, Sprite endImage, int cardIndex)
    {
        StopCoroutine(Flip(startImage, endImage, cardIndex));
        StartCoroutine(Flip(startImage, endImage, cardIndex));
    }

    //the actual method that does the flipping of the card
    IEnumerator Flip(Sprite startImage, Sprite endImage, int cardIndex)
    {
        spriteRenderer.sprite = startImage;

        float time = 0f;
        while(time <= 1f)
        {
            float scale = scaleCurve.Evaluate(time);
            time = time + Time.deltaTime / duration;

            //whatever we have on xyz, we only alter the x component to 'squish' the image to look like it's being flipped
            Vector3 localScale = transform.localScale;
            localScale.x = scale;
            transform.localScale = localScale;

            if(time >= 0.5f)
            {
                spriteRenderer.sprite = endImage;
            }
        
            yield return new WaitForFixedUpdate();
        }

        if(cardIndex == -1)
        {
            model.ToggleFace(false);
        }
        else
        {
            model.cardIndex = cardIndex;
            model.ToggleFace(true);
        }
    }

}
