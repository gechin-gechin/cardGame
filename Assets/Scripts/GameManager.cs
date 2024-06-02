using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using R3;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private GamePlayerManager player = null;
    [SerializeField] private GamePlayerManager enemy = null;
    private GamePlayerManager gamePlayer(bool isPlayer)
    {
        return isPlayer ? player : enemy;
    }

    //other scripts
    [SerializeField] private AI enemyAI;
    [SerializeField] private UIManager UI;
    // whose turn?
    public bool isplayerTurn { get; private set; }
    //TIME
    private ReactiveProperty<int> timeCount = new ReactiveProperty<int>();


    void Start()
    {
        timeCount.Subscribe(time => UI.UpdateTime(time)).AddTo(this);
        UI.TurnEndButton.onClick.AddListener(()=> {
            if (isplayerTurn)
                ChangeTurn();
            });
        enemyAI.Init(player, enemy);
        enemyAI.TurnEnd += ChangeTurn;

        StartGame();
    }

    void StartGame()
    {
        player.Init(true,8);
        enemy.Init(false,7);
        timeCount.Value =  20;
        isplayerTurn = true;
        TurnCalc();
        UI.HideResultPanel();
    }
    

    //turn no shori
    private void TurnCalc()
    {
        StopAllCoroutines();
        StartCoroutine(CountDownTime());
        if (!isplayerTurn)
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
    private void ChangeTurn()
    {
        isplayerTurn = !isplayerTurn;
        player.ChangeTurn(isplayerTurn);
        enemy.ChangeTurn(isplayerTurn);
        TurnCalc();
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
        gamePlayer(!attacker.model.isPlayerCard).TakeDamage(attacker.model.At.CurrentValue);
        attacker.model.canAttack.Value = false;
    }
    public void HealToHero(CardController healer)
    {
        gamePlayer(healer.model.isPlayerCard).TakeHeal(healer.model.At.CurrentValue);
    }

    public void ShowResultPanel()
    {
        StopAllCoroutines();
        UI.ShowResultPanel(player.HP.CurrentValue);
    }

    public void Restart()
    {
        player.Restart();
        enemy.Restart();
        StartGame();
    }

    public CardController[] GetFieldCards(bool isPlayer)
    {
        return gamePlayer(isPlayer).GetFieldCards();
    }
    public int GetPlayerManaCost()
    {
        return player.ManaCost.CurrentValue;
    }

    public void DrowCard(bool isPlayer)
    {
        gamePlayer(isPlayer).DrowCard();
    }
}
