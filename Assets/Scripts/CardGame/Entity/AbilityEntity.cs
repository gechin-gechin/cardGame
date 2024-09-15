using System;
using UnityEngine;

namespace CardGame
{
    public abstract class AbilityEntity<T> : ScriptableObject
    {
        [TextArea]
        [SerializeField] protected string _description;
        public abstract AbilityTarget Target { get; }
        public abstract AbilityTiming Timing { get; }
        public abstract Action<T> Process { get; }
        public string Description => _description;
    }
}
