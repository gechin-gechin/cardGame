using System;
using System.Collections;
using System.Collections.Generic;
using R3;
using UnityEngine;

namespace CardGame
{
    public class CardPresenter : IDisposable
    {
        private CompositeDisposable _disposables;
        public CardPresenter()
        {
            _disposables = new();
        }
        public void Bind(Card model, ICardView view)
        {
            CompositeDisposable cd = new();
            view.Init(model.Name, model.Cost, model.Sprite_);
            model.Power.Subscribe(v => view.SetPower(v)).AddTo(cd);
            view.TryUse += () => model.TryUse.Invoke();
            view.OnRelease += () => cd.Dispose();
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
