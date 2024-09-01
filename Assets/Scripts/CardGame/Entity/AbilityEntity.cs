using System;
using UnityEngine;

namespace CardGame
{
    public abstract class AbilityEntity<T> : ScriptableObject
    {
        public abstract AbilityTiming Timing { get; }
        public abstract Action<T> Process { get; }
    }
}
