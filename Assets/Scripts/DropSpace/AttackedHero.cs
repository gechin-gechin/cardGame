using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//攻撃された側が攻撃処理を行う
public class AttackedHero : BaseDropSpace
{
    protected override void process(CardController card)
    {
        //敵フィールドにシールドカードがあると攻撃できない
        if (Array.Exists(GetEnemyCards(), card => card.model.ability == ABILITY.SHIELD))
        {
            return;
        }

        if (card.model.canAttack.Value)
        {
            //attacked to hero
            GameManager.I.AttackToHero(card);
        }
    }
}

