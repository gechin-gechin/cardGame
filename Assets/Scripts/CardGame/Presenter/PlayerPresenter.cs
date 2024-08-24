using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardGame
{
    public class PlayerPresenter : IDisposable
    {
        public void Bind(Player model, IPlayerView view)
        {
            view.OnTurnEnd += model.TurnEnd;
        }

        public void Dispose()
        {

        }
    }
}
