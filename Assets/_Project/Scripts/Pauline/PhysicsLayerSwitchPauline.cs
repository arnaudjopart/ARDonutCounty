using DG.Tweening;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsLayerSwitchPauline : MonoBehaviour
{
    [SerializeField] int m_cubesEaten;
    private Transform m_doorTransform;

    private void Awake()
    {
        m_cubesEaten = 0;
        m_doorTransform = gameObject.transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.layer = 10;
        m_cubesEaten++;
    }

    private void Update()
    {
        if(m_cubesEaten >= 5)
        {
            m_doorTransform.DOScale(m_doorTransform.localScale * 1.05f, 0.5f).SetEase(Ease.OutBounce);

            Debug.Log("Yay you grew up");
            m_cubesEaten = 0;
        }
    }
}
