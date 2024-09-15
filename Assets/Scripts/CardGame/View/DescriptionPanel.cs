using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace CardGame
{
    public class DescriptionPanel : PooledObject<DescriptionPanel>
    {
        [SerializeField] private TMP_Text _name_text;
        [SerializeField] private TMP_Text _desc_text;

        public async UniTask ShowSequence(string cardName, string description)
        {
            _name_text.text = cardName;
            _desc_text.text = description;
            await UniTask.WaitForSeconds(2f);
            ReleaseToPool();
        }
    }
}
