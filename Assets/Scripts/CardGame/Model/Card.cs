using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;

namespace CardGame
{
    public class Card
    {
        public int ID { get; private set; }
        public string Name { get; private set; }
        public int Cost { get; private set; }
        private ReactiveProperty<int> _power;
        public ReadOnlyReactiveProperty<int> Power => _power;
        public Sprite Sprite_ { get; private set; }
        public CardKind Kind { get; private set; }

        public AbilityEntity<Player>[] AbilitiesToPlayer { get; private set; }
        public AbilityEntity<Follower>[] AbilitiesToFollower { get; private set; }
        public AbilityEntity<Trap>[] AbilitiesToTrap { get; private set; }

        public Func<UniTask<bool>> TryUse;

        public Card(int id, string name, int cost, CardKind kind, int power, Sprite sprite,
        AbilityEntity<Player>[] abilitiesToPlayer, AbilityEntity<Follower>[] abilitiesToFollower, AbilityEntity<Trap>[] abilitiesToTrap)
        {
            ID = id;
            Name = name;
            Cost = cost;
            Kind = kind;
            _power = new(power);
            Sprite_ = sprite;

            AbilitiesToPlayer = abilitiesToPlayer;
            AbilitiesToFollower = abilitiesToFollower;
            AbilitiesToTrap = abilitiesToTrap;
        }
    }
}
