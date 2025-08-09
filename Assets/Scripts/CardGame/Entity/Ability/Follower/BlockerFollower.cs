using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CardGame
{
    [CreateAssetMenu(fileName = "BlockerFollower", menuName = "Ability/BlockerFollower", order = 0)]
    public class BlockerFollower : AbilityEntity<Follower>
    {
        public override AbilityTarget Target => AbilityTarget.PLAYER;

        public override AbilityTiming Timing => AbilityTiming.Common;

        public override Func<Follower, UniTask> Process => (f) => UniTask.Defer(async () =>
        {
            f.SetIsBlocker(true);
            await UniTask.DelayFrame(1);
        });
    }
}
