using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CardGame
{
    public interface ITrapView
    {
        Action<int> OnTakeDamage { get; set; }
        Action OnRelease { get; set; }
        void Init(int playerID, string name, Sprite sprite);
        void SetLife(int num);
        void Release();
    }
    public class TrapView : PooledObject<TrapView>, ITrapView
    {
        public Action<int> OnTakeDamage { get; set; }
        public Action OnRelease { get; set; }
        [SerializeField] private Image _image;
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _lifeText;
        [SerializeField] private DamageZone _damageZone;

        public void Init(int playerID, string name, Sprite sprite)
        {
            _nameText.text = name;
            _image.sprite = sprite;
            _damageZone.Init(playerID);
            _damageZone.TakeDamage = (n) => OnTakeDamage?.Invoke(n);
        }

        public void SetLife(int num)
        {
            _lifeText.text = num.ToString();
        }
        //破壊時にはPooledObjectのRelease()を呼ぶ

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
