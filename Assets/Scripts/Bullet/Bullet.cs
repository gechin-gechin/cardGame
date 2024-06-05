using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BULLET
{
    public class Bullet : MonoBehaviour
    {
        private Vector3 vel;
        private Vector3 pos;
        private Transform target;
        //残り時間
        private float period = 1f;

        public void Init(Transform targetTransform)
        {
            target = targetTransform;
            pos = this.transform.position;
        }

        private void FixedUpdate()
        {
            Vector3 a = Vector3.zero;//acceleration
            Vector3 diff = target.position - pos;
            //運動方程式
            a += (diff - vel * period) * 2f / (period * period);
            period -= Time.deltaTime;
            if (period < 0f)
            {
                endChasing();
                return;
            }

            vel += a * Time.deltaTime;
            pos += vel * Time.deltaTime;
            transform.position = pos;
        }

        private void endChasing()
        {
            Debug.Log("end");
            Destroy(this.gameObject);
        }
    }
}
