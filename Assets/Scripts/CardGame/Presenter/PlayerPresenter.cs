using System;
using R3;
using ObservableCollections;

namespace CardGame
{
    public class PlayerPresenter : IDisposable
    {
        private CompositeDisposable _disposables;
        private ICardProvider _cardProvider;

        public PlayerPresenter(ICardProvider cardProvider)
        {
            _disposables = new();
            _cardProvider = cardProvider;
        }

        public void Bind(Player model, IPlayerView view)
        {
            view.OnTurnEnd += model.TurnEnd;
            model.Mana.Subscribe(n => view.SetMana(n)).AddTo(_disposables);
            model.MaxMana.Subscribe(n => view.SetMaxMana(n)).AddTo(_disposables);

            model.Hand.ObserveAdd().Subscribe(c =>
            {
                if (c == null)
                {
                    return;
                }
                var cv = _cardProvider.Get(c.Value);
                view.DrowCard(cv);
            }).AddTo(_disposables);
            model.Hand.ObserveCountChanged()
                .Subscribe(c => view.SetHandCount(c))
                .AddTo(_disposables);
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
