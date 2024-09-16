using System;
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
        Action OnRelease { get; set; }
        Action OnEndAttack { get; set; }
        Action<int> OnBattle { get; set; }
        Action OnSelect { get; set; }
        void Init(int playerID, int initID, string name, Sprite sprite);
        void SetPower(int num);
        void SetIsAttackAble(bool value);
        void SetIsBlocker(bool value);
        void SetSelectable(bool value);
        void Release();
    }
    public class FollowerView : PooledObject<FollowerView>, IFollowerView
    {
        public Action OnRelease { get; set; }
        public Action OnEndAttack { get; set; }
        public Action<int> OnBattle { get; set; }
        public Action OnSelect { get; set; }
        [SerializeField] private Image _image;
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _powerText;
        [Header("yajirushi")]
        [SerializeField] private CardMovement _cardMovement;
        [SerializeField] private LineRenderer _lineRenderer;
        [SerializeField] private Image _sentan_img;
        [Header("status")]
        [SerializeField] private Outline _outline;
        [SerializeField] private Image _blockerSign;
        [SerializeField] private Button _selectButton;
        [Header("battle")]
        [SerializeField] private AttackZone _attackZone;
        [SerializeField] private BattleZone _battleZone;

        //一度しか呼ばれたくない
        private void Awake()
        {
            _attackZone.OnEndAttack = () =>
            {
                OnEndAttack?.Invoke();
                //Dropが成功するとEndが呼ばれないため
                _cardMovement.transform.localPosition = Vector3.zero;
                _cardMovement.SetBlocksRaycasts(true);
                _lineRenderer.enabled = false;
                _sentan_img.color = Vector4.zero;
            };
            _battleZone.OnBattle = (id) => OnBattle?.Invoke(id);
            _cardMovement.Init();
            Observable.EveryValueChanged(_cardMovement.transform, t => t.localPosition)
                //.Where(_ => _cardMovement.IsDraggable)
                //.Where(_ => _lineRenderer.enabled)
                .Subscribe(pos => _lineRenderer.SetPosition(1, pos))
                .AddTo(this);
            _cardMovement.OnBegin += () =>
            {
                _lineRenderer.enabled = true;
                _sentan_img.color = Vector4.one;
            };
            _cardMovement.OnEnd += () =>
            {
                _cardMovement.transform.localPosition = Vector3.zero;
                _lineRenderer.enabled = false;
                _sentan_img.color = Vector4.zero;
            };
            _selectButton.onClick.AddListener(() => OnSelect?.Invoke());
        }

        public void Init(int playerID, int initID, string name, Sprite sprite)
        {
            _nameText.text = name;
            _image.sprite = sprite;
            _lineRenderer.enabled = false;
            _sentan_img.color = Vector4.zero;

            _attackZone.SetIDs(playerID, initID);
            _battleZone.Init(playerID);
            _cardMovement.IsDraggable = false;
        }

        public void SetPower(int num)
        {
            _powerText.text = num.ToString();
        }

        public void SetIsAttackAble(bool value)
        {
            _cardMovement.IsDraggable = value;
            //見た目でわかるもの
            _outline.enabled = value;
            _attackZone.SetIsAttackAble(value);
        }

        public void SetIsBlocker(bool value)
        {
            _blockerSign.enabled = value;
        }

        public void SetSelectable(bool value)
        {
            _selectButton.gameObject.SetActive(value);
        }

        public void Release()
        {
            OnRelease?.Invoke();
            ReleaseToPool();
            OnRelease = null;
        }

        //購読の破棄
        private void OnDestroy()
        {
            OnRelease?.Invoke();
        }
    }
}
