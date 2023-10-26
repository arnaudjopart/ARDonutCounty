using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEngine.XR.ARCore;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace Thomas
{
    public class AppManager : InputListenerBase
    {
        private ARPlaneManager m_planeManager;
        private ARRaycastManager m_raycastManager;
        [SerializeField] private GameObject m_doorPrefab;
        [SerializeField] private GameObject m_cubePrefab;
        private GameObject m_door;
        //private Vector3 m_doorScale;
        //private int m_doorSize = 1;
        private float m_doorPlaneHeight;
        [SerializeField] private float m_doorMoveSpeed = 1;
        [SerializeField] private float m_heightMargin = 0.1f;
        [SerializeField] private float m_sizeIncrease = 1.2f;
        [SerializeField] private int m_baseQuantity = 5;
        [SerializeField] private int m_stepLevel = 5;
        [SerializeField] private int m_finalLevel = 10;
        private float m_timer;
        private int m_finalScore;
        [SerializeField] private Count m_nbrCubes;
        [SerializeField] private Count m_score;
        [SerializeField] private Count m_level;
        [SerializeField] private Count m_totalValueOfCurrentCubes;
        private List<HighScore> m_hightScores;
        [SerializeField] private Material m_transparentMaterial;
        private TMP_Text m_scoreText;
        private TMP_Text m_levelText;

        private void Awake()
        {
            m_planeManager = FindObjectOfType<ARPlaneManager>();
            m_raycastManager = FindObjectOfType<ARRaycastManager>();
            m_scoreText = FindObjectOfType<ScoreDisplay>().gameObject.GetComponent<TMP_Text>();
            m_levelText = FindObjectOfType<LevelDisplay>().gameObject.GetComponent<TMP_Text>();
        }

        public void Start()
        {
            foreach(ARPlane plane in m_planeManager.trackables)
            {
                Destroy(plane);
            }
            Debug.Log(m_planeManager.trackables.count);
            m_timer = 0;
            m_nbrCubes.count = 0;
            m_score.count = 0;
            m_scoreText.text = m_score.count.ToString();
            m_level.count = 1;
            m_levelText.text = "Level " + m_level.count.ToString();
            m_totalValueOfCurrentCubes.count = 0;
            m_finalScore = m_baseQuantity * m_finalLevel * m_finalLevel;
            m_planeManager.planesChanged += SupressNewPlanes;
        }

        private void Update()
        {
            //if (m_door == null) return;
            if (int.TryParse(m_scoreText.text, out int score) && score != m_score.count)
            {
                m_scoreText.text = m_score.count.ToString();
                if (m_score.count >= m_baseQuantity * m_level.count * m_level.count)
                {
                    if (m_level.count < m_finalLevel)
                    {
                        m_level.count++;
                        m_levelText.text = "Level " + m_level.count.ToString();
                        GrowDoor(m_sizeIncrease);
                    }
                    else
                    {
                        Debug.Log("Bravo, vous avez gagné !");
                        //ManageHisghScores();
                    }
                }
            }
            if (m_door == null) return; //temporary
            if (m_nbrCubes.count == 0 && score < m_finalScore)
            {
                SpawnCubes();
            }
            if (score < m_finalScore)
                m_timer += Time.deltaTime;
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
                //m_doorScale = m_door.transform.lossyScale;
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
            if(m_door == null) return;
            DetermineAmoutOfCubesToSpawn(out List<int> amountOfCubesFromSmallestToLargest, out int totalAmount);
            if (m_nbrCubes.count >= totalAmount) return;
            Unity.Mathematics.Random rand = new(((uint)Time.time));
            float distanceMax = 0.3f + (0.2f * m_level.count);
            int tries = 0;
            for(int i = amountOfCubesFromSmallestToLargest.Count - 1; i >= 0; i--)
            {
                int currentPlusPreviousAimedAmount = 0;
                for(int j = i; j < amountOfCubesFromSmallestToLargest.Count; j++)
                {
                    currentPlusPreviousAimedAmount += amountOfCubesFromSmallestToLargest[j];
                }
                while (m_nbrCubes.count < currentPlusPreviousAimedAmount)
                {
                    if (m_totalValueOfCurrentCubes.count + m_score.count >= (m_finalScore) - i) break;
                    float distanceX = rand.NextFloat(-distanceMax, distanceMax);
                    float distanceZ = rand.NextFloat(-distanceMax, distanceMax);
                    Vector3 origin = new(m_door.transform.position.x - distanceX, m_doorPlaneHeight + 1, m_door.transform.position.z - distanceZ);
                    if (Physics.Raycast(origin, -Vector3.up, out RaycastHit hit) && hit.collider.TryGetComponent(out ARPlane plane) && !PlaneIsInvalid(plane))
                    {
                        Vector3 cubePosition = hit.point + Vector3.up;
                        GameObject cube = Instantiate(m_cubePrefab, cubePosition, Quaternion.Euler(0, 0, 0));
                        cube.GetComponent<Cube>().m_value = i + 1;
                        cube.transform.localScale *= i + 1;
                        m_nbrCubes.count++;
                        m_totalValueOfCurrentCubes.count += cube.GetComponent<Cube>().m_value;
                    }
                    else
                    {
                        tries++;
                        if (tries == 1000)
                        {
                            Debug.Log("Echec - Small");
                            break;
                        }
                    }
                }
                if (tries == 1000) break;
            }
        }

        public void DetermineAmoutOfCubesToSpawn(out List<int> _amountCubesFromSmallestToLargest, out int _totalAmount)
        {
            _amountCubesFromSmallestToLargest = new List<int>();
            _totalAmount = 0;
            int amountOfDifferentTypes = (m_level.count / m_stepLevel) + 1;
            if (m_level.count % m_stepLevel == 0)
            {
                if (m_level.count != m_finalLevel)
                {
                    for (int i = 0; i < amountOfDifferentTypes; i++)
                    {
                        int quantity = 0;
                        if (i == amountOfDifferentTypes - 1)
                            quantity = m_baseQuantity;
                        _amountCubesFromSmallestToLargest.Add(quantity);
                        _totalAmount += quantity;
                    }
                }
                else
                {
                    amountOfDifferentTypes--;
                    for (int i = 0; i < amountOfDifferentTypes; i++)
                    {
                        int quantity = m_stepLevel * (m_baseQuantity - ((amountOfDifferentTypes - 1) - i));
                        if (quantity < 0)
                            quantity = 0;
                        _amountCubesFromSmallestToLargest.Add(quantity);
                        _totalAmount += quantity;
                    }
                }
            }
            else
            {
                for (int i = 0; i < amountOfDifferentTypes; i++)
                {
                    int quantity = (m_level.count % m_stepLevel) * (m_baseQuantity - ((amountOfDifferentTypes - 1) - i));
                    if (quantity < 0)
                        quantity = 0;
                    _amountCubesFromSmallestToLargest.Add(quantity);
                    _totalAmount += quantity;
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

        private void GrowDoor(float _multiplier)
        {
            if (m_door == null) return;
            m_door.transform.localScale *= _multiplier;
            m_doorMoveSpeed *= _multiplier;

            //m_door.transform.localScale = new(m_door.transform.localScale.x + _multiplier, m_door.transform.localScale.y, m_door.transform.localScale.z + _multiplier);
            //m_doorSize++;
            //m_doorMoveSpeed += _multiplier;
        }

        private bool IsClickingOnUIElement()
        {
            return EventSystem.current.IsPointerOverGameObject();
        }

        private void ManageHisghScores()
        {
            m_hightScores.Add(new HighScore(m_timer, "Thomas"));
            for(int i = 1; i < m_hightScores.Count; i++)
            {
                if (m_hightScores[i].m_time < m_hightScores[i - 1].m_time)
                {
                    (m_hightScores[i], m_hightScores[i - 1]) = (m_hightScores[i - 1], m_hightScores[i]);
                    if (i != 0 && i != 1)
                        i -= 2;
                }
            }
        }

        public void Restart()
        {
            Debug.Log("Ici");
            SceneManager.LoadScene(0);
        }
    }
}