using System;
using R3;

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
            CompositeDisposable cd = new();
            view.Init(model.PlayerID, model.Name, model.Sprite_);
            view.OnEndAttack = model.EndAttack;
            view.OnEndBattle = model.EndBattle;
            model.Power.Subscribe(p => view.SetPower(p)).AddTo(cd);
            model.IsAttackAble.Subscribe(f => view.SetIsAttackAble(f)).AddTo(cd);
            view.OnRelease = () => cd.Dispose();
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
