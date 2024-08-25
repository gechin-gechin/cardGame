using System;
using R3;
using ObservableCollections;

namespace CardGame
{
    public class PlayerPresenter : IDisposable
    {
        private CompositeDisposable _disposables;
        private ICardProvider _cardProvider;
        private IFollowerProvider _followerProvider;

        public PlayerPresenter(ICardProvider cardProvider, IFollowerProvider followerProvider)
        {
            _disposables = new();
            _cardProvider = cardProvider;
            _followerProvider = followerProvider;
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

            model.Field.ObserveAdd().Subscribe(f =>
            {
                if (f == null)
                {
                    return;
                }
                var fv = _followerProvider.Get(f.Value);
                view.SummonFollower(fv);
            }).AddTo(_disposables);
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
