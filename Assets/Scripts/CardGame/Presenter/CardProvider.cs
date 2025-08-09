using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;

namespace CardGame
{
    public interface ICardProvider
    {
        CardView Get(Card card);
    }
    public class CardProvider : ObjectPool<CardView>, ICardProvider
    {
        private CardPresenter _cardPresenter;
        private CompositeDisposable _disposables;
        protected override void AltInit()
        {
            _disposables = new();
            _cardPresenter = new();
            _cardPresenter.AddTo(_disposables);
        }
        public CardView Get(Card card)
        {
            var v = GetPooledObject();
            _cardPresenter.Bind(card, v);
            return v;
        }

        private void OnDestroy()
        {
            _disposables.Dispose();
        }
    }
}
