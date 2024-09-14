using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;

namespace CardGame
{
    public partial class Player
    {
        private Follower CardToFollower(Card card)
        {
            var f = new Follower(
                PlayerID,
                GetInitID(),
                card.Name,
                card.Power.CurrentValue,
                card.Sprite_
            );
            //バトル
            f.OnBattle = (id) => TryBattle(f, id);
            //能力
            List<Ability> abilities = new();
            foreach (var a in card.AbilitiesToPlayer)
            {
                var ab = new Ability(
                    a.Timing,
                    () => a.Process?.Invoke(this)
                );
                abilities.Add(ab);
            }
            foreach (var a in card.AbilitiesToFollower)
            {
                var ab = new Ability(
                    a.Timing,
                    () => a.Process?.Invoke(f)
                );
                abilities.Add(ab);
            }
            f.SetAbility(abilities);
            //後処理
            f.AddTo(_disposables);
            f.OnDead += () => AddTrash(card);
            //PIG
            var absf = f.Abilities.Where(a => a.Timing == AbilityTiming.PIG).ToArray();
            if (absf != null)
            {
                foreach (var a in absf)
                {
                    f.OnDead += () => a.Process?.Invoke();
                }
            }
            return f;
        }

        private Trap CardToTrap(Card card)
        {
            var t = new Trap(
                PlayerID,
                card.Name,
                card.Power.CurrentValue,
                card.Sprite_
            );
            //damage
            t.GetEnemyFollower = (initid) => TryTakeDamge(initid, t.IsBlocker.CurrentValue);
            //能力
            List<Ability> abilities = new();
            foreach (var a in card.AbilitiesToPlayer)
            {
                var ab = new Ability(
                    a.Timing,
                    () => a.Process?.Invoke(this)
                );
                abilities.Add(ab);
            }
            foreach (var a in card.AbilitiesToTrap)
            {
                var ab = new Ability(
                    a.Timing,
                    () => a.Process?.Invoke(t)
                );
                abilities.Add(ab);
            }
            t.SetAbility(abilities);
            t.AddTo(_disposables);
            t.OnDead += () => AddTrash(card);
            //PIG
            var abst = t.Abilities.Where(a => a.Timing == AbilityTiming.PIG).ToArray();
            if (abst != null)
            {
                foreach (var a in abst)
                {
                    t.OnDead += () => a.Process?.Invoke();
                }
            }
            return t;
        }
    }
}
