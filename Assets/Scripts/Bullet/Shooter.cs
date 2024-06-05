using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BULLET
{
    public class Shooter : MonoBehaviour
    {
        [SerializeField] private Bullet bullet = null;
        public void Shot(Transform target)
        {
            Bullet b = Instantiate(bullet, this.transform, false);
            b.transform.position = this.transform.position;
            b.Init(target);
        }
    }
}
