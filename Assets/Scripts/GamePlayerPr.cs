using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using R3;

public class GamePlayerPr : MonoBehaviour
{
    [SerializeField] private GamePlayerManager manager;
    [SerializeField] private GamePlayerUI UI;

    void Start()
    {
        manager.HP.Subscribe(hp => {
            UI.HPText.text=hp.ToString();
            if (hp <= 0)
            {
                GameManager.I.ShowResultPanel();
            }
        }).AddTo(this);
        manager.ManaCost.Subscribe(cost => UI.ManaCostText.text = cost.ToString());
    }
}
