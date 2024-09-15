using UnityEngine;
using System;

namespace CardGame
{
    [CreateAssetMenu(fileName = "SpeedAttacker", menuName = "Ability/SpeedAttacker", order = 0)]
    public class SpeedAttacker : AbilityEntity<Follower>
    {
        public override AbilityTarget Target => AbilityTarget.PLAYER;

        public override AbilityTiming Timing => AbilityTiming.Common;

        public override Action<Follower> Process => (f) =>
        {
            f.SetIsAttackAble(true);
        };
    }
}
