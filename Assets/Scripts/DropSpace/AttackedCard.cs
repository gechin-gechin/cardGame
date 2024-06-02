using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//攻撃された側が攻撃処理を行う
public class AttackedCard : BaseDropSpace
{
    protected override void process(CardController attacker)
    {
        //ディフェンダーカードを選択
        CardController defender = GetComponent<CardController>();
        if (defender == null)
        {
            return;
        }
        //敵フィールドにシールドカードがあれば、シールドカード以外は攻撃できない
        if (Array.Exists(GetEnemyCards(), card => card.model.ability == ABILITY.SHIELD) && defender.model.ability != ABILITY.SHIELD)
        {
            return;
        }

        if (attacker.model.isPlayerCard == defender.model.isPlayerCard)
        {
            return;
        }
        if (attacker.model.canAttack.Value)
        {
            //battle
            GameManager.I.CardsBattle(attacker, defender);
        }
    }
}
