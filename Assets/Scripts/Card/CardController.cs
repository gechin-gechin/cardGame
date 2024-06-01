using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardController : MonoBehaviour
{
    CardView view;          //　見かけ(view)に関することを操作
    public CardModel model;        //　データ(model)に関することを操作
    public CardMovement movement;  //　移動(movement)に関することを操作

    GameManager gameManager;
    // spell dato true
    public bool IsSpell
    {
        get { return model.spell != SPELL.NONE; }
    }

    private void Awake()
    {
        view = GetComponent<CardView>();
        movement = GetComponent<CardMovement>();
        gameManager = GameManager.I;
    }
    public void Init(CardEntity entity, bool isPlayer)
    {
        model = new CardModel(entity, isPlayer);
        view.SetCard(model);
    }

    public void Attack(CardController enemyCard)
    {
        model.Attack(enemyCard);
        SetCanAttack(false);
    }
    public void Heal(CardController friendCard)
    {
        model.Heal(friendCard);
        friendCard.RefreshView();
    }
    public void DestroyCard(CardController destroyCard)
    {
        model.DestroyCard(destroyCard);
    }
    // 一枚カードをひく
    public void DrawCard1()
    {
        if (model.isPlayerCard)
        {
            gameManager.Player.DrawCard();
        }
        else
        {
            gameManager.Enemy.DrawCard();
        }
    }
    public void RefreshView()
    {
        view.Refresh(model);
    }
    public void Show()
    {
        view.Show();
    }

    public void SetCanAttack(bool canAtk)
    {
        model.canAttack = canAtk;
        view.SetActiveSelectablePanel(canAtk);
    }

    public void OnField()
    {
        gameManager.ReduceManaCost(model.cost, model.isPlayerCard);//コストを支払う
        model.isFieldCard = true;//フィールドカードする
        if (model.ability == ABILITY.INIT_ATTACKABLE)//疾走
        {
            SetCanAttack(true);
        }
        if (model.cip != CIP.NONE)
        {
            ActCIP();
        }
    }

    public IEnumerator CheakAlive()
    {
        RefreshView();
        yield return new WaitForSeconds(0.25f);
        if (model.isAlive)
        {
            RefreshView();
        }
        else
        {
            if (model.pig != PIG.NONE)
            {
                ActPIG();
            }
            StartCoroutine(Trash(gameObject));
        }
        yield break;
    }
    /*CIP*/
    public void ActCIP()
    {
        switch (model.cip)
        {
            case CIP.ALL_DELETE:
                //破壊カードを取得
                //相手フィールド全て
                CardController[] enemycards = gameManager.GetEnemyFieldCards(this.model.isPlayerCard);
                CardController[] destroyCard = Array.FindAll(gameManager.GetFriendFieldCards(this.model.isPlayerCard), card => card != this);
                //破壊
                foreach (CardController Cards in destroyCard)
                {
                    DestroyCard(Cards);
                }
                foreach (CardController Cards in enemycards)
                {
                    DestroyCard(Cards);
                }
                foreach (CardController Cards in destroyCard)
                {
                    StartCoroutine(Cards.CheakAlive());
                }
                foreach (CardController Cards in enemycards)
                {
                    StartCoroutine(Cards.CheakAlive());
                }
                return;
            case CIP.NONE:
                return;
        }
    }


    /*PIG*/
    public void ActPIG()
    {
        switch (model.pig)
        {
            case PIG.DRAW1:
                DrawCard1();
                return;
            case PIG.NONE:
                return;
        }
    }

    /*SPELL*/
    //使えるか
    public bool CanUseSpell()
    {
        switch (model.spell)
        {
            //相手が必要な場合
            case SPELL.DAMAGE_ENEMY_CARD:
            case SPELL.DAMAGE_ENEMY_CARDS:
            case SPELL.DESTROY_ENEMY_CARD:
                CardController[] cards = gameManager.GetEnemyFieldCards(this.model.isPlayerCard);
                if (cards.Length > 0)
                {
                    return true;
                }
                return false;
            //味方が必要な場合
            case SPELL.HEAL_FRIEND_CARD:
            case SPELL.HEAL_FRIEND_CARDS:
                CardController[] friendCards = gameManager.GetFriendFieldCards(this.model.isPlayerCard);
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
    


    //実施
    public void UseSpellTo(CardController target)
    {
        switch (model.spell)
        {
            case SPELL.DAMAGE_ENEMY_CARD:
                //特定の敵を攻撃する
                if (target == null)//ターゲットがいない場合不発
                {
                    return;
                }
                if (target.model.isPlayerCard == model.isPlayerCard)//自分のカードには使えない
                {
                    return;
                }
                Attack(target);
                StartCoroutine(target.CheakAlive());
                break;
            case SPELL.DAMAGE_ENEMY_CARDS:
                //相手フィールド全てに攻撃する
                CardController[]cards= gameManager.GetEnemyFieldCards(this.model.isPlayerCard);
                //回ってる途中に削除されると怖いから二回呼ぶ
                foreach(CardController enemyCard in cards)
                {
                    Attack(enemyCard);
                }
                foreach (CardController enemyCard in cards)
                {
                    StartCoroutine(enemyCard.CheakAlive());
                }
                break;
            case SPELL.DAMAGE_ENEMY_HERO:
                // tekihero
                gameManager.AttackToHero(this);
                break;
            case SPELL.HEAL_FRIEND_CARD:
                if (target == null)
                {
                    return;
                }
                if (target.model.isPlayerCard != model.isPlayerCard)//相手のカードには使えない
                {
                    return;
                }
                Heal(target);
                break;
            case SPELL.HEAL_FRIEND_CARDS:
                CardController[] friendCards = gameManager.GetFriendFieldCards(this.model.isPlayerCard);
                foreach(CardController friendCard in friendCards)
                {
                    Heal(friendCard);
                }
                break;
            case SPELL.HEAL_FRIEND_HERO:
                gameManager.HealToHero(this);
                break;
            case SPELL.DRAW_CARD:
                DrawCard1();
                break;
            case SPELL.DRAW_2CARD:
                for (int i = 0; i < 2; i++)
                {
                    DrawCard1();
                }
                break;
            case SPELL.DESTROY_ENEMY_CARD:
                //特定の敵をDESTROYする
                if (target == null)//ターゲットがいない場合不発
                {
                    return;
                }
                if (target.model.isPlayerCard == model.isPlayerCard)//自分のカードには使えない
                {
                    return;
                }
                DestroyCard(target);
                StartCoroutine(target.CheakAlive());
                break;
            case SPELL.NONE:
                return;
        }
        gameManager.ReduceManaCost(model.cost, model.isPlayerCard);//コストを支払う
        StartCoroutine(Trash(this.gameObject));
    }

    private IEnumerator Trash(GameObject gameObject)
    {
        gameObject.GetComponent<CanvasGroup>().alpha = 0;
        yield return new WaitForSeconds(0.51f);
        Destroy(this.gameObject);
    }
}
