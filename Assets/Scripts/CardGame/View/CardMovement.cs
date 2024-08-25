using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using R3;
using R3.Triggers;
using UnityEngine.EventSystems;
using System;

namespace CardGame
{
    //ドラッグ機能のみ
    [RequireComponent(typeof(ObservableEventTrigger))]
    [RequireComponent(typeof(CanvasGroup))]
    public class CardMovement : MonoBehaviour
    {
        public Action OnBegin;
        public Action<PointerEventData> OnDrag;
        public Action OnEnd;

        private ObservableEventTrigger _trigger;
        private CanvasGroup _canvasGroup;
        private Camera _camera;

        public bool IsDraggable { get; set; }
        public void Init()
        {
            _camera = Camera.main;
            _trigger = GetComponent<ObservableEventTrigger>();
            _canvasGroup = GetComponent<CanvasGroup>();
            //ドラッグ開始
            _trigger.OnBeginDragAsObservable()
                .Where(_ => IsDraggable)
                .Subscribe(_ => BeginDrag())
                .AddTo(this);
            //ドラッグ中
            _trigger.OnDragAsObservable()
                .Where(_ => IsDraggable)
                .Subscribe(e => Drag(e))
                .AddTo(this);
            //ドラッグ終わり
            _trigger.OnEndDragAsObservable()
                .Where(_ => IsDraggable)
                .Subscribe(_ => EndDrag())
                .AddTo(this);
        }

        private void BeginDrag()
        {
            _canvasGroup.blocksRaycasts = false;
            Debug.Log("begin drag");
            OnBegin?.Invoke();
        }

        private void Drag(PointerEventData e)
        {
            Vector3 pos = _camera.ScreenToWorldPoint(e.position);
            pos.z = 0;
            transform.position = pos;
            OnDrag?.Invoke(e);
        }

        private void EndDrag()
        {
            _canvasGroup.blocksRaycasts = true;
            OnEnd?.Invoke();
        }
    }
}
