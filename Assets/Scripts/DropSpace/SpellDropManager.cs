using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//攻撃された側が攻撃処理を行う
public class SpellDropManager : BaseDropSpace
{
    protected override void process(CardController card)
    {
        //分岐に使うため、nullも許容？？？
        CardController target = GetComponent<CardController>();
        if (card.CanUseSpell())
        {
            card.UseSpellTo(target);
        }
    }
}