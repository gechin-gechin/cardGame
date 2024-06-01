using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardModel
{
    public string name;
    public int hp;
    public int at;
    public int cost;
    public Sprite icon;
    public ABILITY ability;
    public CIP cip;
    public PIG pig;
    public SP_ABILITY spAbility;
    public SPELL spell;
    public bool isAlive;
    public bool canAttack;
    public bool isFieldCard;
    public bool isPlayerCard;

    public CardModel(int cardID, bool isPlayer)
    {
        CardEntity cardEntity = Resources.Load<CardEntity>("CardDataList/Card" + cardID);
        name = cardEntity.name;
        hp = cardEntity.hp;
        at = cardEntity.at;
        cost = cardEntity.cost;
        icon = cardEntity.icon;
        cip = cardEntity.cip;
        pig = cardEntity.pig;
        spAbility = cardEntity.spAbility;
        ability = cardEntity.ability;
        spell = cardEntity.spell;
        isAlive = true;
        isPlayerCard = isPlayer;
    }

     void Damage(int dmg)
    {
        hp -= dmg;
        if (hp <= 0)
        {
            hp = 0;
            isAlive = false;
        }
    }

    void recoveryHP(int point)
    {
        hp += point;
    }

    public void Attack(CardController card)
    {
        card.model.Damage(at);
    }
    public void Heal(CardController card)
    {
        card.model.recoveryHP(at);
    }
    public void DestroyCard(CardController card)
    {
        card.model.isAlive = false;
    }
}
