using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardGame
{
    public interface IAttackAble
    {
        Action<bool> OnEndAttack { get; set; }//isdead
        int PlayerID { get; }
        int Power { get; }
        bool IsAttackAble { get; }
    }
    public class AttackZone : MonoBehaviour, IAttackAble
    {
        public Action<bool> OnEndAttack { get; set; }
        public int PlayerID { get; private set; }
        public int Power { get; private set; }
        public bool IsAttackAble { get; private set; }

        public void SetPlayerID(int id)
        {
            PlayerID = id;
        }

        public void SetPower(int power)
        {
            Power = power;
        }
        //Follower生成したターンもDropできてしまうためここで防ぐ、おそらくR３のバグ
        public void SetIsAttackAble(bool value)
        {
            IsAttackAble = value;
        }
    }
}
