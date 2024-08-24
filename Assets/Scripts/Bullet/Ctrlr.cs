using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using R3;

namespace BULLET
{
    public class Ctrlr : MonoBehaviour
    {
        [SerializeField] private Shooter shooter = null;
        [SerializeField] private GameObject target = null;

        private void Start()
        {
            Observable<Unit> click = Observable
                .EveryUpdate()
                .Where(_ => Input.anyKeyDown)
                .ThrottleFirst(System.TimeSpan.FromSeconds(0.2));
                

            click.Subscribe(_ => shooter.Shot(target.transform))
                .AddTo(this);
        }
    }
}
