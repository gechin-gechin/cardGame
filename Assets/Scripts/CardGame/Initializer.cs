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

        private CompositeDisposable _disposables;

        private void Awake()
        {
            _disposables = new();
            //model
            var player = new Player();
            var enemy = new Player();
            var main = new Main(player, enemy);
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
            var pp = new PlayerPresenter();
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
