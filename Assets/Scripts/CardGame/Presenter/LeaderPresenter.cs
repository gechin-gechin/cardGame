using System;
using System.Collections;
using System.Collections.Generic;
using R3;
using UnityEngine;

namespace CardGame
{
    public class LeaderPresenter : IDisposable
    {
        private CompositeDisposable _disposables;
        public LeaderPresenter()
        {
            _disposables = new();
        }
        public void Bind(Leader model, ILeaderView view)
        {
            view.Init(model.PlayerID, model.Chara_sprite, model.Colors);
            view.OnTakeDamage = model.TakeDamage;
            model.Name.Subscribe(n => view.SetName(n)).AddTo(_disposables);
            model.Life.Subscribe(n => view.SetLife(n)).AddTo(_disposables);
            model.Level.Subscribe(n => view.SetLevel(n)).AddTo(_disposables);
            model.Exp.Subscribe(n => view.SetExp(n)).AddTo(_disposables);
            model.RequireExp.Subscribe(n => view.SetRequireExp(n)).AddTo(_disposables);
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}