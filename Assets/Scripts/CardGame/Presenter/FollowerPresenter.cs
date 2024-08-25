using System;
using System.Collections;
using System.Collections.Generic;
using R3;
using UnityEngine;

namespace CardGame
{
    public class FollowerPresenter : IDisposable
    {
        private CompositeDisposable _disposables;
        public FollowerPresenter()
        {
            _disposables = new();
        }

        public void Bind(Follower model, IFollowerView view)
        {
            view.Init(model.Name, model.Sprite_);
            model.Power.Subscribe(p => view.SetPower(p)).AddTo(_disposables);
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
