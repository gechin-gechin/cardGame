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
        [SerializeField] private CardKind _kind;
        [SerializeField] private int _power;
        [SerializeField] private Sprite _sprite;
        [Header("Abilities")]
        [SerializeField] private AbilityEntity<Player>[] _abilitiesToPlayer;
        [SerializeField] private AbilityEntity<Follower>[] _abilitiesToFollower;
        [SerializeField] private AbilityEntity<Trap>[] _abilitiesToTrap;

        public int ID => _id;
        public string Name => _name;
        public int Cost => _cost;
        public CardKind Kind => _kind;
        public int Power => _power;
        public Sprite Sprite_ => _sprite;

        public AbilityEntity<Player>[] AbilitiesToPlayer => _abilitiesToPlayer;
        public AbilityEntity<Follower>[] AbilitiesToFollower => _abilitiesToFollower;
        public AbilityEntity<Trap>[] AbilitiesToTrap => _abilitiesToTrap;
    }
}