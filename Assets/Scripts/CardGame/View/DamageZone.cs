using System;
using UnityEngine;

namespace CardGame
{
    public class DamageZone : BaseDropZone
    {
        public Action<int> TakeDamage;
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
                    TakeDamage?.Invoke(a.Power);
                    a.OnEndAttack?.Invoke(false);//死んでないからfalse
                }
            }
        }
    }
}
