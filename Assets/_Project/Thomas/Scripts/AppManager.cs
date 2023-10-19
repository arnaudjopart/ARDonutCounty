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
        [SerializeField] GameObject m_cubePrefab;
        private int m_nbrCubes;

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
                if (m_raycastManager.Raycast(_touchPosition, listOfHits, TrackableType.PlaneWithinPolygon) && m_planeManager.GetPlane(listOfHits[0].trackableId).alignment.IsHorizontal())
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
                if (m_raycastManager.Raycast(_touchPosition, listOfHits, TrackableType.PlaneWithinPolygon))
                {
                    ARRaycastHit hit = listOfHits[0];
                    Vector3 moveDirection = new(hit.pose.position.x - m_door.transform.position.x, 0, hit.pose.position.z - m_door.transform.position.z);
                    //Debug.Log(moveDirection.ToString());
                    if (moveDirection.magnitude < m_doorMoveSpeed * Time.deltaTime)
                        m_door.transform.Translate(moveDirection);
                    else
                    {
                        moveDirection = moveDirection.normalized;
                        m_door.transform.Translate(moveDirection * m_doorMoveSpeed * Time.deltaTime);
                    }
                }
            }
        }

        public void SpawnCubes(int _expectedCubesNbr)
        {
            if (m_door != null && m_nbrCubes < _expectedCubesNbr)
            {
                Unity.Mathematics.Random rand = new(((uint)Time.time));
                float distanceMax = 0.1f;
                while(m_nbrCubes < _expectedCubesNbr)
                {
                    float distanceX = rand.NextFloat(-distanceMax, distanceMax);
                    float distanceZ = rand.NextFloat(-distanceMax, distanceMax);
                    Vector3 origin = new(m_door.transform.position.x - distanceX, 0, m_door.transform.position.z - distanceZ);
                    if (Physics.Raycast(origin, m_doorPlane.normal, out RaycastHit hit) && hit.collider.TryGetComponent(out ARPlane foundPlane))
                    {
                        if(foundPlane.trackableId == m_doorPlane.trackableId)
                        {
                            Vector3 cubePosition = hit.point + m_doorPlane.normal;
                            Instantiate(m_cubePrefab, origin, Quaternion.Euler(0, 0, 0));
                            m_nbrCubes++;
                        }
                    }
                }
            }
        }
    }
}