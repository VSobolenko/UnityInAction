using System;
using UnityEngine;

namespace UnionToFinalGame
{
    public class BaseDevice : MonoBehaviour
    {
        public float radius;

        private void OnMouseDown()
        {
            var player = GameObject.FindGameObjectWithTag("Player").transform;
            if (Vector3.Distance(player.position, transform.position) < radius)
            {
                var direction = transform.position - player.position;
                if (Vector3.Dot(player.forward, direction) > 0.5f)
                {
                    Operate();
                }
            }
        }
        
        public virtual  void Operate(){ }
    }
}