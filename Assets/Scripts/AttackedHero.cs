using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//攻撃された側が攻撃処理を行う
public class AttackedHero : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        /*攻撃*/
        //アタッカーカードを選択
        CardController attacker = eventData.pointerDrag.GetComponent<CardController>();
        if (attacker == null )
        {
            return;
        }

        //敵フィールドにシールドカードがあると攻撃できない
        CardController[] enemyFieldCards = GameManager.instance.GetEnemyFieldCards(attacker.model.isPlayerCard);
        if (Array.Exists(enemyFieldCards, card => card.model.ability == ABILITY.SHIELD))
        {
            return;
        }

        if (attacker.model.canAttack)
        {
            //attacked to hero
            GameManager.instance.AttackToHero(attacker);
            GameManager.instance.CheckHeroHP();
        }

    }
}

