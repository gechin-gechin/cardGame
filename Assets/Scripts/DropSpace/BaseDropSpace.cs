using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using R3;
using R3.Triggers;

[RequireComponent(typeof(ObservableDropTrigger))]

abstract public class BaseDropSpace : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<ObservableDropTrigger>().OnDropAsObservable()
            .Subscribe(e => OnDrop(e))
            .AddTo(this);
    }

    private void OnDrop(PointerEventData e)
    {
        CardController card = e.pointerDrag.GetComponent<CardController>();
        if(card != null)
        {
            process(card);
        }
    }

    protected virtual void process(CardController card)
    {

    }
}
