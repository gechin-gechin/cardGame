using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using ObservableCollections;
using R3;
using UnityEngine;

namespace CardGame
{
    public interface IPlayer
    {
        void StartTurn();
        void TimeOver();
        Action OnTurnEnd { get; set; }
    }

    public class Player : IPlayer, IDisposable
    {
        public int PlayerID { get; private set; }
        public Action OnTurnEnd { get; set; }
        private List<Card> _deck;
        public ObservableList<Card> Hand;
        public ObservableList<Follower> Field;
        public ObservableList<Trap> TrapZone;
        public ObservableList<Card> Trash;
        private ReactiveProperty<int> _mana;
        public ReadOnlyReactiveProperty<int> Mana => _mana;

        private ReactiveProperty<int> _maxMana;
        public ReadOnlyReactiveProperty<int> MaxMana => _maxMana;
        private ReactiveProperty<Leader> _leader;
        public ReadOnlyReactiveProperty<Leader> Leader_ => _leader;

        //repository
        private CardRepository _cardRepository;
        private LeaderRepository _leaderRepository;

        private CompositeDisposable _disposables;

        public Player(int id, CardRepository cardRepository, LeaderRepository leaderRepository)
        {
            PlayerID = id;
            _deck = new();
            Hand = new();
            Field = new();
            TrapZone = new();
            Trash = new();
            _mana = new(0);
            _maxMana = new(0);

            _disposables = new();

            _cardRepository = cardRepository;
            _leaderRepository = leaderRepository;
        }
        public async UniTask CreateLeader()
        {
            int id = 0;
            var l = await _leaderRepository.GetByID(id, PlayerID);
            _leader = new(l);
            _leader.Value.MaxCost.Subscribe(mc => _maxMana.Value = mc).AddTo(_disposables);
        }

        public async UniTask CreateDeck()
        {
            //int[] _decklist = new int[40];
            int[] _decklist = { 0, 0, 0, 0, 1, 1, 1, 2, 2, 3, 3, 3, 4, 4, 4 };
            foreach (int id in _decklist)
            {
                var c = await _cardRepository.GetByID(id);
                //Debug.Log("deck add " + c.Name);
                _deck.Add(c);
            }
            _deck = _deck.OrderBy(c => Guid.NewGuid()).ToList();
        }

        public void StartTurn()
        {
            _mana.Value = _maxMana.Value;
            foreach (var f in Field)
            {
                f.SetIsAttackAble(true);
            }
            Drow();
        }

        public void Drow()
        {
            if (_deck.Count <= 0)
            {
                Debug.Log("deck out");
            }
            var c = _deck[_deck.Count - 1];
            c.TryUse = () => TryUseHandCard(c);
            _deck.RemoveAt(_deck.Count - 1);
            Hand.Add(c);
        }

        public bool TryUseHandCard(Card card)
        {
            _leader.Value.GetExp(1);
            if (card.Cost <= _mana.Value)
            {
                switch (card.Kind)
                {
                    case CardKind.FOLLOWER:
                        _mana.Value -= card.Cost;
                        var f = CardToFollower(card);
                        Field.Add(f);
                        f.OnDead += () => Field.Remove(f);
                        Hand.Remove(card);
                        //CIP
                        var absf = f.Abilities.Where(a => a.Timing == AbilityTiming.CIP).ToArray();
                        if (absf != null)
                        {
                            foreach (var a in absf)
                            {
                                a.Process?.Invoke();
                            }
                        }
                        return true;
                    case CardKind.SPELL:
                        return false;//未実装
                    case CardKind.TRAP:
                        _mana.Value -= card.Cost;
                        var t = CardToTrap(card);
                        TrapZone.Add(t);
                        t.OnDead += () => TrapZone.Remove(t);
                        Hand.Remove(card);
                        //CIP
                        var abst = t.Abilities.Where(a => a.Timing == AbilityTiming.CIP).ToArray();
                        if (abst != null)
                        {
                            foreach (var a in abst)
                            {
                                a.Process?.Invoke();
                            }
                        }
                        return true;
                    default:
                        break;
                }
            }
            return false;
        }

        public void TimeOver()
        {
            Debug.Log("timeover");
            OnTurnEnd?.Invoke();
        }

        public void TurnEnd()
        {
            OnTurnEnd?.Invoke();
        }

        public void Dispose()
        {
            _mana.Dispose();
            _maxMana.Dispose();
            _leader.Dispose();

            _disposables.Dispose();
        }

        private void AddTrash(Card card)
        {
            Trash.Add(card);
        }

        private Follower CardToFollower(Card card)
        {
            var f = new Follower(
                PlayerID,
                card.Name,
                card.Power.CurrentValue,
                card.Sprite_
            );
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

        private Action UseableAbility()
        {
            Action act = null;
            return act;
        }
    }
}
