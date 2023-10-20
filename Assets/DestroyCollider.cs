using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyCollider : MonoBehaviour
{
    [SerializeField] private SizeModifier m_sizeModifier;
    private void OnTriggerEnter(Collider other)
    {
        m_sizeModifier.Grow();
        Destroy(other.gameObject);
    }
}
