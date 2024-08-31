using System.Collections;
using System.Collections.Generic;
using R3;
using UnityEngine;

namespace CardGame
{
    public class Initializer : MonoBehaviour
    {
        [SerializeField] private MainView _mainView;
        [SerializeField] private PlayerView _playerView;
        [SerializeField] private PlayerView _enemyView;
        [Header("provider")]
        [SerializeField] private CardProvider _cardProvider;
        [SerializeField] private FollowerProvider _followerProvider;

        private CompositeDisposable _disposables;

        private async void Awake()
        {
            _disposables = new();
            //repository
            var leaderRepo = new LeaderRepository();
            var cardRepo = new CardRepository();
            //model
            var player = new Player(cardRepo, leaderRepo);
            var enemy = new Player(cardRepo, leaderRepo);
            await player.CreateLeader();
            await enemy.CreateLeader();
            await player.CreateDeck();
            await enemy.CreateDeck();
            var main = new Main(player, enemy);
            _cardProvider.Init(8);
            _followerProvider.Init(6);
            //model AddTo
            main.AddTo(_disposables);
            player.AddTo(_disposables);
            enemy.AddTo(_disposables);
            //view Init
            _mainView.Init();
            _playerView.Init();
            _enemyView.Init();

            //presenter
            //main
            var mp = new MainPresenter();
            mp.Bind(main, _mainView);
            mp.AddTo(_disposables);
            //player
            var pp = new PlayerPresenter(_cardProvider, _followerProvider);
            pp.Bind(player, _playerView);
            pp.Bind(enemy, _enemyView);
            pp.AddTo(_disposables);

            //準備が終わり次第スタート
            main.Start();
        }

        private void OnDestroy()
        {
            _disposables.Dispose();
        }
    }
}
