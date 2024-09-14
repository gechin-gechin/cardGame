using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace CardGame
{
    public interface IMainView
    {
        void SetCountDownTime(int num);
        void SetMessage(string text);
    }
    public class MainView : MonoBehaviour, IMainView
    {
        [SerializeField] private TMP_Text _timerTMP;
        [SerializeField] private CenterMessage _centerMessage;

        public void Init()
        {
            _centerMessage.Init();
        }

        public void SetCountDownTime(int num)
        {
            _timerTMP.text = num.ToString();
        }

        public void SetMessage(string text)
        {
            _ = _centerMessage.Show(text);
        }
    }
}
