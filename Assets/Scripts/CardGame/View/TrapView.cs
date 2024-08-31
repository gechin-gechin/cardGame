using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CardGame
{
    public interface ITrapView
    {
        void Init(string name, Sprite sprite);
        void SetLife(int num);
    }
    public class TrapView : PooledObject<TrapView>, ITrapView
    {

        [SerializeField] private Image _image;
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _lifeText;

        public void Init(string name, Sprite sprite)
        {
            _nameText.text = name;
            _image.sprite = sprite;
        }

        public void SetLife(int num)
        {
            _lifeText.text = num.ToString();
        }
        //破壊時にはPooledObjectのRelease()を呼ぶ
    }
}
