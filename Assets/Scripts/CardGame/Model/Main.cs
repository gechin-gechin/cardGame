using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using R3;

namespace CardGame
{
    public class Main : IDisposable
    {
        public Action<string> OnMessage;

        private IPlayer _player;
        private IPlayer _enemy;
        private CancellationTokenSource _timeOver_cts;

        //RP
        private ReactiveProperty<int> _countdownTime;
        public ReadOnlyReactiveProperty<int> CountDownTime => _countdownTime;

        private CompositeDisposable _disposables;

        public Main(IPlayer player, IPlayer enemy)
        {
            //キャッシュ
            _player = player;
            _enemy = enemy;
            //初期化
            _disposables = new();
            _timeOver_cts = new();

            _countdownTime = new(60);
            _countdownTime.AddTo(_disposables);
            //デリゲートの登録
            _player.OnTurnEnd += () => ChangeTurn(_enemy);
            _enemy.OnTurnEnd += () => ChangeTurn(_player);
            _player.OnMessage = (str) => OnMessage?.Invoke(str);
            _enemy.OnMessage = (str) => OnMessage?.Invoke(str);
        }

        public void Start()
        {
            //選考を決める
            var senkou = _player;
            //先行側のターンを開始
            ChangeTurn(senkou);
        }

        private void ChangeTurn(IPlayer nextPlayer)
        {
            _timeOver_cts = _timeOver_cts.CancelAndNew();

            nextPlayer.StartTurn();
            _ = TimeOverCountDown(nextPlayer, 60, 1f);
        }


        private async UniTask TimeOverCountDown(IPlayer turnPlayer, int count, float interval)
        {
            _countdownTime.Value = count;
            while (_countdownTime.Value > 0)
            {
                await UniTask.WaitForSeconds(interval, cancellationToken: _timeOver_cts.Token);
                _countdownTime.Value--;
            }
            turnPlayer.TimeOver();
        }



        public void Dispose()
        {
            _timeOver_cts.Cancel();
            _timeOver_cts.Dispose();

            _disposables.Dispose();
        }
    }
}
