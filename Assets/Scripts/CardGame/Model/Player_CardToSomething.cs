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
            f.OnSelect = () => OnSelectable?.Invoke(f);
            //バトル
            f.OnBattle = (id) => TryBattle(f, id);
            //能力
            List<Ability> abilities = new();
            foreach (var a in card.AbilitiesToPlayer)
            {
                var ab = new Ability(
                    a.Timing,
                    async () =>
                    {
                        if (a.Process != null)//?が使えないため
                            await a.Process.Invoke(this);
                        OnDescription?.Invoke(f.Name, a.Description);
                    },
                    a.Description
                );
                abilities.Add(ab);
            }
            foreach (var a in card.AbilitiesToFollower)
            {
                var ab = new Ability(
                    a.Timing,
                    async () =>
                    {
                        if (a.Process != null)
                            await a.Process.Invoke(f);
                        OnDescription?.Invoke(f.Name, a.Description);
                    },
                    a.Description
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
            t.OnSelect = () => OnSelectable?.Invoke(t);
            //damage
            t.GetEnemyFollower = (initid) => TryTakeDamge(initid, t.IsBlocker.CurrentValue, t.Name);
            //能力
            List<Ability> abilities = new();
            foreach (var a in card.AbilitiesToPlayer)
            {
                var ab = new Ability(
                    a.Timing,
                    async () =>
                    {
                        if (a.Process != null)
                            await a.Process.Invoke(this);
                        OnDescription?.Invoke(t.Name, a.Description);
                    },
                    a.Description
                );
                abilities.Add(ab);
            }
            foreach (var a in card.AbilitiesToTrap)
            {
                var ab = new Ability(
                    a.Timing,
                    async () =>
                    {
                        if (a.Process != null)
                            await a.Process.Invoke(t);
                        OnDescription?.Invoke(t.Name, a.Description);
                    },
                    a.Description
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
