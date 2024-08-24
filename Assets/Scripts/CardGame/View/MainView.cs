using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace CardGame
{
    public interface IMainView
    {
        void SetCountDownTime(int num);
    }
    public class MainView : MonoBehaviour, IMainView
    {
        [SerializeField] private TMP_Text _timerTMP;

        public void Init()
        {

        }

        public void SetCountDownTime(int num)
        {
            _timerTMP.text = num.ToString();
        }
    }
}
