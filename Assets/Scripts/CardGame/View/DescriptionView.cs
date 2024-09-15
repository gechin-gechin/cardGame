using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CardGame
{
    public class DescriptionView : ObjectPool<DescriptionPanel>
    {
        public void Set(string cardName, string description)
        {
            var panel = GetPooledObject();
            panel.transform.SetParent(this.transform);
            panel.transform.SetAsLastSibling();
            panel.transform.localScale = Vector3.one;
            _ = panel.ShowSequence(cardName, description);
        }
    }
}
