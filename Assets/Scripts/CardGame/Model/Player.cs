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
        public Action OnTurnEnd { get; set; }
        private List<Card> _deck;
        public ObservableList<Card> Hand;
        public ObservableList<Follower> Field;
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

        public Player(CardRepository cardRepository, LeaderRepository leaderRepository)
        {
            _deck = new();
            Hand = new();
            Field = new();
            _mana = new(0);
            _maxMana = new(0);

            _disposables = new();

            _cardRepository = cardRepository;
            _leaderRepository = leaderRepository;
        }
        public async UniTask CreateLeader()
        {
            int id = 0;
            var l = await _leaderRepository.GetByID(id);
            _leader = new(l);
            _leader.Value.MaxCost.Subscribe(mc => _maxMana.Value = mc).AddTo(_disposables);
        }

        public async UniTask CreateDeck()
        {
            //int[] _decklist = new int[40];
            int[] _decklist = { 0, 0, 0, 0, 1, 1, 1, 2, 2, 0, 1, 0, 2 };
            foreach (int id in _decklist)
            {
                var c = await _cardRepository.GetByID(id);
                Debug.Log("deck add " + c.Name);
                _deck.Add(c);
            }
            _deck = _deck.OrderBy(c => Guid.NewGuid()).ToList();
        }

        public void StartTurn()
        {
            _mana.Value = _maxMana.Value;
            Drow();
        }

        private void Drow()
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
                _mana.Value -= card.Cost;
                if (card.Kind == CardKind.FOLLOWER)
                {
                    var f = CardToFollower(card);
                    Field.Add(f);
                }
                Hand.Remove(card);
                return true;
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

        private Follower CardToFollower(Card card)
        {
            return new Follower(
                card.Name,
                card.Power.CurrentValue,
                card.Sprite_
            );
        }
    }
}
