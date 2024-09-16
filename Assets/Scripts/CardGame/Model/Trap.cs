using System;
using UnityEngine;
using R3;
using System.Collections.Generic;

namespace CardGame
{
    public class Trap : IDisposable, ISelectable
    {
        public Action OnDead;
        public Action OnSelect;
        public Func<int, Follower> GetEnemyFollower;//nullの場合攻撃失敗
        public int PlayerID { get; private set; }
        public string Name { get; private set; }
        public Sprite Sprite_ { get; private set; }
        public List<Ability> Abilities { get; private set; }
        private ReactiveProperty<int> _life;
        public ReadOnlyReactiveProperty<int> Life => _life;
        private ReactiveProperty<bool> _isBlocker;
        public ReadOnlyReactiveProperty<bool> IsBlocker => _isBlocker;
        private ReactiveProperty<bool> _isSelectable;
        public ReadOnlyReactiveProperty<bool> IsSelectable => _isSelectable;

        private CompositeDisposable _disposables;

        public Trap(int playerID, string name, int life, Sprite sprite)
        {
            PlayerID = playerID;
            _disposables = new();
            Name = name;
            _life = new(life);
            _isBlocker = new(false);
            _isSelectable = new(false);
            Sprite_ = sprite;
        }
        public void SetAbility(List<Ability> abilities)
        {
            Abilities = abilities;
        }

        public void TakeDamage(int initID)
        {
            var enemy = GetEnemyFollower(initID);
            if (enemy == null)
            {
                return;
            }
            _life.Value -= enemy.Power.CurrentValue;
            if (_life.Value <= 0)
            {
                Dead();
            }
        }

        public void Dead()
        {
            OnDead?.Invoke();
        }

        public void ChangePower(int amount)
        {
            _life.Value += amount;
        }

        public void SetIsBlocker(bool value)
        {
            _isBlocker.Value = value;
        }

        public void SetSelectable(bool value)
        {
            _isSelectable.Value = value;
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
