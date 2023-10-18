using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using DG.Tweening;

namespace pauline.gossart
{
    public class UserInputManagerPauline : MonoBehaviour
    {
        [SerializeField] private ARRaycastManager m_raycastManager;
        [SerializeField] private GameObject m_holePrefab;
        [SerializeField] private GameObject m_cubePrefab;
        [SerializeField] private float duration;
        private string m_name;
        private GameObject m_hole;


        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 position = new Vector2();
#if UNITY_EDITOR
                position = Input.mousePosition;
#else
            position = Input.GetTouch(0).position;
#endif

                List<ARRaycastHit> listOfHits = new List<ARRaycastHit>();

                if (m_raycastManager.Raycast(position, listOfHits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon))
                {
                    ARRaycastHit hit = listOfHits[0];
                    var positionOfHit = hit.pose.position;

                    if (m_hole == null)
                    {
                        m_hole = Instantiate(m_holePrefab, positionOfHit, Quaternion.identity);
                    }
                    else
                    {
                        float distance = Vector3.Distance(m_hole.transform.position, positionOfHit);
                        //m_hole.transform.position = positionOfHit;
                        m_hole.transform.DOMove(positionOfHit, duration * distance).SetEase(Ease.InOutSine);
                    }
                }
            }

            if (Input.GetMouseButtonDown(1))
            {
                Vector2 position = new Vector2();
                position = Input.mousePosition;

                List<ARRaycastHit> listOfHits = new List<ARRaycastHit>();

                if (m_raycastManager.Raycast(position, listOfHits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon))
                {
                    ARRaycastHit hit = listOfHits[0];
                    Vector3 positionOfHits = hit.pose.position;
                    
                    Instantiate(m_cubePrefab, positionOfHits, Quaternion.identity);
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


