using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using R3;

namespace CardGame
{
    public class TrapPresenter : IDisposable
    {
        private CompositeDisposable _disposables;
        public TrapPresenter()
        {
            _disposables = new();
        }

        public void Bind(Trap model, ITrapView view)
        {
            view.Init(model.PlayerID, model.Name, model.Sprite_);
            model.Life.Subscribe(p => view.SetLife(p)).AddTo(_disposables);
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
