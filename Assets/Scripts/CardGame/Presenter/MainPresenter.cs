using System;
using System.Collections;
using System.Collections.Generic;
using R3;

namespace CardGame
{
    public class MainPresenter : IDisposable
    {
        private CompositeDisposable _disposables;

        public MainPresenter()
        {
            _disposables = new();
        }

        public void Bind(Main model, IMainView view)
        {
            model.CountDownTime.Subscribe(n => view.SetCountDownTime(n))
                .AddTo(_disposables);
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
