using System;
using R3;
using ObservableCollections;
using Cysharp.Threading.Tasks;

namespace CardGame
{
    public class PlayerPresenter : IDisposable
    {
        private CompositeDisposable _disposables;
        private ICardProvider _cardProvider;
        private IFollowerProvider _followerProvider;
        private ITrapProvider _trapProvider;
        private LeaderPresenter _leaderPresenter;

        public PlayerPresenter(
            ICardProvider cardProvider,
            IFollowerProvider followerProvider,
            ITrapProvider trapProvider)
        {
            _disposables = new();
            _cardProvider = cardProvider;
            _followerProvider = followerProvider;
            _trapProvider = trapProvider;
            _leaderPresenter = new();
            _leaderPresenter.AddTo(_disposables);
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

            model.TrapZone.ObserveAdd().Subscribe(t =>
            {
                if (t == null)
                {
                    return;
                }
                var tv = _trapProvider.Get(t.Value);
                view.SetTrap(tv);
            }).AddTo(_disposables);

            model.Leader_.Subscribe(l =>
            {
                _leaderPresenter.Bind(l, view.ILeaderView_);
            }).AddTo(_disposables);
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
