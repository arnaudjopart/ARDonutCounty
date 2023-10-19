using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class IrisDoor : MonoBehaviour
{
    [SerializeField] Transform[] m_doors;
    [SerializeField] private ARRaycastManager m_move;
    [SerializeField] private GameObject m_holePrefab;




    // Start is called before the first frame update
    void Start()
    {
        Open();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var position = new Vector2();
#if UNITY_EDITOR
            position = Input.mousePosition;
#else
            position = Input.GetTouch(0).position;
#endif

            var listOfHits = new List<ARRaycastHit>();
            if (m_move.Raycast(position, listOfHits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon))
            {
                var hit = listOfHits[0];
                var positionOfHit = hit.pose.position;
                Instantiate(m_holePrefab, positionOfHit, Quaternion.identity);
            }
        }
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
