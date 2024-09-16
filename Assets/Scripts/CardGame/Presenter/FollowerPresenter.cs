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
            view.Init(model.PlayerID, model.InitID, model.Name, model.Sprite_);
            view.OnEndAttack = model.EndAttack;
            view.OnBattle = (id) => model.OnBattle?.Invoke(id);
            view.OnSelect = () => model.OnSelect?.Invoke();
            model.Power.Subscribe(p => view.SetPower(p)).AddTo(cd);
            model.IsAttackAble.Subscribe(f => view.SetIsAttackAble(f)).AddTo(cd);
            model.IsBlocker.Subscribe(f => view.SetIsBlocker(f)).AddTo(cd);
            model.IsSelectable.Subscribe(f => view.SetSelectable(f)).AddTo(cd);
            model.OnDead += () => view?.Release();
            view.OnRelease = () => cd.Dispose();
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
