using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using R3;
using R3.Triggers;

public class CardController : MonoBehaviour
{
    private CardView view;          //　見かけ(view)に関することを操作
    public CardModel model { get; private set; }       //　データ(model)に関することを操作
    public CardMovement movement { get; private set; }  //　移動(movement)に関することを操作

    // spell dato true
    public bool IsSpell=> model.spell != SPELL.NONE;
    //use cost propaty
    private Subject<int> useThis = new Subject<int>();
    public Observable<int> UseThis => useThis;

    //INITIALIZE
    private void Awake()
    {
        view = GetComponent<CardView>();
        movement = GetComponent<CardMovement>();
    }
    public void Init(CardEntity entity, bool isPlayer)
    {
        model = new CardModel(entity, isPlayer);
        view.SetCard(model);
        movement.Init(model);
    }

    public void Attack(CardController enemyCard)
    {
        model.Attack(enemyCard);
        model.canAttack.Value = false;
    }
    public void Heal(CardController friendCard)
    {
        model.Heal(friendCard);
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
            GameManager.I.Player.DrawCard();
        }
        else
        {
            GameManager.I.Enemy.DrawCard();
        }
    }
    public void Show()
    {
        view.Show();
    }

    public void OnField()
    {
        useThis.OnNext(model.cost);//コストを支払う
        model.isFieldCard = true;//フィールドカードする
        if (model.ability == ABILITY.INIT_ATTACKABLE)//疾走
        {
            model.canAttack.Value = true;
        }
        if (model.cip != CIP.NONE)
        {
            ActCIP();
        }
    }

    public IEnumerator CheakAlive()
    {
        yield return new WaitForSeconds(0.25f);
        if (!model.isAlive)
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
                CardController[] enemycards = GameManager.I.gamePlayer(!this.model.isPlayerCard).GetFieldCards();
                CardController[] destroyCard = Array.FindAll(
                    GameManager.I.gamePlayer(!this.model.isPlayerCard).GetFieldCards(),
                    card => card != this);
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
                //相手のフィールドカードを取得
                CardController[] cards = GameManager.I.gamePlayer(!this.model.isPlayerCard).GetFieldCards();
                if (cards.Length > 0)
                {
                    return true;
                }
                return false;
            //味方が必要な場合
            case SPELL.HEAL_FRIEND_CARD:
            case SPELL.HEAL_FRIEND_CARDS:
                //味方のフィールドカードを取得
                CardController[] friendCards = GameManager.I.gamePlayer(this.model.isPlayerCard).GetFieldCards();
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
                //相手のフィールドカードを取得
                CardController[] cards = GameManager.I.gamePlayer(!this.model.isPlayerCard).GetFieldCards();
                //回ってる途中に削除されると怖いから二回呼ぶ
                foreach (CardController enemyCard in cards)
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
                GameManager.I.AttackToHero(this);
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
                CardController[] friendCards = GameManager.I.gamePlayer(this.model.isPlayerCard).GetFieldCards();
                foreach(CardController friendCard in friendCards)
                {
                    Heal(friendCard);
                }
                break;
            case SPELL.HEAL_FRIEND_HERO:
                GameManager.I.HealToHero(this);
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
        useThis.OnNext(model.cost);//コストを支払う
        StartCoroutine(Trash(this.gameObject));
    }

    private IEnumerator Trash(GameObject gameObject)
    {
        gameObject.GetComponent<CanvasGroup>().alpha = 0;
        yield return new WaitForSeconds(0.51f);
        Destroy(this.gameObject);
    }
}
