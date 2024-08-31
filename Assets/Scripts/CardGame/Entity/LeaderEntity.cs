using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardGame
{
    [CreateAssetMenu(fileName = "LeaderEntity", menuName = "LeaderEntity", order = 0)]
    public class LeaderEntity : ScriptableObject
    {
        [SerializeField] private int _id;
        [SerializeField] private string _name;
        [SerializeField] private Sprite _chara_sprite;
        [SerializeField] private LeaderStage[] _stages = new LeaderStage[3];
        [SerializeField] private CardCol[] _colors;

        public int ID => _id;
        public string Name => _name;
        public Sprite Chara_sprite => _chara_sprite;
        public LeaderStage[] Stages => _stages;
        public CardCol[] Colors => _colors;
    }

    [Serializable]
    public struct LeaderStage
    {
        public int RequireExp;
        public int MaxCost;
    }
}
