using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using ObservableCollections;
using R3;
using UnityEngine;
using UnityEngine.UI;

namespace CardGame
{
    public interface IPlayer
    {
        void StartTurn();
        void TimeOver();
        Action OnTurnEnd { get; set; }
        public Action<string> OnMessage { get; set; }
        public Action<string, string> OnDescription { get; set; }
        public Action<ISelectable> OnSelectable { get; set; }
    }

    public sealed partial class Player : IPlayer, IDisposable
    {
        public Action<string> OnMessage { get; set; }//いずれはenumで指定
        public Action<string, string> OnDescription { get; set; }//name, desc
        public Action<ISelectable> OnSelectable { get; set; }
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

        //tmp
        public ISelectable NowSelectable = null;

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
            l.GetEnemyFollower = (initid) => TryTakeDamge(initid, false, l.Name.CurrentValue);
            _leader = new(l);
            _leader.Value.MaxCost.Subscribe(mc => _maxMana.Value = mc).AddTo(_disposables);
        }

        public async UniTask CreateDeck()
        {
            //int[] _decklist = new int[40];
            int[] _decklist = { 0, 0, 0, 1, 1, 1, 2, 2, 3, 3, 3, 4, 4, 4, 5, 5, 5, 6, 6, 6, 7, 7, 7 };
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

        public async UniTask<bool> TryUseHandCard(Card card)
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
                                await a.Process.Invoke();
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
                                await a.Process.Invoke();
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
            //自分がブロッカー以外
            if (!myFollower.IsBlocker.CurrentValue)
            {
                //他にブロッカーがいるか
                if (IsHasBlocker())
                {
                    enemyFollower.SetIsAttackAble(true);
                    OnMessage?.Invoke("ブロッカーがいるよ！");
                    return;
                }
            }
            _ = FollowerBattle(myFollower, enemyFollower);
        }

        //こちらは攻撃されている、isblockerは攻撃されているものがブロッカーかどうか
        private Follower TryTakeDamge(int enemyInitID, bool isBlocker, string myName)
        {
            var enemyFollower = Enemy.GetFieldFollower(enemyInitID);
            //自分がブロッカー以外
            if (!isBlocker)
            {
                //他にブロッカーがいるか
                if (IsHasBlocker())
                {
                    enemyFollower.SetIsAttackAble(true);
                    OnMessage?.Invoke("ブロッカーがいるよ！");
                    return null;
                }
            }
            OnDescription?.Invoke(enemyFollower.Name, myName + "に攻撃");
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

        private async UniTask FollowerBattle(Follower f1, Follower f2)
        {
            OnDescription?.Invoke("battle", f1.Name + " vs " + f2.Name + " !");
            //バトル時のアビリティ
            f1.BattleFollower = f2;
            f2.BattleFollower = f1;
            var as1 = f1.Abilities.Where(a => a.Timing == AbilityTiming.Battle).ToArray();
            var as2 = f2.Abilities.Where(a => a.Timing == AbilityTiming.Battle).ToArray();
            if (as1 != null)
                foreach (var a in as1)
                    await a.Process.Invoke();
            if (as2 != null)
                foreach (var a in as2)
                    await a.Process.Invoke();
            f1.BattleFollower = null;
            f2.BattleFollower = null;
            //待機
            await UniTask.WaitForSeconds(1f);
            //実際のバトル
            if (f1.Power.CurrentValue > f2.Power.CurrentValue)
            {
                f2.Dead();
            }
            else if (f1.Power.CurrentValue < f2.Power.CurrentValue)
            {
                f1.Dead();
            }
            else
            {
                f1.Dead();
                f2.Dead();
            }
        }

        public void ResetSelectable()
        {
            _ = SetSelectable(CardKind.FOLLOWER, false, (s) => true);
            _ = SetSelectable(CardKind.TRAP, false, (s) => true);
        }
        public int SetSelectable(CardKind cardKind, bool value, Func<ISelectable, bool> func)
        {
            if (cardKind == CardKind.FOLLOWER)
            {
                var fs = Field.Where((f) => func(f));
                foreach (var f in fs)
                {
                    f.SetSelectable(value);
                }
                return fs.ToArray().Length;
            }

            if (cardKind == CardKind.TRAP)
            {
                var ts = TrapZone.Where((t) => func(t));
                foreach (var t in ts)
                {
                    t.SetSelectable(value);
                }
                return ts.ToArray().Length;
            }
            return 0;
        }

        public async UniTask<ISelectable> PickUp(AbilityTarget target, CardKind[] cardKinds, Func<ISelectable, bool> func)
        {
            //入れ物を空に
            NowSelectable = null;
            //選択可能なものをピックアップ
            int setCount = 0;
            switch (target)
            {
                case AbilityTarget.PLAYER:
                    foreach (var k in cardKinds)
                        setCount += SetSelectable(k, true, func);
                    break;
                case AbilityTarget.ENEMY:
                    foreach (var k in cardKinds)
                        setCount += Enemy.SetSelectable(k, true, func);
                    break;
                case AbilityTarget.BOTH:
                    foreach (var k in cardKinds)
                    {
                        setCount += SetSelectable(k, true, func);
                        setCount += Enemy.SetSelectable(k, true, func);
                    }
                    break;
            }
            //選択可能なものがなければnullを返す
            if (setCount < 1)
            {
                return null;
            }
            //入力を待つ
            OnMessage?.Invoke("選んでね");
            await UniTask.WaitUntil(() => NowSelectable != null);
            //選択待機状態をリセット
            Enemy.ResetSelectable();
            ResetSelectable();

            return NowSelectable;
        }
    }
}
