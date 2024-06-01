using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject resultPanel;
    [SerializeField] Text resultText;

    [SerializeField] Text playerHeroHpText;
    [SerializeField] Text EnemyHeroHpText;

    [SerializeField] Text playerManaCostText;
    [SerializeField] Text enemyManaCostText;

    [SerializeField] Text timeCountText;

    public void HideResultPanel()
    {
        resultPanel.SetActive(false);
    }
    public void UpdateTime(int timeCount)
    {
        timeCountText.text = timeCount.ToString();
    }

    public void ShowPlayerHp(int hp)
    {
        playerHeroHpText.text = hp.ToString();
    }

    public void ShowEnemyHp(int hp)
    {
        EnemyHeroHpText.text = hp.ToString();
    }

    public void ShowManaCost(int playerManaCost, int enemyManaCost)
    {
        playerManaCostText.text = playerManaCost.ToString();
        enemyManaCostText.text = enemyManaCost.ToString();
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
