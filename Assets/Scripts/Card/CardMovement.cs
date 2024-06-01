using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class CardMovement : MonoBehaviour,IDragHandler,IBeginDragHandler,IEndDragHandler
{
    //kimarimonku
    public Transform defaultParent;

    public bool isDraggable;

    private void Start()
    {
        defaultParent = transform.parent;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //カードのコストとPlayerのManaコストを比較
        CardController card = GetComponent<CardController>();
        if (card.model.isPlayerCard && GameManager.I.isplayerTurn && !card.model.isFieldCard && card.model.cost <= GameManager.I.Player.ManaCost.CurrentValue)
        {
            isDraggable = true;
        }
        else if (card.model.isPlayerCard && GameManager.I.isplayerTurn && card.model.isFieldCard && card.model.canAttack.Value)
        {
            isDraggable = true;
        }
        else
        {
            isDraggable = false;
        }
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
    public void OnDrag(PointerEventData eventData)
    {
        if (!isDraggable)
        {
            return;
        }
        Vector3 TargetPos = Camera.main.ScreenToWorldPoint(eventData.position);
        TargetPos.z = 0;
        transform.position = TargetPos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isDraggable)
        {
            return;
        }
        transform.SetParent(defaultParent, false);
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

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
