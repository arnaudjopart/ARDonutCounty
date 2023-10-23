using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Thomas
{
    public class SelfDestroyCube : MonoBehaviour
    {
        private GameObject m_door;

        void Start()
        {
            m_door = FindObjectOfType<IrisDoor>().gameObject;
        }

        void Update()
        {
            if(transform.position.y <= m_door.transform.position.y - 10)
                Destroy(gameObject);
        }
    }
}