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

    //バトル用
    //双方の計算が終わってからCheakAlive()したいので、分けてある
    public void Attack(CardController enemyCard)
    {
        model.Attack(enemyCard);
        model.canAttack.Value = false;
    }
    public IEnumerator CheakAlive()
    {
        yield return new WaitForSeconds(0.25f);
        if (!model.isAlive)
        {
            if (model.pig != PIG.NONE)
            {
                PIGData.Act(this);
            }
            StartCoroutine(Trash(gameObject));
        }
        yield break;
    }
    //カードを公開する
    public void Show()
    {
        view.Show();
    }
    //フィールド登録処理
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
            CIPData.Act(this);
        }
    }

    //具体的な処理群、今はまだ置いておく
    public void Heal(CardController friendCard)
    {
        model.Heal(friendCard);
    }
    public void DestroyCard(CardController destroyCard)
    {
        model.DestroyCard(destroyCard);
    }
    // 一枚カードをひく
    public void DrowCard()
    {
        GameManager.I.DrowCard(model.isPlayerCard);
    }

    /*SPELL*/
    //使えるか
    public bool CanUseSpell()
    {
        return SpellData.CheckUseAble(model);
    }
    //実施
    public void UseSpellTo(CardController target)
    {
        SpellData.Use(target, this);
        useThis.OnNext(model.cost);//コストを支払う
        StartCoroutine(Trash(this.gameObject));
    }

    //墓地に送る処理
    private IEnumerator Trash(GameObject gameObject)
    {
        gameObject.GetComponent<CanvasGroup>().alpha = 0;
        yield return new WaitForSeconds(0.51f);
        Destroy(this.gameObject);
    }
}
