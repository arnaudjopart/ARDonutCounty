using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Thomas
{
    public class Cube : MonoBehaviour
    {
        [SerializeField] private Count m_score;
        [HideInInspector] public int m_value;

        private void OnDestroy()
        {
            m_score.count += m_value;
        }
    }
}