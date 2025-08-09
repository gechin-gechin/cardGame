using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CardGame
{
    [CreateAssetMenu(fileName = "DrowCard", menuName = "Ability/DrowCard", order = 0)]
    public class DrowCardAbility : AbilityEntity<Player>
    {
        [SerializeField] private AbilityTiming _timing;
        [SerializeField] private int _num;

        public override AbilityTiming Timing => _timing;
        public override AbilityTarget Target => AbilityTarget.PLAYER;

        public override Func<Player, UniTask> Process => (p) => UniTask.Defer(async () =>
        {
            for (int i = 0; i < _num; i++)
            {
                p.Drow();
            }
            await UniTask.DelayFrame(1);
        });
    }
}
