using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//攻撃された側が攻撃処理を行う
public class AttackedCard : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        /*攻撃*/
        //アタッカーカードを選択
        CardController attacker = eventData.pointerDrag.GetComponent <CardController>();
        //ディフェンダーカードを選択
        CardController defender = GetComponent<CardController>();
        if (attacker == null || defender == null)
        {
            return;
        }

        //敵フィールドにシールドカードがあれば、シールドカード以外は攻撃できない
        CardController[] enemyFieldCards = GameManager.I.gamePlayer(!attacker.model.isPlayerCard).GetFieldCards();
        if (Array.Exists(enemyFieldCards, card => card.model.ability == ABILITY.SHIELD)&&defender.model.ability!=ABILITY.SHIELD)
        {
            return;
        }

        if (attacker.model.isPlayerCard == defender.model.isPlayerCard)
        {
            return;
        }
        if(attacker.model.canAttack.Value)
        {
            //battle
            GameManager.I.CardsBattle(attacker, defender);
        }
        
    }
}
