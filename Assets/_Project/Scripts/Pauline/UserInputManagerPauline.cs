using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using DG.Tweening;
using UnityEngine.EventSystems;

namespace pauline.gossart
{
    public class UserInputManagerPauline : MonoBehaviour
    {
        [SerializeField] private ARRaycastManager m_raycastManager;
        [SerializeField] private GameObject m_holePrefab;
        [SerializeField] private GameObject m_cubePrefab;
        [SerializeField] private float duration;
        [SerializeField] private float cubeDropDistance = .5f;
        [SerializeField] private Joystick m_joystick;
        private string m_name;
        private GameObject m_hole;
        private Camera m_camera;

        private Vector3 m_startPosition;
        private Vector3 m_endPosition;
        private Vector3 m_currentSwipe;
        private bool m_allowsShooting;

        // Start is called before the first frame update
        void Start()
        {
            m_camera = Camera.main;
        }


        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {

                if (IsClickingOnUIElement()) return;



#if UNITY_EDITOR
                m_startPosition = Input.mousePosition;
#else
            m_startPosition = Input.GetTouch(0).position;
#endif

                List<ARRaycastHit> listOfHits = new List<ARRaycastHit>();

                if (m_raycastManager.Raycast(m_startPosition, listOfHits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon))
                {
                    ARRaycastHit hit = listOfHits[0];
                    var positionOfHit = hit.pose.position;

                    if (m_hole == null)
                    {
                        m_hole = Instantiate(m_holePrefab, positionOfHit, Quaternion.identity);

                    }

                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (IsClickingOnUIElement()) return;

                if (m_allowsShooting == false)
                {
                    m_allowsShooting = true;
                    return;
                }

                m_endPosition = Input.mousePosition;

                m_currentSwipe = m_endPosition - m_startPosition;

                if (m_currentSwipe != m_startPosition)
                {
                    var instance = Instantiate(m_cubePrefab, m_startPosition, Quaternion.identity);
                    instance.GetComponent<Rigidbody>().AddForce(m_startPosition * 3);

                    Debug.Log("this is the current swipe position " + m_currentSwipe);
                }

            }



            if (m_hole != null)
            {
                //Player Input
                float playerHorizontalInput = m_joystick.Horizontal;
                float playerVerticalInput = m_joystick.Vertical;

                //Camera normalized directional vectors
                Vector3 right = m_camera.transform.right;
                Vector3 forward = m_camera.transform.forward;

                forward.y = 0;
                right.y = 0;
                forward = forward.normalized;
                right = right.normalized;

                //Creating directional relative input 
                Vector3 forwardRelativeHorizontalInput = playerHorizontalInput * right;
                Vector3 rightRelativeVerticalInput = playerVerticalInput * forward;

                //Apply camera relative movement
                Vector3 cameraRelativeMovement = forwardRelativeHorizontalInput + rightRelativeVerticalInput;

                m_hole.transform.Translate(cameraRelativeMovement * Time.deltaTime, Space.World);


                //m_hole.transform.Translate((playerHorizontalInput * Time.deltaTime), 0, (playerVerticalInput * Time.deltaTime));

            }

            /*if (Input.GetMouseButtonDown(1))
            {
                Vector2 position = new Vector2();
                position = Input.mousePosition;

                List<ARRaycastHit> listOfHits = new List<ARRaycastHit>();

                if (m_raycastManager.Raycast(position, listOfHits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon))
                {
                    ARRaycastHit hit = listOfHits[0];
                    Vector3 positionOfHits = hit.pose.position;
                    
                    Instantiate(m_cubePrefab, positionOfHits + Vector3.up * cubeDropDistance, Quaternion.identity);
                }
            }*/

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

        private bool IsClickingOnUIElement()
        {
            return EventSystem.current.IsPointerOverGameObject();
        }

        private void Swipe()
        {
            if (Input.GetMouseButtonDown(0))
            {
                m_startPosition = Input.mousePosition;
            }

            if (Input.GetMouseButtonUp(0))
            {
                m_endPosition = Input.mousePosition;

                m_currentSwipe = m_endPosition - m_startPosition;

                Debug.Log("this is the current swipe position " + m_currentSwipe);
            }
        }

    }

}


