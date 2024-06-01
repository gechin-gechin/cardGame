using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject resultPanel;
    [SerializeField] private Text resultText;
    [SerializeField] private Text timeCountText;
    [SerializeField] private Button turnEndButton;

    public Button TurnEndButton { get => turnEndButton;private set => turnEndButton = value; }

    public void HideResultPanel()
    {
        resultPanel.SetActive(false);
    }
    public void UpdateTime(int timeCount)
    {
        timeCountText.text = timeCount.ToString();
    }
    public void ShowResultPanel(int heroHP)
    {
        resultPanel.SetActive(true);
        if (heroHP <= 0)
        {
            resultText.text = "LOSE";
        }
        else
        {
            resultText.text = "WIN";
        }
    }

}
