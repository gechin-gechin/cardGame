using System;
using R3;

namespace CardGame
{
    public class TrapPresenter : IDisposable
    {
        private CompositeDisposable _disposables;
        public TrapPresenter()
        {
            _disposables = new();
        }

        public void Bind(Trap model, ITrapView view)
        {
            CompositeDisposable cd = new();
            view.Init(model.PlayerID, model.Name, model.Sprite_);
            model.Life.Subscribe(p => view.SetLife(p)).AddTo(cd);
            model.IsBlocker.Subscribe(f => view.SetIsBlocker(f)).AddTo(cd);
            model.IsSelectable.Subscribe(f => view.SetSelectable(f)).AddTo(cd);
            view.OnTakeDamage = model.TakeDamage;
            view.OnSelect = () => model.OnSelect?.Invoke();
            model.OnDead += () => view?.Release();
            view.OnRelease = () => cd.Dispose();
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
