using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CardGame
{
    public class Ability
    {
        public Func<UniTask> Process { get; private set; }
        public AbilityTiming Timing { get; private set; }
        public string Description { get; private set; }
        public Ability(AbilityTiming timing, Func<UniTask> process, string description)
        {
            Process = process;
            Timing = timing;
            Description = description;
        }
    }
}
