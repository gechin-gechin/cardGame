using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using R3;
using DG.Tweening;

namespace BULLET
{
    public class UnitaskTest : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            var cts = new CancellationTokenSource();
            var token = cts.Token;

            Observable.EveryUpdate()
                .Where(_ => Input.GetMouseButtonDown(0))
                .ThrottleFirst(TimeSpan.FromSeconds(2))
                .SubscribeAwait(async (str, token) => {
                    await transform.DOLocalMove(Vector3.one,2f)
                        .OnComplete(() => Debug.Log("complete"))
                        .ToUniTask(TweenCancelBehaviour.Complete,token);
                    }).AddTo(this);
        }
    }
}
