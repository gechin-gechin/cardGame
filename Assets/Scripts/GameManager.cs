using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using R3;   //UniRxから書き換え

public class GameManager : Singleton<GameManager>
{
    public Transform playerHeroTransform,
                    enemyHeroTransform;
    // whose turn?
    public bool isplayerTurn;

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
    int timeCount;


    void Start()
    {
        StartGame();
        Player.HP.Subscribe(hp => {
            uiManager.ShowPlayerHp(hp);
            if (hp <= 0) {
                ShowResultPanel(Player.HP.CurrentValue);
            }
        }).AddTo(Player);
        Enemy.HP.Subscribe(hp => {
            uiManager.ShowEnemyHp(hp);
            if (hp <= 0)
            {
                ShowResultPanel(Player.HP.CurrentValue);
            }
        }).AddTo(Enemy);
    }

    void StartGame()
    {
        Player.Init(true,8);
        Enemy.Init(false,7);
        timeCount =  20;
        uiManager.UpdateTime(timeCount);
        isplayerTurn = true;
        TurnCalc();
        uiManager.ShowManaCost(Player.manaCost,Enemy.manaCost);
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
        if (isPlayerCard)
        {
            Player.manaCost -= cost;
        }
        else
        {
            Enemy.manaCost -= cost;
        }
        uiManager.ShowManaCost(Player.manaCost,Enemy.manaCost);
    }

    private void ShowResultPanel(int heroHP)
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
        uiManager.ShowManaCost(Player.manaCost, Enemy.manaCost);
    }

    public void Restart()
    {
        Player.Restart();
        Enemy.Restart();
        StartGame();
    }
}
