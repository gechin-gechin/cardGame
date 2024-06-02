using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PIGData
{
    static PIGData() { }
    public static void Act(CardController card)
    {
        switch (card.model.pig)
        {
            case PIG.DRAW1:
                card.DrawCard();
                return;
            case PIG.NONE:
                return;
        }
    }
}
