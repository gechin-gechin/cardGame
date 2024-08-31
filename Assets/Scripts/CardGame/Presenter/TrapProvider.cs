using System.Collections;
using System.Collections.Generic;
using R3;
using UnityEngine;

namespace CardGame
{
    public interface ITrapProvider
    {
        TrapView Get(Trap model);
    }
    public class TrapProvider : ObjectPool<TrapView>, ITrapProvider
    {
        private TrapPresenter _presenter;
        private CompositeDisposable _disposables;

        protected override void AltInit()
        {
            _presenter = new();
            _disposables = new();
            _presenter.AddTo(_disposables);
        }

        public TrapView Get(Trap model)
        {
            var view = GetPooledObject();
            _presenter.Bind(model, view);
            return view;
        }

        private void OnDestroy()
        {
            _disposables.Dispose();
        }
    }
}
