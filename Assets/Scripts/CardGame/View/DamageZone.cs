using System;
using UnityEngine;

namespace CardGame
{
    public class DamageZone : BaseDropZone
    {
        public Action<int> TakeDamage;//initid
        private int _playerID;

        public void Init(int playerID)
        {
            _playerID = playerID;
        }

        protected override void process(GameObject obj)
        {
            var a = obj.GetComponent<IAttackAble>();
            if (a != null)
            {
                if (a.PlayerID != _playerID && a.IsAttackAble)
                {
                    TakeDamage?.Invoke(a.InitID);
                    a.OnEndAttack?.Invoke();
                }
            }
        }
    }
}
