using System;
using System.Collections;
using System.Collections.Generic;
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
