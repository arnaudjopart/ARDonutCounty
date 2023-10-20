using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
        [SerializeField] GameObject m_cubePrefab;
        private GameObject m_door;
        private float m_doorPlaneHeight;
        [SerializeField] private float m_doorMoveSpeed = 1;
        [SerializeField] private float m_heightMargin = 0.1f;
        [SerializeField] private float m_growingMultiplier = 1.2f;
        [SerializeField] private int m_finalLevel = 10;
        [SerializeField] private Count m_nbrCubes;
        [SerializeField] private Count m_score;
        [SerializeField] private Count m_level;
        [SerializeField] private Material m_transparentMaterial;
        [SerializeField] private TMP_Text m_scoreText;
        [SerializeField] private TMP_Text m_levelText;

        public void Start()
        {
            m_nbrCubes.count = 0;
            m_score.count = 0;
            m_scoreText.text = m_score.count.ToString();
            m_level.count = 1;
            m_levelText.text = "Level " + m_level.count.ToString();
            m_planeManager.planesChanged += SupressNewPlanes;
        }

        private void Update()
        {
            if (int.TryParse(m_scoreText.text, out int score) && score != m_score.count)
            {
                m_scoreText.text = m_score.count.ToString();
                if (m_score.count % (5 * ((m_level.count) * (m_level.count))) == 0)
                {
                    if (m_level.count < m_finalLevel)
                    {
                        m_level.count++;
                        m_levelText.text = "Level " + m_level.count.ToString();
                        GrowDoor(m_growingMultiplier);
                    }
                    else
                        Debug.Log("Bravo, vous avez gagné !");
                }
            }
            if (m_nbrCubes.count == 0 && m_door != null && score < 5 * m_finalLevel * m_finalLevel)
            {
                SpawnCubes();
            }
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
            DetermineAmoutOfCubesToSpawn(out int amountSmallCubes);
            if (m_door != null && m_nbrCubes.count < amountSmallCubes)
            {
                Unity.Mathematics.Random rand = new(((uint)Time.time));
                float distanceMin = 0.3f;
                float distanceMax = 0.5f * m_level.count;
                int tries = 0;
                while(m_nbrCubes.count < amountSmallCubes)
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
                            GameObject cube = Instantiate(m_cubePrefab, cubePosition, Quaternion.Euler(0, 0, 0));
                            cube.GetComponent<Cube>().m_value = 1;
                            m_nbrCubes.count++;

                            if (m_nbrCubes.count + m_score.count >= 5 * m_finalLevel * m_finalLevel)
                                break;
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

        public void DetermineAmoutOfCubesToSpawn(out int _amountSmallCubes)
        {
            _amountSmallCubes = m_level.count * 5;
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

        private void GrowDoor(float _multiplier)
        {
            if (m_door == null) return;
            m_door.transform.localScale *= _multiplier;
            m_doorMoveSpeed *= _multiplier;
        }

        private bool IsClickingOnUIElement()
        {
            return EventSystem.current.IsPointerOverGameObject();
        }
    }
}