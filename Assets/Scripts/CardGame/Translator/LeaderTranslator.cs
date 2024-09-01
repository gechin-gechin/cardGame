using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardGame
{
    public class LeaderTranslator : MonoBehaviour
    {
        public Leader EntityToLeader(int playerID, LeaderEntity entity)
        {
            return new Leader(
                playerID,
                entity.Name,
                entity.Chara_sprite,
                entity.Stages,
                entity.Colors
            );
        }
    }
}
