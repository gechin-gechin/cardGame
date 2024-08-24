using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CIPData
{
    static CIPData() { }
    public static void Act(CardController card)
    {
        switch (card.model.cip)
        {
            case CIP.ALL_DELETE:
                //破壊カードを取得
                //相手フィールド全て
                CardController[] enemycards = GameManager.I.GetFieldCards(!card.model.isPlayerCard);
                CardController[] destroyCard = Array.FindAll(
                    GameManager.I.GetFieldCards(!card.model.isPlayerCard),
                    _card => _card != card);
                //破壊
                foreach (CardController Cards in destroyCard)
                {
                    card.DestroyCard(Cards);
                }
                foreach (CardController Cards in enemycards)
                {
                    card.DestroyCard(Cards);
                }
                foreach (CardController Cards in destroyCard)
                {
                    card.StartCoroutine(Cards.CheakAlive());
                }
                foreach (CardController Cards in enemycards)
                {
                    card.StartCoroutine(Cards.CheakAlive());
                }
                return;
            case CIP.NONE:
                return;
        }
    }
}
