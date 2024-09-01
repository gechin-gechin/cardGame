using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardGame
{
    public class CardTranslator
    {
        public Card EntityToCard(CardEntity entity)
        {
            return new Card(
                entity.ID,
                entity.Name,
                entity.Cost,
                entity.Kind,
                entity.Power,
                entity.Sprite_,
                entity.AbilitiesToPlayer,
                entity.AbilitiesToFollower,
                entity.AbilitiesToTrap
            );
        }
    }
}
