using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public Transform PlayerHandTransform,
                    PlayerFieldTransform,
                    EnemyHandTransform,
                    EnemyFieldTransform,
                    playerHeroTransform,
                    enemyHeroTransform;

    [SerializeField] CardController cardPrefab;
    // whose turn?
    public bool isplayerTurn;

    public Transform heroTransform;
    [SerializeField] Transform enemyTransform;

    //other scripts
    [SerializeField] AI enemyAI;
    [SerializeField] UIManager uiManager;
    public GamePlayerManager player;
    public GamePlayerManager enemy;



    //TIME
    int timeCount;


    void Start()
    {
        StartGame();
    }

    void StartGame()
    {
        player.Init(new List<int>() { 5, 7, 9, 10, 6, 3, 10, 5, 2, 2, 5, 2, 10, 3, 3, 4 }, 8);
        enemy.Init(new List<int>() { 10,3,7,8,8,10,10,10,7,6,3,5,2,1,3,4,1,2,3,4,10},7);
        timeCount =  20;
        uiManager.UpdateTime(timeCount);
        SetInitHand();
        isplayerTurn = true;
        TurnCalc();
        uiManager.ShowHeroHP(player.heroHp, enemy.heroHp);
        uiManager.ShowManaCost(player.manaCost,enemy.manaCost);
        uiManager.HideResultPanel();
    }
    void SetInitHand()
    {
        //3mai kubaru
        for (int i = 0; i < 3; i++)
        {
            GiveCardToHand(player.deck,PlayerHandTransform);
            GiveCardToHand(enemy.deck,EnemyHandTransform);
        }
    }
    //カードをひく
    public void GiveCardToHand(List<int>deck,Transform hand)
    {
        if (deck.Count == 0)
        {
            return;
        }
        int cardID = deck[0];
        deck.RemoveAt(0);
        CreateCard(cardID, hand);
    }

    void CreateCard(int cardID ,Transform hand)
    {
        CardController card = Instantiate(cardPrefab, hand, false);
        if (hand.name == "PlayerHand")
        {
            card.Init(cardID,true);
        }
        else
        {
            card.Init(cardID, false);
        }

    }

    //turn no shori
    void TurnCalc()
    {
        StopAllCoroutines();
        StartCoroutine(CountDownTime());
        if (isplayerTurn)
        {
            PlayerTurn();
        }
        else
        {
            StartCoroutine(enemyAI.EnemyrTurn());
        }
    }

    IEnumerator CountDownTime()
    {
        timeCount = 20;
        uiManager.UpdateTime(timeCount);
        while (timeCount>0)
        {
            yield return new WaitForSeconds(1);
            timeCount--;
            uiManager.UpdateTime(timeCount);
        }
        ChangeTurn();
    }

    public void SettingCanAttackView(CardController[] fieldCardList,bool canAttack)
    {
        foreach (CardController card in fieldCardList)
        {
            card.SetCanAttack(canAttack);
        }
    }

    void PlayerTurn()
    {
        Debug.Log("player turn");
        //フィールドカードを攻撃表示にする。
        CardController[] playerFieldCardList = PlayerFieldTransform.GetComponentsInChildren<CardController>();
        SettingCanAttackView(playerFieldCardList, true);
    }

    //Battle!
    public void CardsBattle(CardController attaker, CardController defender)
    {
        attaker.Attack(defender);
        defender.Attack(attaker);
        StartCoroutine(attaker.CheakAlive());
        StartCoroutine(defender.CheakAlive());
    }

    //主人公に攻撃
    public void AttackToHero(CardController attacker)
    {
        if (attacker.model.isPlayerCard)
        {
            enemy.heroHp -= attacker.model.at;
        }
        else
        {
            player.heroHp -= attacker.model.at;
        }
        attacker.SetCanAttack(false);
        uiManager.ShowHeroHP(player.heroHp,enemy.heroHp);
    }
    public void HealToHero(CardController healer)
    {
        if (healer.model.isPlayerCard)
        {
            player.heroHp += healer.model.at;
        }
        else
        {
            enemy.heroHp += healer.model.at;
        }
        uiManager.ShowHeroHP(player.heroHp, enemy.heroHp);
    }

    
    public void ReduceManaCost(int cost, bool isPlayerCard)
    {
        if (isPlayerCard)
        {
            player.manaCost -= cost;
        }
        else
        {
            enemy.manaCost -= cost;
        }
        uiManager.ShowManaCost(player.manaCost,enemy.manaCost);
    }

    public void CheckHeroHP()
    {
        if (player.heroHp <= 0 || enemy.heroHp <= 0)
        {
            ShowResultPanel(player.heroHp);
        }
    }
    void ShowResultPanel(int heroHP)
    {
        StopAllCoroutines();
        uiManager.ShowResultPanel(heroHP);
    }

    public void OnClickTurnENdButton()
    {
        if (isplayerTurn)
        {
            ChangeTurn();
        }
    }
    // tekinoka-do
    public CardController[] GetEnemyFieldCards(bool isPlayer)
    {
        if (isPlayer)
        {
            return EnemyFieldTransform.GetComponentsInChildren<CardController>();
        }
        else
        {
            return PlayerFieldTransform.GetComponentsInChildren<CardController>();
        }
    }
    //mikatanoka-do
    public CardController[] GetFriendFieldCards(bool isPlayer)
    {
        if (isPlayer)
        {
            return PlayerFieldTransform.GetComponentsInChildren<CardController>();
        }
        else
        {
            return EnemyFieldTransform.GetComponentsInChildren<CardController>();
        }
    }

    public void ChangeTurn()
    {
        CardController[] playerFieldCardList = PlayerFieldTransform.GetComponentsInChildren<CardController>();
        SettingCanAttackView(playerFieldCardList, false);
        CardController[] enemyFieldCardList = EnemyFieldTransform.GetComponentsInChildren<CardController>();
        SettingCanAttackView(enemyFieldCardList, false);
        isplayerTurn = !isplayerTurn;
        //draw
        if (isplayerTurn)
        {
            player.IncreaseManaCost();
            GiveCardToHand(player.deck, PlayerHandTransform);
        }
        else
        {
            enemy.IncreaseManaCost();
            GiveCardToHand(enemy.deck, EnemyHandTransform);
        }
        TurnCalc();
        uiManager.ShowManaCost(player.manaCost, enemy.manaCost);
    }

    public void Restart()
    {
        //場のカードを破壊
        foreach (Transform card in PlayerHandTransform)
        {
            Destroy(card.gameObject);
        }
        foreach (Transform card in PlayerFieldTransform)
        {
            Destroy(card.gameObject);
        }
        foreach (Transform card in EnemyHandTransform)
        {
            Destroy(card.gameObject);
        }
        foreach (Transform card in EnemyFieldTransform)
        {
            Destroy(card.gameObject);
        }
        //デッキを元に戻す
        player.deck = new List<int>() { 1, 1, 2, 3, 1, 4, 2, 2 };
        enemy.deck = new List<int>() { 1, 2, 1, 2, 4, 1, 2, 3 };
        StartGame();
    }
}
