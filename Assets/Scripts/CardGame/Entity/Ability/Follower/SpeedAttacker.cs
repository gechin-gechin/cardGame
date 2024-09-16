using UnityEngine;
using System;
using Cysharp.Threading.Tasks;
using R3;

namespace CardGame
{
    [CreateAssetMenu(fileName = "SpeedAttacker", menuName = "Ability/SpeedAttacker", order = 0)]
    public class SpeedAttacker : AbilityEntity<Follower>
    {
        public override AbilityTarget Target => AbilityTarget.PLAYER;

        public override AbilityTiming Timing => AbilityTiming.Common;

        public override Func<Follower, UniTask> Process => (f) => UniTask.Defer(async () =>
        {
            f.SetIsAttackAble(true);
            await UniTask.DelayFrame(1);
        });
    }
}
