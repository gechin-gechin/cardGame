using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using R3;

public class CardModel
{
    public string name;
    private ReactiveProperty<int> hp = new ReactiveProperty<int>();
    public ReadOnlyReactiveProperty<int> HP => hp;
    private ReactiveProperty<int> at = new ReactiveProperty<int>();
    public ReadOnlyReactiveProperty<int> At => at;
    public int cost;
    public Sprite icon;
    public ABILITY ability;
    public CIP cip;
    public PIG pig;
    public SP_ABILITY spAbility;
    public SPELL spell;
    public bool isAlive;
    public ReactiveProperty<bool> canAttack = new ReactiveProperty<bool>();
    public bool isFieldCard;
    public bool isPlayerCard;

    public CardModel(CardEntity entity, bool isPlayer)
    {
        CardEntity cardEntity = entity;
        name = cardEntity.Name;
        hp.Value = cardEntity.HP;
        at.Value = cardEntity.At;
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

    private void Damage(int dmg)
    {
        hp.Value -= dmg;
        if (hp.Value <= 0)
        {
            hp.Value = 0;
            isAlive = false;
        }
    }
    private void recoveryHP(int point)
    {
        hp.Value += point;
    }

    public void Attack(CardController card)
    {
        card.model.Damage(at.Value);
    }
    public void Heal(CardController card)
    {
        card.model.recoveryHP(at.Value);
    }
    public void DestroyCard(CardController card)
    {
        card.model.isAlive = false;
    }
}
