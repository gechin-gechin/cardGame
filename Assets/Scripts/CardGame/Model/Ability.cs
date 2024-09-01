using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardGame
{
    public class Ability
    {
        public Action Process { get; private set; }
        public AbilityTiming Timing { get; private set; }
        public Ability(AbilityTiming timing, Action process)
        {
            Process = process;
            Timing = timing;
        }
    }
}
