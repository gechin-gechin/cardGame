using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardGame
{
    [CreateAssetMenu(fileName = "CardEntity", menuName = "CardEntity", order = 0)]
    public class CardEntity : ScriptableObject
    {
        [SerializeField] private int _id;
        [SerializeField] private string _name;
        [SerializeField] private int _cost;
        [SerializeField] private int _power;
        [SerializeField] private Sprite _sprite;

        public int ID => _id;
        public string Name => _name;
        public int Cost => _cost;
        public int Power => _power;
        public Sprite Sprite_ => _sprite;
    }
}