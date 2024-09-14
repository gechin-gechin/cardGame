using System;
using UnityEngine;

namespace CardGame
{
    [CreateAssetMenu(fileName = "BlockerFollower", menuName = "Ability/BlockerFollower", order = 0)]
    public class BlockerFollower : AbilityEntity<Follower>
    {
        public override AbilityTarget Target => AbilityTarget.PLAYER;

        public override AbilityTiming Timing => AbilityTiming.Common;

        public override Action<Follower> Process => (f) =>
        {
            f.SetIsBlocker(true);
        };
    }
}
