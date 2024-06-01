using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using R3;   //UniRxから書き換え

public class GameManager : Singleton<GameManager>
{
    // whose turn?
    public bool isplayerTurn { get; private set;}

    public Transform heroTransform;
    [SerializeField] Transform enemyTransform;

    //other scripts
    [SerializeField] AI enemyAI;
    [SerializeField] UIManager uiManager;

    public GamePlayerManager Player;
    public GamePlayerManager Enemy;
    public GamePlayerManager gamePlayer(bool isPlayer)
    {
        return isPlayer ? Player : Enemy;
    }


    //TIME
    private ReactiveProperty<int> timeCount = new ReactiveProperty<int>();


    void Start()
    {
        StartGame();
        timeCount.Subscribe(time => uiManager.UpdateTime(time)).AddTo(this);
    }

    void StartGame()
    {
        Player.Init(true,8);
        Enemy.Init(false,7);
        timeCount.Value =  20;
        isplayerTurn = true;
        TurnCalc();
        uiManager.HideResultPanel();
    }
    

    //turn no shori
    private void TurnCalc()
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

    private IEnumerator CountDownTime()
    {
        timeCount.Value = 20;
        while (timeCount.Value >0)
        {
            yield return new WaitForSeconds(1);
            timeCount.Value--;
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

    private void PlayerTurn()
    {
        Debug.Log("player turn");
        //フィールドカードを攻撃表示にする。
        SettingCanAttackView(Player.GetFieldCards(), true);
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
        //shujinko ja nai kara
        gamePlayer(!attacker.model.isPlayerCard).TakeDamage(attacker.model.at);
        attacker.SetCanAttack(false);
    }
    public void HealToHero(CardController healer)
    {
        gamePlayer(healer.model.isPlayerCard).TakeHeal(healer.model.at);
    }

    
    public void ReduceManaCost(int cost, bool isPlayerCard)
    {
        gamePlayer(isPlayerCard).ReduceManaCost(cost);
    }

    public void ShowResultPanel()
    {
        StopAllCoroutines();
        uiManager.ShowResultPanel(Player.HP.CurrentValue);
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
        return gamePlayer(!isPlayer).GetFieldCards();
    }
    //mikatanoka-do
    public CardController[] GetFriendFieldCards(bool isPlayer)
    {
        return gamePlayer(isPlayer).GetFieldCards();
    }

    public void ChangeTurn()
    {
        SettingCanAttackView(Player.GetFieldCards(), false);
        SettingCanAttackView(Enemy.GetFieldCards(), false);
        isplayerTurn = !isplayerTurn;
        //draw
        if (isplayerTurn)
        {
            Player.IncreaseManaCost();
            Player.DrawCard();
        }
        else
        {
            Enemy.IncreaseManaCost();
            Enemy.DrawCard();
        }
        TurnCalc();
    }

    public void Restart()
    {
        Player.Restart();
        Enemy.Restart();
        StartGame();
    }
}
