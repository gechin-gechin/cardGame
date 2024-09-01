using System;
using System.Collections;
using System.Collections.Generic;
using R3;
using UnityEngine;

namespace CardGame
{
    public class Follower : IDisposable
    {
        public string Name { get; set; }
        public Sprite Sprite_ { get; set; }
        private ReactiveProperty<int> _power;
        public ReadOnlyReactiveProperty<int> Power => _power;
        private CompositeDisposable _disposables;

        public Follower(string name, int power, Sprite sprite)
        {
            _disposables = new();
            Name = name;
            _power = new(power);
            Sprite_ = sprite;
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
