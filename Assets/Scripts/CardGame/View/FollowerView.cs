using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CardGame
{
    public interface IFollowerView
    {
        void Init(string name, Sprite sprite);
        void SetPower(int num);
    }
    public class FollowerView : PooledObject<FollowerView>, IFollowerView
    {
        [SerializeField] private Image _image;
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _powerText;

        public void Init(string name, Sprite sprite)
        {
            _nameText.text = name;
            _image.sprite = sprite;
        }

        public void SetPower(int num)
        {
            _powerText.text = num.ToString();
        }
    }
}
