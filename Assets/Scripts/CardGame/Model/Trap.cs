using System;
using UnityEngine;
using R3;

namespace CardGame
{
    public class Trap : IDisposable
    {
        public int PlayerID { get; private set; }
        public string Name { get; private set; }
        public Sprite Sprite_ { get; private set; }
        private ReactiveProperty<int> _life;
        public ReadOnlyReactiveProperty<int> Life => _life;
        private CompositeDisposable _disposables;

        public Trap(int playerID, string name, int life, Sprite sprite)
        {
            PlayerID = playerID;
            _disposables = new();
            Name = name;
            _life = new(life);
            Sprite_ = sprite;
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
