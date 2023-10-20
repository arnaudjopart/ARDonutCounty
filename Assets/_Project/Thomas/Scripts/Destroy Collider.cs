using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Thomas
{
    public class DestroyCollider : MonoBehaviour
    {
        [SerializeField] private Count m_nbrCubes;
        [SerializeField] private Count m_score;

        private void OnTriggerEnter(Collider other)
        {
            Destroy(other.gameObject);
            m_nbrCubes.count--;
        }
    }
}