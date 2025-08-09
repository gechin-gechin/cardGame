using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CardGame
{
    public interface ILeaderView
    {
        Action<int> OnTakeDamage { get; set; }
        void Init(int playerID, Sprite sprite, CardCol[] cardCols);
        void SetName(string _name);
        void SetLife(int num);
        void SetLevel(int num);
        void SetExp(int num);
        void SetRequireExp(int num);//先がない場合は-1
    }

    public class LeaderView : MonoBehaviour, ILeaderView
    {
        public Action<int> OnTakeDamage { get; set; }
        [SerializeField] private TMP_Text _name_text;
        [SerializeField] private TMP_Text _life_text;
        [SerializeField] private TMP_Text _level_text;
        [SerializeField] private Image _chara_image;
        [SerializeField] private Image _base_img;
        [SerializeField] private Slider _exp_slider;
        [SerializeField] private DamageZone _damageZone;
        private int _exp;
        private int _requireExp;

        public void Init(int playerID, Sprite sprite, CardCol[] cardCols)
        {
            _exp = 0;
            _requireExp = 0;
            _chara_image.sprite = sprite;
            _damageZone.Init(playerID);
            _damageZone.TakeDamage = (n) => OnTakeDamage?.Invoke(n);
        }
        public void SetName(string _name)
        {
            _name_text.text = _name;
        }

        public void SetLife(int num)
        {
            _life_text.text = num.ToString();
        }

        public void SetLevel(int num)
        {
            _level_text.text = num.ToString();
        }

        public void SetExp(int num)
        {
            _exp = num;
            SetExpSlider();
        }

        public void SetRequireExp(int num)
        {
            _requireExp = num;
            SetExpSlider();
        }

        private void SetExpSlider()
        {
            if (_requireExp < 0)
            {
                _exp_slider.value = 1;
                return;
            }
            float v = (float)_exp / (float)_requireExp;
            _exp_slider.value = v;
            /*
            DOTween.To(() => _exp_slider.value,
                (n) => _exp_slider.value = n,
                v,
                0.2f
            );
            */
        }
    }
}
