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
        public Action<string> OnMessage { get; set; }
    }

    public sealed partial class Player : IPlayer, IDisposable
    {
        public Action<string> OnMessage { get; set; }//いずれはenumで指定
        public int PlayerID { get; private set; }
        public Player Enemy { get; private set; }
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
        private int _initID = 0;
        private int GetInitID()
        {
            _initID++;
            return _initID;
        }

        public Player(int id)
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

            _cardRepository = new();
            _leaderRepository = new();
        }
        public void SetEnemy(Player enemy)
        {
            Enemy = enemy;
        }
        public async UniTask CreateLeader()
        {
            int id = 0;
            var l = await _leaderRepository.GetByID(id, PlayerID);
            l.GetEnemyFollower = (initid) => TryTakeDamge(initid, false);
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
            OnMessage?.Invoke("turn changed");
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
                        //CIPとcommon
                        var absf = f.Abilities.Where(a => a.Timing == AbilityTiming.CIP || a.Timing == AbilityTiming.Common).ToArray();
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
                        //CIPとcommon
                        var abst = t.Abilities.Where(a => a.Timing == AbilityTiming.CIP || a.Timing == AbilityTiming.Common).ToArray();
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

        //battle
        public Follower GetFieldFollower(int initID)
        {
            return Field.Where(f => f.InitID == initID).FirstOrDefault();
        }

        private void TryBattle(Follower myFollower, int enemyInitID)
        {
            var enemyFollower = Enemy.GetFieldFollower(enemyInitID);
            FollowerBattle(myFollower, enemyFollower);
            //シールドカードの有無を調べる
            //そうじゃない場合enemyfollowerを再度攻撃可能にする
        }

        //こちらは攻撃されている、isblockerは攻撃されているものがブロッカーかどうか
        private Follower TryTakeDamge(int enemyInitID, bool isBlocker)
        {
            var enemyFollower = Enemy.GetFieldFollower(enemyInitID);
            //自分がブロッカー以外
            if (!isBlocker)
            {
                //他にブロッカーがいるか
                if (IsHasBlocker())
                {
                    enemyFollower.SetIsAttackAble(true);
                    OnMessage?.Invoke("exist blocker");
                    return null;
                }
            }
            return enemyFollower;
        }
        private bool IsHasBlocker()
        {
            var fs = Field.Where(f => f.IsBlocker.CurrentValue).FirstOrDefault();
            var ts = TrapZone.Where(t => t.IsBlocker.CurrentValue).FirstOrDefault();
            if (fs != null || ts != null)
            {
                return true;
            }
            return false;
        }

        private void FollowerBattle(Follower myFollower, Follower enemyFollower)
        {
            if (myFollower.Power.CurrentValue > enemyFollower.Power.CurrentValue)
            {
                enemyFollower.Dead();
            }
            else if (myFollower.Power.CurrentValue < enemyFollower.Power.CurrentValue)
            {
                myFollower.Dead();
            }
            else
            {
                myFollower.Dead();
                enemyFollower.Dead();
            }
        }
    }
}
