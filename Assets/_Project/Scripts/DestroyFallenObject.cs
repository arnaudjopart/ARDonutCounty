using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Christophe.Fanchamps
{


public class DestroyFallenObject : MonoBehaviour
{

        Transform m_hole;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collision");
        Destroy(other.gameObject);
            m_hole = gameObject.transform.root;
            m_hole.DOScale(m_hole.localScale * 1.2f, 0.2f);
    }
}
}
