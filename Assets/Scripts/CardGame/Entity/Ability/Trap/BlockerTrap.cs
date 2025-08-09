using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CardGame
{
    [CreateAssetMenu(fileName = "BlockerTrap", menuName = "Ability/BlockerTrap", order = 0)]
    public class BlockerTrap : AbilityEntity<Trap>
    {
        public override AbilityTarget Target => AbilityTarget.PLAYER;

        public override AbilityTiming Timing => AbilityTiming.Common;

        public override Func<Trap, UniTask> Process => (t) => UniTask.Defer(async () =>
        {
            t.SetIsBlocker(true);
            await UniTask.DelayFrame(2);
        });
    }
}
