using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace CardGame
{
    public interface IPlayerView
    {
        Action OnTurnEnd { get; set; }
        void DrowCard(CardView cardview);
        void SummonFollower(FollowerView followerView);
        void SetHandCount(int count);
        void SetMana(int num);
        void SetMaxMana(int num);
    }
    public class PlayerView : MonoBehaviour, IPlayerView
    {
        public Action OnTurnEnd { get; set; }

        [SerializeField] private Transform _hand;
        [SerializeField] private Transform _field;
        [Header("text")]
        [SerializeField] private TMP_Text _handcountText;
        [SerializeField] private TMP_Text _manaText;
        [SerializeField] private TMP_Text _maxManaText;
        [Header("input")]
        [SerializeField] private InputActionReference _turnend_ref;
        [SerializeField] private Button _turnend_button;

        public void Init()
        {
            //_turnend_ref.action.performed += (e) => OnTurnEnd?.Invoke();
            _turnend_ref.action.performed += (e) => Debug.Log("Enter");
            _turnend_button.onClick.AddListener(() => OnTurnEnd?.Invoke());
        }

        public void DrowCard(CardView card)
        {
            card.transform.SetParent(_hand);
            card.transform.localScale = Vector3.one;
            card.transform.localPosition = Vector3.zero;
        }

        public void SummonFollower(FollowerView follower)
        {
            follower.transform.SetParent(_field);
            follower.transform.localScale = Vector3.one;
            follower.transform.localPosition = Vector3.zero;
        }

        public void SetHandCount(int c)
        {
            _handcountText.text = "Hand : " + c.ToString();
        }

        public void SetMana(int num)
        {
            _manaText.text = num.ToString();
        }
        public void SetMaxMana(int num)
        {
            _maxManaText.text = num.ToString();
        }
    }
}
