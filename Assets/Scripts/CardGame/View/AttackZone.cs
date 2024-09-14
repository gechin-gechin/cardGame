using System;
using UnityEngine;

namespace CardGame
{
    public interface IAttackAble
    {
        Action OnEndAttack { get; set; }//isdead
        int PlayerID { get; }
        int InitID { get; }
        bool IsAttackAble { get; }
    }
    public class AttackZone : MonoBehaviour, IAttackAble
    {
        public Action OnEndAttack { get; set; }
        public int PlayerID { get; private set; }
        public int InitID { get; private set; }
        public bool IsAttackAble { get; private set; }

        public void SetIDs(int playerID, int initID)
        {
            PlayerID = playerID;
            InitID = initID;
            Debug.Log(initID);
        }

        //Follower生成したターンもDropできてしまうためここで防ぐ、おそらくR３のバグ
        public void SetIsAttackAble(bool value)
        {
            IsAttackAble = value;
        }
    }
}
