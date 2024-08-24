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

        public Player()
        {
            _deck = new();
            Hand = new();
        }

        public async UniTask CreateDeck()
        {
            _deck = new();
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
            Debug.Log("startturn");
            Drow();
        }

        private void Drow()
        {
            if (_deck.Count <= 0)
            {
                Debug.Log("deck out");
            }
            var c = _deck[_deck.Count - 1];
            _deck.RemoveAt(_deck.Count - 1);
            Hand.Add(c);
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

        }
    }
}
