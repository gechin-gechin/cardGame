using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropSpace : MonoBehaviour, IDropHandler
{
    public enum TYPE
    {
        HAND,
        FIELD,
    }
    public TYPE type;

    public void OnDrop(PointerEventData eventData)
    {
        if (type == TYPE.HAND)
        {
            return;
        }

        CardController card = eventData.pointerDrag.GetComponent<CardController>();
        if (card != null)
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
}
