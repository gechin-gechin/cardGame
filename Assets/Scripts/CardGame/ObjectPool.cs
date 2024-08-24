using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardGame
{
    public abstract class ObjectPool<T> : MonoBehaviour where T : PooledObject<T>
    {
        [SerializeField] private T _pooledObject;
        private Stack<T> _stack;

        public void Init(int initSize)
        {
            _stack = new();
            for (int i = 0; i < initSize; i++)
            {
                var obj = Instantiate(_pooledObject);
                obj.OnReleaseToPool += (o) => ReturnToPool(o);
                obj.gameObject.SetActive(false);
                _stack.Push(obj);
            }
            AltInit();
        }
        protected virtual void AltInit() { }

        protected T GetPooledObject()
        {
            if (_stack.Count == 0)
            {
                T obj = Instantiate(_pooledObject);
                obj.OnReleaseToPool += (o) => ReturnToPool(o);
                return obj;
            }
            T nextInstance = _stack.Pop();
            nextInstance.gameObject.SetActive(true);
            return nextInstance;
        }

        private void ReturnToPool(T pooledObject)
        {
            _stack.Push(pooledObject);
            pooledObject.gameObject.SetActive(false);
        }
    }
}
