using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CardGame
{
    public interface ICardView
    {
        Action OnRelease { get; set; }
        Action OnUse { get; set; }
        void Init(string name, int cost, Sprite sprite);
        void SetPower(int value);
    }

    public class CardView : PooledObject<CardView>, ICardView
    {
        public Action OnRelease { get; set; }
        public Action OnUse { get; set; }
        [SerializeField] CardMovement _movement;
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _costText;
        [SerializeField] private TMP_Text _powerText;
        [SerializeField] private Image _image;

        public void Init(string name, int cost, Sprite sprite)
        {
            _nameText.text = name;
            _costText.text = cost.ToString();
            _image.sprite = sprite;

            _movement.Init();
            _movement.IsDraggable = true;
            _movement.OnEnd += EndDrag;
        }

        public void SetPower(int value)
        {
            _powerText.text = value.ToString();
        }


        public void Release()
        {
            OnRelease?.Invoke();
            ReleaseToPool();
        }

        //購読の破棄
        private void OnDestroy()
        {
            OnRelease?.Invoke();
        }

        private void EndDrag()
        {
            if (_movement.transform.localPosition.y > 500)
            {
                OnUse?.Invoke();
                Release();
            }
            else
            {
                _movement.transform.DOLocalMove(Vector3.zero, 0.15f);
            }
        }
    }
}
