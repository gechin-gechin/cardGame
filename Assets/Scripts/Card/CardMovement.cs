using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using R3;
using R3.Triggers;
[RequireComponent(typeof(ObservableEventTrigger))]

public class CardMovement : MonoBehaviour
{
    private CardModel model;
    private ObservableEventTrigger trigger = null;

    public Transform defaultParent { get; set; }
    public bool isDraggable { get; set; }


    public void Init(CardModel _model)
    {
        //初期化
        model = _model;
        defaultParent = transform.parent;
        trigger = GetComponent<ObservableEventTrigger>();
        //ドラッグ開始
        trigger.OnBeginDragAsObservable()
            .Do(_ => isDraggable = ResetIsDraggable())
            .Where(_=> isDraggable)
            .Subscribe(_ => OnBeginDrag()).AddTo(this);
        //ドラッグ中
        trigger.OnDragAsObservable()
            .Where(_ => isDraggable)
            .Subscribe(e => OnDrag(e)).AddTo(this);
        //ドラッグ終わり
        trigger.OnEndDragAsObservable()
            .Where(_ => isDraggable)
            .Subscribe(_ => OnEndDrag()).AddTo(this);
    }

    private bool ResetIsDraggable()
    {
        //カードのコストとPlayerのManaコストを比較
        bool shareFrag = model.isPlayerCard && GameManager.I.isplayerTurn;
        if (!model.isFieldCard && shareFrag && model.cost <= GameManager.I.Player.ManaCost.CurrentValue)
        {
            return true;
        }
        else if (model.isFieldCard && shareFrag && model.canAttack.Value)
        {
            return true;
        }
        return false;
    }

    private void OnBeginDrag()
    {
        if (!isDraggable)
        {
            return;
        }
        defaultParent = transform.parent;
        //to smoozy
        transform.SetParent(defaultParent.parent, false);
        //Raycast taisaku
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    //camera yoh
    private void OnDrag(PointerEventData e)
    {
        Vector3 TargetPos = Camera.main.ScreenToWorldPoint(e.position);
        TargetPos.z = 0;
        transform.position = TargetPos;
    }

    private void OnEndDrag()
    {
        transform.SetParent(defaultParent, false);
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }


    //AI
    public IEnumerator MoveToField(Transform field)
    {
        //一度親をCanvasに変更する
        transform.SetParent(defaultParent.parent);
        //DOTweenでカードをフィールドに移動
        transform.DOMove(field.position, 0.25f);
        yield return new WaitForSeconds(0.25f);
        defaultParent = field;
        transform.SetParent(defaultParent);
    }
    public IEnumerator MoveToTarget(Transform target)
    {
        //現在の位置を取得
        Vector3 currentPosition = transform.position;
        int siblingIndex = transform.GetSiblingIndex();//子要素で何番目か

        //一度親をCanvasに変更する
        transform.SetParent(defaultParent.parent);
        //DOTweenでカードをフィールドに移動
        transform.DOMove(target.position, 0.25f);

        //元に戻る
        yield return new WaitForSeconds(0.25f);
        transform.DOMove(currentPosition, 0.1f);
        yield return new WaitForSeconds(0.25f);
        if (this!=null)
        {
            transform.SetParent(defaultParent);
            transform.SetSiblingIndex(siblingIndex);
        }
        
    }
}
