using System.Collections;
using System.Collections.Generic;
using R3;
using UnityEngine;
namespace CardGame
{
    public interface IFollowerProvider
    {
        FollowerView Get(Follower follower);
    }

    public class FollowerProvider : ObjectPool<FollowerView>, IFollowerProvider
    {
        private FollowerPresenter _presenter;
        private CompositeDisposable _disposables;

        protected override void AltInit()
        {
            _presenter = new();
            _disposables = new();
            _presenter.AddTo(_disposables);
        }

        public FollowerView Get(Follower follower)
        {
            var view = GetPooledObject();
            _presenter.Bind(follower, view);
            return view;
        }

        private void OnDestroy()
        {
            _disposables.Dispose();
        }
    }
}
