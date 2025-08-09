using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CardGame
{
    [CreateAssetMenu(fileName = "BattlePowerChange", menuName = "Ability/BattlePowerChange", order = 0)]
    public class BattlePowerChange : AbilityEntity<Follower>
    {
        [SerializeField] private int _amount = 0;
        [SerializeField] private bool _isMe = true;
        public override AbilityTarget Target => AbilityTarget.PLAYER;
        public override AbilityTiming Timing => AbilityTiming.Battle;

        public override Func<Follower, UniTask> Process => (f) => UniTask.Defer(async () =>
        {
            var follower = _isMe ? f : f.BattleFollower;
            follower.ChangePower(_amount);
            await UniTask.WaitForSeconds(0.2f);
        });
    }
}
