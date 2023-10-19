using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using DG.Tweening;
using UnityEngine.XR.Interaction.Toolkit.Utilities.Tweenables.Primitives;

namespace Christophe.Fanchamps { 

public class UserInputManager : MonoBehaviour
{
    [SerializeField] private ARRaycastManager m_raycastManager;
    [SerializeField] private GameObject m_holePrefab, m_cubePrefab;
    private string m_name;
    GameObject m_hole;
    [SerializeField] float m_speed;

    // Start is called before the first frame update
    void Start()
    {
       
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
                if (m_raycastManager.Raycast(position, listOfHits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon))
                {
                    var hit = listOfHits[0];
                    var positionOfHit = hit.pose.position;
                    if (m_hole == null)
                    {
                        m_hole = Instantiate(m_holePrefab, positionOfHit, Quaternion.identity);
                    }
                    else
                    {

                        Instantiate(m_cubePrefab, positionOfHit + Vector3.up * .8f, Quaternion.identity);
                        //float distance = Vector3.Distance(m_hole.transform.position, positionOfHit);
                        //m_hole.transform.DOMove(positionOfHit, m_speed * distance);
                    }


                }
            }

            if (Input.GetMouseButtonDown(1))
            {
                var position = new Vector2();
#if UNITY_EDITOR
                position = Input.mousePosition;
#else
            position = Input.GetTouch(0).position;
#endif

                var listOfHits = new List<ARRaycastHit>();
                if (m_raycastManager.Raycast(position, listOfHits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon))
                {
                    var hit = listOfHits[0];
                    var positionOfHit = hit.pose.position;
                    {
                       Instantiate(m_cubePrefab, positionOfHit+Vector3.up*.8f, Quaternion.identity);




                    }
                }
            }
        }

    private string ReturnGameObjectName(GameObject _gameObject)
    {
        m_name = _gameObject.name;
        return m_name;
    }

    private bool IsActive()
    {
        return gameObject.activeSelf;
    }
}
}
