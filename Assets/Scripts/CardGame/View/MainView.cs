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
        void SetDesCription(string cardName, string description);
    }
    public class MainView : MonoBehaviour, IMainView
    {
        [SerializeField] private TMP_Text _timerTMP;
        [SerializeField] private CenterMessage _centerMessage;
        [SerializeField] private DescriptionView _descriptionView;

        public void Init()
        {
            _centerMessage.Init();
            _descriptionView.Init(3);
        }

        public void SetCountDownTime(int num)
        {
            _timerTMP.text = num.ToString();
        }

        public void SetMessage(string text)
        {
            _ = _centerMessage.Show(text);
        }

        public void SetDesCription(string cardName, string description)
        {
            _descriptionView.Set(cardName, description);
        }
    }
}
