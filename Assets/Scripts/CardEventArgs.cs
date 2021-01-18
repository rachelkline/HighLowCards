using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;
using UnityEngine;

public class CardEventArgs : EventArgs
{
    public int CardIndex { get; private set; }

    public CardEventArgs(int cardIndex)
    {
        CardIndex = cardIndex;
    }
}