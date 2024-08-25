using System;
using System.Collections;
using System.Collections.Generic;
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

        public Func<bool> TryUse;

        public Card(int id, string name, int cost, int power, Sprite sprite)
        {
            ID = id;
            Name = name;
            Cost = cost;
            _power = new(power);
            Sprite_ = sprite;
        }
    }
}
