using System;
using UnityEngine;

namespace CardGame
{
    public class BattleZone : BaseDropZone
    {
        public Action<int> OnBattle;//enemyinitid
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
                    a.OnEndAttack();
                    OnBattle?.Invoke(a.InitID);
                }
            }
        }
    }
}
