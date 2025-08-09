using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using R3;
using R3.Triggers;

[RequireComponent(typeof(ObservableDropTrigger))]

abstract public class BaseDropZone : MonoBehaviour
{
    [SerializeField] private string[] _tags;

    private void Awake()
    {
        GetComponent<ObservableDropTrigger>().OnDropAsObservable()
            .Subscribe(e => OnDrop(e))
            .AddTo(this);
    }

    private void OnDrop(PointerEventData e)
    {
        var obj = e.pointerDrag;
        foreach (var t in _tags)
        {
            if (obj.CompareTag(t))
            {
                process(obj);
            }
        }
    }

    protected virtual void process(GameObject obj)
    {

    }
}
