using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Thomas
{
    public class DestroyCollider : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            Destroy(other.gameObject);
        }
    }
}