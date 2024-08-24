using System;
using R3;
using ObservableCollections;

namespace CardGame
{
    public class PlayerPresenter : IDisposable
    {
        private CompositeDisposable _disposables;

        public PlayerPresenter()
        {
            _disposables = new();
        }

        public void Bind(Player model, IPlayerView view)
        {
            view.OnTurnEnd += model.TurnEnd;

            model.Hand.ObserveAdd().Subscribe(c => view.DrowCard(c.Value)).AddTo(_disposables);
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
