using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CardGame
{
    public interface ICardView
    {
        Action OnRelease { get; set; }
        void Init(string name, int cost, Sprite sprite);
        void SetPower(int value);
    }

    public class CardView : PooledObject<CardView>, ICardView
    {
        public Action OnRelease { get; set; }
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _costText;
        [SerializeField] private TMP_Text _powerText;
        [SerializeField] private Image _image;

        public void Init(string name, int cost, Sprite sprite)
        {
            _nameText.text = name;
            _costText.text = cost.ToString();
            _image.sprite = sprite;
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
    }
}
