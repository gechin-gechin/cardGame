using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject resultPanel;
    [SerializeField] Text resultText;

    [SerializeField] Text timeCountText;

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
