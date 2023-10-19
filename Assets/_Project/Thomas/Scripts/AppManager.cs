using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace Thomas
{
    public class AppManager : InputListenerBase
    {
        [SerializeField] private ARPlaneManager m_planeManager;
        [SerializeField] private ARRaycastManager m_raycastManager;
        [SerializeField] private GameObject m_doorPrefab;
        private GameObject m_door;
        private ARPlane m_doorPlane;
        [SerializeField] private float m_doorMoveSpeed;

        private void Start()
        {

        }

        private void Update()
        {
            
        }

        public override void ProcessTouchDown(Vector2 _touchPosition)
        {
            if (m_door == null)
            {
                List<ARRaycastHit> listOfHits = new();
                if (m_raycastManager.Raycast(_touchPosition, listOfHits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon) && m_planeManager.GetPlane(listOfHits[0].trackableId).alignment.IsHorizontal())
                {
                    ARRaycastHit hit = listOfHits[0];
                    m_doorPlane = m_planeManager.GetPlane(hit.trackableId);
                    Vector3 positionOfHit = hit.pose.position;
                    m_door = Instantiate(m_doorPrefab, positionOfHit, Quaternion.identity);
                }
            }
        }

        public override void ProcessTouch(Vector2 _touchPosition)
        {
            if (m_door != null)
            {
                List<ARRaycastHit> listOfHits = new();
                if (m_raycastManager.Raycast(_touchPosition, listOfHits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon))
                {
                    for (int i = 0; i < listOfHits.Count; i++)
                    {
                        if (m_planeManager.GetPlane(listOfHits[i].trackableId).trackableId == m_doorPlane.trackableId)
                        {
                            ARRaycastHit hit = listOfHits[i];
                            Vector3 moveDirection = new(hit.pose.position.x - m_door.transform.position.x, 0, hit.pose.position.z - m_door.transform.position.z);
                            //Debug.Log(moveDirection.ToString());
                            if (moveDirection.magnitude < m_doorMoveSpeed * Time.deltaTime)
                                m_door.transform.Translate(moveDirection);
                            else
                            {
                                moveDirection = moveDirection.normalized;
                                m_door.transform.Translate(moveDirection * m_doorMoveSpeed * Time.deltaTime);
                            }
                            break;
                        }
                    }
                }
            }
        }

        

        private void UpdatePlane()
        {
            Debug.Log("Grossis");
        }
    }
}