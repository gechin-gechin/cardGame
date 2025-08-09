using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CardGame
{
    public abstract class AbilityEntity<T> : ScriptableObject
    {
        [TextArea]
        [SerializeField] protected string _description;
        public abstract AbilityTarget Target { get; }
        public abstract AbilityTiming Timing { get; }
        public abstract Func<T, UniTask> Process { get; }
        public string Description => _description;
    }
}
