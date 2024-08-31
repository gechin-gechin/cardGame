using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardGame
{
    public class LeaderTranslator : MonoBehaviour
    {
        public Leader EntityToLeader(LeaderEntity entity)
        {
            return new Leader(
                entity.Name,
                entity.Chara_sprite,
                entity.Stages,
                entity.Colors
            );
        }
    }
}
