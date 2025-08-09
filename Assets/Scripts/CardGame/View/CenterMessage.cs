using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace CardGame
{
    public class CenterMessage : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private CanvasGroup _canvasGroup;

        public void Init()
        {
            _canvasGroup.alpha = 0;
        }

        public async UniTask Show(string text)
        {
            _text.text = text;
            _canvasGroup.alpha = 1;
            await UniTask.WaitForSeconds(2f);
            _canvasGroup.alpha = 0;
        }
    }
}
