using System.Collections;
using System.Collections.Generic;
using R3;
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
        [Header("yajirushi")]
        [SerializeField] private CardMovement _cardMovement;
        [SerializeField] private LineRenderer _lineRenderer;
        [SerializeField] private GameObject _sentan_obj;

        //一度しか呼ばれたくない
        private void Awake()
        {
            Debug.Log("awake");
            _cardMovement.Init();
            _cardMovement.IsDraggable = true;
            Observable.EveryValueChanged(_cardMovement.transform, t => t.localPosition)
                .Subscribe(pos => _lineRenderer.SetPosition(1, pos))
                .AddTo(this);
            _cardMovement.OnBegin += () =>
            {
                _lineRenderer.gameObject.SetActive(true);
                _sentan_obj.SetActive(true);
            };
            _cardMovement.OnEnd += () =>
            {
                _cardMovement.transform.localPosition = Vector3.zero;
                _lineRenderer.gameObject.SetActive(false);
                _sentan_obj.SetActive(false);
            };
        }

        public void Init(string name, Sprite sprite)
        {
            _nameText.text = name;
            _image.sprite = sprite;
            _lineRenderer.gameObject.SetActive(false);
            _sentan_obj.SetActive(false);
        }

        public void SetPower(int num)
        {
            _powerText.text = num.ToString();
        }
    }
}
