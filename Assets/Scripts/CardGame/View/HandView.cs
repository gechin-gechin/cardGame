using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CardGame
{
    public class HandView : MonoBehaviour
    {
        [SerializeField] private Button _close_button;
        [SerializeField] private Button _open_button;
        [SerializeField] private Transform _target;
        [SerializeField] private Vector3 _open_localPos;
        [SerializeField] private Vector3 _close_localPos;
        [SerializeField] private Vector3 _open_localScale = Vector3.one;
        [SerializeField] private Vector3 _close_localScale = Vector3.one;
        [SerializeField] private bool _isOpenNow;
        [SerializeField] private float _moveSpeed;

        private bool _isMoving;
        private bool _isOpen;

        private void Awake()
        {
            _open_button.onClick.AddListener(() => Open());
            _close_button.onClick.AddListener(() => Close());
            _isOpen = _isOpenNow;
            _isMoving = false;
        }

        private void Open()
        {
            if (_isMoving || _isOpen)
            {
                return;
            }
            _isMoving = true;
            _target.transform.DOLocalMove(_open_localPos, _moveSpeed);
            _target.transform.DOScale(_open_localScale, _moveSpeed).OnComplete(() =>
            {
                _isMoving = false;
                _isOpen = true;
            });
        }

        private void Close()
        {
            if (_isMoving || !_isOpen)
            {
                return;
            }
            _isMoving = true;
            _target.transform.DOLocalMove(_close_localPos, _moveSpeed);
            _target.transform.DOScale(_close_localScale, _moveSpeed).OnComplete(() =>
            {
                _isMoving = false;
                _isOpen = false;
            });
        }
    }
}
