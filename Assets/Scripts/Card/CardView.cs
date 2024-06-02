using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using R3;

public class CardView : MonoBehaviour
{
    [SerializeField] private Text nameText;
    [SerializeField] private Text hpText;
    [SerializeField] private Text atText;
    [SerializeField] private Text costText;
    [SerializeField] private Image iconImage;
    [SerializeField] private GameObject selectablePanale;
    [SerializeField] private GameObject shieldPanel;
    [SerializeField] private GameObject  UraPanel;


    public void SetCard(CardModel cardModel)
    {
        nameText.text = cardModel.name;
        cardModel.HP.Subscribe(hp => hpText.text = hp.ToString()).AddTo(this);
        cardModel.At.Subscribe(at => atText.text = at.ToString()).AddTo(this);
        cardModel.canAttack.Subscribe(flag => selectablePanale.SetActive(flag)).AddTo(this);
        costText.text = cardModel.cost.ToString();
        iconImage.sprite = cardModel.icon;
        UraPanel.SetActive(!cardModel.isPlayerCard);

        if (cardModel.ability == ABILITY.SHIELD)
        {
            shieldPanel.SetActive(true);
        }
        else
        {
            shieldPanel.SetActive(false);
        }
        if (cardModel.spell != SPELL.NONE)
        {
            hpText.gameObject.SetActive(false);
            atText.gameObject.SetActive(false);
        }
    }

    public void Show()
    {
        UraPanel.SetActive(false);
    }
}
