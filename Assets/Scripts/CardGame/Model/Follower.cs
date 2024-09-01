using System;
using System.Collections;
using System.Collections.Generic;
using R3;
using UnityEngine;

namespace CardGame
{
    public class Follower : IDisposable
    {
        public Action OnDead;
        public int PlayerID { get; private set; }
        public string Name { get; private set; }
        public List<Ability> Abilities { get; private set; }
        public Sprite Sprite_ { get; private set; }
        private ReactiveProperty<int> _power;
        public ReadOnlyReactiveProperty<int> Power => _power;
        private ReactiveProperty<bool> _isAttackAble;
        public ReadOnlyReactiveProperty<bool> IsAttackAble => _isAttackAble;
        private CompositeDisposable _disposables;

        public Follower(int playerID, string name, int power, Sprite sprite)
        {
            PlayerID = playerID;
            _disposables = new();
            Name = name;
            _power = new(power);
            _isAttackAble = new(false);
            Sprite_ = sprite;
        }
        public void SetAbility(List<Ability> abilities)
        {
            Abilities = abilities;
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }

        public void SetIsAttackAble(bool value)
        {
            _isAttackAble.Value = value;
        }

        public void EndAttack(bool isdead)
        {
            if (isdead)
            {
                OnDead?.Invoke();
            }
            else
            {
                _isAttackAble.Value = false;
            }
        }

        public void EndBattle(bool isdead)
        {
            if (isdead)
            {
                OnDead?.Invoke();
            }
        }
    }
}
