using System;
using UnityEngine;

namespace CardGame
{
    public class BattleZone : BaseDropZone
    {
        public Action<bool> OnEndBattle;//isdead
        private int _playerID;
        private int _power;

        public void Init(int playerID)
        {
            _playerID = playerID;
        }

        public void SetPower(int num)
        {
            _power = num;
        }

        protected override void process(GameObject obj)
        {
            var a = obj.GetComponent<IAttackAble>();
            if (a != null)
            {
                if (a.PlayerID != _playerID && a.IsAttackAble)
                {
                    if (a.Power > _power)
                    {
                        //攻撃側が勝ち
                        OnEndBattle?.Invoke(true);
                        a.OnEndAttack?.Invoke(false);
                    }
                    else
                    {
                        OnEndBattle?.Invoke(false);
                        a.OnEndAttack?.Invoke(true);
                    }
                }
            }
        }
    }
}
