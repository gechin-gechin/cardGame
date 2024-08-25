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

        public Player()
        {
            _deck = new();
            Hand = new();
            Field = new();
            _mana = new(0);
            _maxMana = new(0);
        }

        public async UniTask CreateDeck()
        {
            var cardRepository = new CardRepository();
            //int[] _decklist = new int[40];
            int[] _decklist = { 0, 0, 0, 0, 1, 1, 1, 2, 2, 0, 1, 0, 2 };
            foreach (int id in _decklist)
            {
                var c = await cardRepository.Get(id);
                Debug.Log("deck add " + c.Name);
                _deck.Add(c);
            }
            _deck = _deck.OrderBy(c => Guid.NewGuid()).ToList();
        }

        public void StartTurn()
        {
            _maxMana.Value++;
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
