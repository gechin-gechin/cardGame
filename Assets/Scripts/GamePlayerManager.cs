using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayerManager : MonoBehaviour
{
    //Deck
    public List<int> deck = new List<int>();
    //HP
    public int heroHp;
    //COST
    public int manaCost;
    public int defaultManaCost;

    public void Init(List<int>cardDeck,int mana)
    {
        deck = cardDeck;
        heroHp = 10;
        manaCost = defaultManaCost = mana;
    }
    public void IncreaseManaCost()
    {
        if (defaultManaCost < 10)
        {
            defaultManaCost++;
        }
        manaCost = defaultManaCost;
    }
}
