using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldDropSpace : BaseDropSpace
{
    protected override void process(CardController card)
    {
        if (!card.movement.isDraggable)//ドラッグできるか
        {
            return;
        }
        if (card.IsSpell)
        {
            return;
        }

        card.movement.defaultParent = this.transform;//親を移動
        if (card.model.isFieldCard)//フィールドカードなのか
        {
            return;
        }
        card.OnField();
    }
}
