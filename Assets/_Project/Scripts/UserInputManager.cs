using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;

public class UserInputManager : MonoBehaviour
{
    [SerializeField] private ARRaycastManager m_raycastManager;
    [SerializeField] private GameObject m_holePrefab;
    [SerializeField] private GameObject m_cubePrefab;
    [SerializeField] private GameObject m_bigCube;
    private string m_name;
    public bool m_canSpawn = true;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            var position = new Vector2();
#if UNITY_EDITOR
            position = Input.mousePosition;
#else
            position = Input.GetTouch(0).position;
#endif

            var listOfHits = new List<ARRaycastHit>();
            if (m_raycastManager.Raycast(position, listOfHits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon) && m_canSpawn)
            {
                var hit = listOfHits[0];
                var positionOfHit = hit.pose.position;
                Instantiate(m_holePrefab, positionOfHit, Quaternion.identity);
                m_canSpawn = false;
            }
            
        } 
        
        /*if (Input.GetMouseButtonDown(1))
        {
            var position = new Vector2();
#if UNITY_EDITOR
            position = Input.mousePosition;
#else
            position = Input.GetTouch(1).position;
#endif

            var listOfHits = new List<ARRaycastHit>();
            if (m_raycastManager.Raycast(position, listOfHits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon))
            {
                var hit = listOfHits[0];
                var positionOfHit = hit.pose.position;
                Instantiate(m_cubePrefab, positionOfHit + new Vector3(0,0.1f,0), Quaternion.identity);
            }
        }*/

        if (Input.GetMouseButtonDown(2) || Input.touchCount == 2)
        {
            var position = new Vector2();
#if UNITY_EDITOR
            position = Input.mousePosition;
#else
            position = Input.GetTouch(1).position;
#endif

            var listOfHits = new List<ARRaycastHit>();
            if (m_raycastManager.Raycast(position, listOfHits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon))
            {
                var hit = listOfHits[0];
                var positionOfHit = hit.pose.position;
                Instantiate(m_bigCube, positionOfHit + new Vector3(0, 1f, 0), Quaternion.identity);
            }
        }
        
    }
    float lastTimeClick;

    public void OnPointerClick(PointerEventData eventData)
    {
        float currentTimeClick = eventData.clickTime;

        if (Mathf.Abs(currentTimeClick - lastTimeClick) < 0.75f)
        {
            var position = new Vector2();
#if UNITY_EDITOR
            position = Input.mousePosition;
#else
            position = Input.GetTouch(1).position;
#endif

            var listOfHits = new List<ARRaycastHit>();
            if (m_raycastManager.Raycast(position, listOfHits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon))
            {
                var hit = listOfHits[0];
                var positionOfHit = hit.pose.position;
                Instantiate(m_cubePrefab, positionOfHit + new Vector3(0, 0.1f, 0), Quaternion.identity);
            }
        }

        lastTimeClick = currentTimeClick;
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
