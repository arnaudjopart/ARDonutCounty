using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
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
        private float m_doorPlaneHeight;
        [SerializeField] private float m_heightMargin = 0.1f;
        [SerializeField] private float m_doorMoveSpeed;
        [SerializeField] GameObject m_cubePrefab;
        [SerializeField] private Count m_nbrCubes;
        [SerializeField] private int m_finalNbrCubesOnSpawn;
        [SerializeField] private Count m_score;
        [SerializeField] private Material m_transparentMaterial;

        public void Start()
        {
            m_nbrCubes.count = 0;
            m_planeManager.planesChanged += SupressNewPlanes;
        }

        private void Update()
        {
            if (m_nbrCubes.count == 0 && m_door != null)
                SpawnCubes();
        }

        public override void ProcessTouchDown(Vector2 _touchPosition)
        {
            if (IsClickingOnUIElement() || m_door != null) return;
            List<ARRaycastHit> listOfHits = new();
            if (m_raycastManager.Raycast(_touchPosition, listOfHits, TrackableType.PlaneWithinPolygon) && m_planeManager.GetPlane(listOfHits[0].trackableId).alignment.IsHorizontal())
            {
                ARRaycastHit hit = listOfHits[0];
                m_doorPlaneHeight = m_planeManager.GetPlane(hit.trackableId).transform.position.y;
                Vector3 positionOfHit = hit.pose.position;
                m_door = Instantiate(m_doorPrefab, positionOfHit, Quaternion.identity);
                foreach (ARPlane plane in m_planeManager.trackables)
                {
                    if (PlaneIsInvalid(plane))
                        DeactivatePlane(plane);
                }
            }
        }

        public override void ProcessTouch(Vector2 _touchPosition)
        {
            if (IsClickingOnUIElement() || m_door == null) return;
                List<ARRaycastHit> listOfHits = new();
            if (m_raycastManager.Raycast(_touchPosition, listOfHits, TrackableType.PlaneWithinPolygon))
            {
                for (int i = 0; i < listOfHits.Count; i++)
                {
                    if (PlaneIsInvalid(m_planeManager.GetPlane(listOfHits[i].trackableId))) continue;
                    ARRaycastHit hit = listOfHits[i];
                    Vector3 moveDirection = new(hit.pose.position.x - m_door.transform.position.x, 0, hit.pose.position.z - m_door.transform.position.z);
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

        public void SpawnCubes()
        {
            if (m_door != null && m_nbrCubes.count < m_finalNbrCubesOnSpawn)
            {
                Unity.Mathematics.Random rand = new(((uint)Time.time));
                float distanceMin = 0.3f;
                float distanceMax = 1;
                int tries = 0;
                while(m_nbrCubes.count < m_finalNbrCubesOnSpawn)
                {
                    float distanceX = rand.NextFloat(distanceMin, distanceMax);
                    int exposant = rand.NextInt(0, 2);
                    if (exposant == 1)
                        distanceX = -distanceX;
                    float distanceZ = rand.NextFloat(distanceMin, distanceMax);
                    exposant = rand.NextInt(0, 2);
                    if (exposant == 1)
                        distanceZ = -distanceZ;
                    Vector3 origin = new(m_door.transform.position.x - distanceX, m_doorPlaneHeight + 1, m_door.transform.position.z - distanceZ);
                    if (Physics.Raycast(origin, -Vector3.up, out RaycastHit hit))
                    {
                        if (hit.collider.TryGetComponent(out ARPlane plane) && !PlaneIsInvalid(plane))
                        {
                                Vector3 cubePosition = hit.point + Vector3.up;
                                Instantiate(m_cubePrefab, cubePosition, Quaternion.Euler(0, 0, 0));
                                m_nbrCubes.count++;
                        }
                    }
                    tries++;
                    if (tries == 100)
                    {
                        Debug.Log("Echec");
                        break;
                    }
                }
            }
        }

        private void SupressNewPlanes(ARPlanesChangedEventArgs args)
        {
            if (m_door == null) return;
            foreach (ARPlane plane in args.added)
            {
                if (PlaneIsInvalid(plane))
                    DeactivatePlane(plane);
            }
            foreach (ARPlane plane in args.updated)
            {
                if (PlaneIsInvalid(plane))
                    DeactivatePlane(plane);
            }
        }

        private bool PlaneIsInvalid(ARPlane plane)
        {
            return (plane.transform.position.y < m_doorPlaneHeight - m_heightMargin || plane.transform.position.y > m_doorPlaneHeight + m_heightMargin);
        }

        private void DeactivatePlane(ARPlane plane)
        {
            plane.GetComponent<MeshCollider>().enabled = false;
            //plane.GetComponent<MeshRenderer>().
        }

        private bool IsClickingOnUIElement()
        {
            return EventSystem.current.IsPointerOverGameObject();
        }
    }
}