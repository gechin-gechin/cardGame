using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardGame
{
    public abstract class PooledObject<T> : MonoBehaviour where T : PooledObject<T>
    {
        public Action<T> OnReleaseToPool;

        protected void ReleaseToPool()
        {
            OnReleaseToPool?.Invoke(this as T);
        }
    }
}
