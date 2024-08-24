using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace CardGame
{
    public interface IPlayerView
    {
        Action OnTurnEnd { get; set; }
        void DrowCard(Card card);
    }
    public class PlayerView : MonoBehaviour, IPlayerView
    {
        public Action OnTurnEnd { get; set; }

        [SerializeField] private InputActionReference _turnend_ref;
        [SerializeField] private Button _turnend_button;

        public void Init()
        {
            //_turnend_ref.action.performed += (e) => OnTurnEnd?.Invoke();
            _turnend_ref.action.performed += (e) => Debug.Log("Enter");
            _turnend_button.onClick.AddListener(() => OnTurnEnd?.Invoke());
        }

        public void DrowCard(Card card)
        {
            if (card == null)
            {
                return;
            }
            Debug.Log("drow : " + card.Name);
        }
    }
}
