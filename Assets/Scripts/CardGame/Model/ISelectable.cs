using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardGame
{
    public interface ISelectable
    {
        void SetIsBlocker(bool value);
        void ChangePower(int amount);
        void Dead();
        void SetSelectable(bool value);
    }
}
