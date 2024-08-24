using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace CardGame
{
    public class ScriptableObjectLoader<T> : IDisposable where T : ScriptableObject
    {
        private AsyncOperationHandle<IList<T>> handle;
        public async UniTask<List<T>> LoadAll(string label)
        {
            var objs = new List<T>();
            handle = Addressables.LoadAssetsAsync<T>(label, null);
            await handle.ToUniTask();
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                foreach (var o in handle.Result)
                {
                    objs.Add(o);
                }
            }
            return objs;
        }

        public void Dispose()
        {
            Addressables.Release(handle);
        }
    }
}
