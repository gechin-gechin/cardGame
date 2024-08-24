using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SpellData
{
    static SpellData() { }
    public static bool CheckUseAble(CardModel model) {
        switch (model.spell)
        {
            //相手が必要な場合
            case SPELL.DAMAGE_ENEMY_CARD:
            case SPELL.DAMAGE_ENEMY_CARDS:
            case SPELL.DESTROY_ENEMY_CARD:
                //相手のフィールドカードを取得
                CardController[] cards = GameManager.I.GetFieldCards(!model.isPlayerCard);
                if (cards.Length > 0)
                {
                    return true;
                }
                return false;
            //味方が必要な場合
            case SPELL.HEAL_FRIEND_CARD:
            case SPELL.HEAL_FRIEND_CARDS:
                //味方のフィールドカードを取得
                CardController[] friendCards = GameManager.I.GetFieldCards(model.isPlayerCard);
                if (friendCards.Length > 0)
                {
                    return true;
                }
                return false;
            //特になし
            case SPELL.HEAL_FRIEND_HERO:
            case SPELL.DRAW_CARD:
            case SPELL.DRAW_2CARD:
            case SPELL.DAMAGE_ENEMY_HERO:
                return true;

            //スペルじゃない
            case SPELL.NONE:
                return false;
        }
        return false;
    }

    public static void Use(CardController target, CardController user)
    {
        switch (user.model.spell)
        {
            case SPELL.DAMAGE_ENEMY_CARD:
                //特定の敵を攻撃する
                if (target == null)//ターゲットがいない場合不発
                {
                    break;
                }
                if (target.model.isPlayerCard == user.model.isPlayerCard)//自分のカードには使えない
                {
                    break;
                }
                user.Attack(target);
                user.StartCoroutine(target.CheakAlive());
                break;
            case SPELL.DAMAGE_ENEMY_CARDS:
                //相手フィールド全てに攻撃する
                //相手のフィールドカードを取得
                CardController[] cards = GameManager.I.GetFieldCards(!user.model.isPlayerCard);
                //回ってる途中に削除されると怖いから二回呼ぶ
                foreach (CardController enemyCard in cards)
                {
                    user.Attack(enemyCard);
                }
                foreach (CardController enemyCard in cards)
                {
                    user.StartCoroutine(enemyCard.CheakAlive());
                }
                break;
            case SPELL.DAMAGE_ENEMY_HERO:
                // tekihero
                GameManager.I.AttackToHero(user);
                break;
            case SPELL.HEAL_FRIEND_CARD:
                if (target == null)
                {
                    return;
                }
                if (target.model.isPlayerCard != user.model.isPlayerCard)//相手のカードには使えない
                {
                    return;
                }
                user.Heal(target);
                break;
            case SPELL.HEAL_FRIEND_CARDS:
                CardController[] friendCards = GameManager.I.GetFieldCards(user.model.isPlayerCard);
                foreach (CardController friendCard in friendCards)
                {
                    user.Heal(friendCard);
                }
                break;
            case SPELL.HEAL_FRIEND_HERO:
                GameManager.I.HealToHero(user);
                break;
            case SPELL.DRAW_CARD:
                user.DrowCard();
                break;
            case SPELL.DRAW_2CARD:
                for (int i = 0; i < 2; i++)
                {
                    user.DrowCard();
                }
                break;
            case SPELL.DESTROY_ENEMY_CARD:
                //特定の敵をDESTROYする
                if (target == null)//ターゲットがいない場合不発
                {
                    return;
                }
                if (target.model.isPlayerCard == user.model.isPlayerCard)//自分のカードには使えない
                {
                    return;
                }
                user.DestroyCard(target);
                user.StartCoroutine(target.CheakAlive());
                break;
            case SPELL.NONE:
                return;
        }
    }
}
