using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayerUI : MonoBehaviour
{
    [SerializeField] private Text hpText = null;
    [SerializeField] private Text manaCostText = null;

    public Text HPText { get => hpText; set => hpText = value; }
    public Text ManaCostText { get => manaCostText; set => manaCostText = value; }
}
