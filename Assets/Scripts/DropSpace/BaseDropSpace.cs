using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using R3;
using R3.Triggers;

[RequireComponent(typeof(ObservableEventTrigger))]

abstract public class BaseDropSpace : MonoBehaviour
{
    private CardController card = null;

    private void Awake()
    {
        GetComponent<ObservableEventTrigger>().OnDropAsObservable()
            .Subscribe(e => OnDrop(e))
            .AddTo(this);
    }

    private void OnDrop(PointerEventData e)
    {
        card = e.pointerDrag.GetComponent<CardController>();
        if(card != null)
        {
            process(card);
        }
    }

    protected virtual void process(CardController card)
    {

    }

    protected CardController[] GetEnemyCards()
    {
        return GameManager.I.GetFieldCards(!card.model.isPlayerCard);
    }
}
