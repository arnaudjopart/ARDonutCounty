using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IrisDoor : MonoBehaviour
{
    [SerializeField] Transform[] m_doors;
    void Start()
    {
        Open();
    }

    void Update()
    {

    }

    public void Open()
    {
        var sequence = DOTween.Sequence();
        sequence.AppendInterval(5f);
        foreach (var door in m_doors)
        {
            sequence.Join(door.DORotate(new Vector3(-90, 0, -90), 5f).SetEase(Ease.OutBounce));
            //sequence.AppendInterval(.1f);
        }
    }
}
