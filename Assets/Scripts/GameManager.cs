using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;
using Cysharp.Threading.Tasks;
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
    private int timeCount = 20;

    private void Start()
    {
        //共通の初期化
        UI.TurnEndButton.onClick.AsObservable()
            .Where(_ => isplayerTurn)
            .Subscribe(_ => ChangeTurn()).AddTo(this);
            
        enemyAI.Init(player, enemy);
        enemyAI.TurnEnd.Subscribe(_=>ChangeTurn()).AddTo(this);

        //timer
        Observable.Interval(TimeSpan.FromSeconds(1), destroyCancellationToken)
            .Subscribe(_ =>
            {
                timeCount--;
                UI.UpdateTime(timeCount);
                if(timeCount <= 0)
                    ChangeTurn();
            }).AddTo(this);

        //リセット時の初期化
        StartGame();
    }

    private void StartGame()
    {
        player.Init(true,8);
        enemy.Init(false,7);
        timeCount =  20;
        isplayerTurn = true;
        TurnCalc();
        UI.HideResultPanel();
    }
    

    //turn no shori
    private void TurnCalc()
    {
        timeCount = 20;
        if (!isplayerTurn)
        {
            StartCoroutine(enemyAI.EnemyrTurn());
        }
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
