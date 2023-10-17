using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace Ajc
{
    public class UserInputManager : MonoBehaviour
    {
        [SerializeField] private ARRaycastManager m_raycastManager;
        [SerializeField] private GameObject m_holePrefab;
        private string m_name;


        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 finger_0_position = Input.GetTouch(0).position;
                var listOfHits = new List<ARRaycastHit>();
                if (m_raycastManager.Raycast(finger_0_position, listOfHits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon))
                {
                    var hit = listOfHits[0];
                    var positionOfHit = hit.pose.position;
                    Instantiate(m_holePrefab, positionOfHit, Quaternion.identity);
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


