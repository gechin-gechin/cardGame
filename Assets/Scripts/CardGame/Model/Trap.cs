using System;
using UnityEngine;
using R3;

namespace CardGame
{
    public class Trap : IDisposable
    {
        public string Name { get; set; }
        public Sprite Sprite_ { get; set; }
        private ReactiveProperty<int> _life;
        public ReadOnlyReactiveProperty<int> Life => _life;
        private CompositeDisposable _disposables;

        public Trap(string name, int life, Sprite sprite)
        {
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
