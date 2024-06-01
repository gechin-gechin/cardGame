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

    public CardModel(CardEntity entity, bool isPlayer)
    {
        CardEntity cardEntity = entity;
        name = cardEntity.Name;
        hp = cardEntity.HP;
        at = cardEntity.At;
        cost = cardEntity.Cost;
        icon = cardEntity.Icon;
        cip = cardEntity.Cip;
        pig = cardEntity.Pig;
        spAbility = cardEntity.SpAbility;
        ability = cardEntity.Ability;
        spell = cardEntity.Spell;
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
