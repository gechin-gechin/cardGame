using System;
using UnityEngine;
using R3;
using System.Collections.Generic;

namespace CardGame
{
    public class Trap : IDisposable
    {
        public Action OnDead;
        public int PlayerID { get; private set; }
        public string Name { get; private set; }
        public Sprite Sprite_ { get; private set; }
        public List<Ability> Abilities { get; private set; }
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
        public void SetAbility(List<Ability> abilities)
        {
            Abilities = abilities;
        }

        public void TakeDamage(int num)
        {
            _life.Value -= num;
            if (_life.Value <= 0)
            {
                OnDead?.Invoke();
            }
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
