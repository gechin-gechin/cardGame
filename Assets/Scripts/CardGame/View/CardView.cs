using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CardGame
{
    public interface ICardView
    {
        Action OnRelease { get; set; }
        Func<UniTask<bool>> TryUse { get; set; }
        void Init(string name, int cost, Sprite sprite);
        void SetPower(int value);
    }

    public class CardView : PooledObject<CardView>, ICardView
    {
        public Action OnRelease { get; set; }
        public Func<UniTask<bool>> TryUse { get; set; }
        [SerializeField] CardMovement _movement;
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _costText;
        [SerializeField] private TMP_Text _powerText;
        [SerializeField] private Image _image;
        private bool _isEndDragWorking = false;

        //これ自体のtransformの初期化はplayerviewがやっている
        public void Init(string name, int cost, Sprite sprite)
        {
            _nameText.text = name;
            _costText.text = cost.ToString();
            _image.sprite = sprite;

            _movement.Init();
            _movement.IsDraggable = true;
            _movement.OnEnd += () => _ = EndDrag();
            _movement.transform.localPosition = Vector3.zero;
            _movement.transform.localScale = Vector3.one;
        }

        public void SetPower(int value)
        {
            _powerText.text = value.ToString();
        }


        public void Release()
        {
            OnRelease?.Invoke();
            ReleaseToPool();
            TryUse = null;
            OnRelease = null;
        }

        //購読の破棄
        private void OnDestroy()
        {
            OnRelease?.Invoke();
        }

        private async UniTask EndDrag()
        {
            //何度も呼ばれる時があったため
            if (_isEndDragWorking)
            {
                return;
            }
            _isEndDragWorking = true;
            if (_movement.transform.localPosition.y > 100 && TryUse != null)
            {
                var b = await TryUse.Invoke();
                if (b)
                {
                    Release();
                }
                else
                {
                    _movement.transform.DOLocalMove(Vector3.zero, 0.15f);
                }
            }
            else
            {
                _movement.transform.DOLocalMove(Vector3.zero, 0.15f);
            }
            _isEndDragWorking = false;
        }
    }
}
