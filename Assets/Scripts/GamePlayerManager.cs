using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using R3;

public class GamePlayerManager : MonoBehaviour
{
    private bool isPlayer = false;
    //Deck
    [SerializeField]private List<CardEntity> decklist = new List<CardEntity>();//motomoto
    public List<CardEntity> Deck { get; private set; }//yamafuda
    //Hand
    [SerializeField] private Transform icon = null;
    [SerializeField] private Transform hand = null;
    [SerializeField] private Transform field = null;
    [SerializeField] private CardController cardPrefab = null;

    //HP
    private ReactiveProperty<int> hp = new ReactiveProperty<int>();
    public ReadOnlyReactiveProperty<int> HP => hp;
    //COST
    private ReactiveProperty<int> manaCost = new ReactiveProperty<int>();
    public ReadOnlyReactiveProperty<int> ManaCost => manaCost;

    //Propaty
    public Transform Field { get => field; private set => field = value; }//koko to ai
    public Transform Icon { get => icon; private set => icon = value; }

    public int defaultManaCost;

    public void Init(bool isplayer,int mana)
    {
        this.Deck = this.decklist;
        this.isPlayer = isplayer;

        hp.Value = 10;
        manaCost.Value = defaultManaCost = mana;

        //3mai kubaru
        for (int i = 0; i < 3; i++)
        {
            DrawCard();
        }
    }

    //カードをひく
    public void DrawCard()
    {
        if (Deck.Count == 0)
        {
            return;
        }
        CardEntity entity = Deck[0];
        Deck.RemoveAt(0);
        CreateCard(entity, hand);
    }

    private void CreateCard(CardEntity entity, Transform hand)
    {
        CardController card = Instantiate(cardPrefab, hand, false);
        card.Init(entity, isPlayer);
        card.UseThis.Subscribe(cost => {
            manaCost.Value -= cost;
        }).AddTo(card);
    }

    public void IncreaseManaCost()
    {
        if (defaultManaCost < 10)
        {
            defaultManaCost++;
        }
        manaCost.Value = defaultManaCost;
    }

    public CardController[] GetFieldCards()
    {
        return this.Field.GetComponentsInChildren<CardController>();
    }
    public CardController[] GetHandCards()
    {
        return this.hand.GetComponentsInChildren<CardController>();
    }

    public void Restart()
    {
        //場のカードを破壊
        foreach (Transform card in hand)
        {
            Destroy(card.gameObject);
        }
        foreach (Transform card in Field)
        {
            Destroy(card.gameObject);
        }
    }

    public void TakeHeal(int strength)
    {
        hp.Value += strength;
    }

    public void TakeDamage(int strength)
    {
        hp.Value -= strength;
    }

    public void ChangeTurn(bool isplayerTurn)
    {
        //case myturn
        if (isplayerTurn==isPlayer)
        {
            IncreaseManaCost();
            DrawCard();
            SettingCanAttackView(true);
        }
        else
        {
            SettingCanAttackView(false);
        }
    }

    private void SettingCanAttackView(bool canAttack)
    {
        foreach (CardController card in this.GetFieldCards())
        {
            card.model.canAttack.Value = canAttack;
        }
    }
}
