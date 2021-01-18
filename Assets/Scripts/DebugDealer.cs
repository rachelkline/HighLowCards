using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugDealer : MonoBehaviour
{
    public CardStack dealer;
    public CardStack player;

    //take a card from the dealer and give it to the player
    void OnGUI()
    {
        if(GUI.Button(new Rect(10, 10, 256, 56), "Hit Me!"))
        {
            player.Push(dealer.Pop());
        }
    }

}
