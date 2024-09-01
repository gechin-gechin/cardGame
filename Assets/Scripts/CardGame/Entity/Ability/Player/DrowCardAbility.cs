using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardGame
{
    [CreateAssetMenu(fileName = "DrowCard", menuName = "Ability/DrowCard", order = 0)]
    public class DrowCardAbility : AbilityEntity<Player>
    {
        [SerializeField] private AbilityTiming _timing;
        [SerializeField] private int _num;

        public override AbilityTiming Timing => _timing;

        public override Action<Player> Process => (player) =>
        {
            for (int i = 0; i < _num; i++)
            {
                player.Drow();
            }
        };
    }
}
