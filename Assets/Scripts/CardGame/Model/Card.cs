using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardGame
{
    public class Card
    {
        public int ID { get; private set; }
        public string Name { get; private set; }
        public int Cost { get; private set; }

        public Card(int id, string name, int cost)
        {
            ID = id;
            Name = name;
            Cost = cost;
        }
    }
}
