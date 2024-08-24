using System;
using System.Collections;
using System.Collections.Generic;
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
        private ObservableList<Card> _cards;

        public Player()
        {

        }

        public async UniTask CreateDeck()
        {
            _cards = new();
            var cardRepository = new CardRepository();
            //int[] _decklist = new int[40];
            int[] _decklist = { 0, 0, 0, 0, 1, 1, 1, 2, 2, 0, 1, 0, 2 };
            foreach (int id in _decklist)
            {
                var c = await cardRepository.Get(id);
                Debug.Log(c.Name);
                _cards.Add(c);
            }
        }

        public void StartTurn()
        {
            Debug.Log("startturn");
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
