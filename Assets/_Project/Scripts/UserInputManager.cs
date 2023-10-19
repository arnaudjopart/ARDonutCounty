using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class UserInputManager : MonoBehaviour
{
    [SerializeField] private ARRaycastManager m_raycastManager;
    [SerializeField] private DoorController m_controller;
    [SerializeField] private GameObject m_cubeDebugPrefab;
    [SerializeField] private GameObject m_holePrefab;
    private string m_name;
    private bool m_spawnBool = true; 

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
            if (m_raycastManager.Raycast(position, listOfHits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon) && m_spawnBool)
            {
                var hit = listOfHits[0];
                var positionOfHit = hit.pose.position;
                //m_controller.ProcessTouch(positionOfHit);
                
                Instantiate(m_holePrefab, positionOfHit, Quaternion.identity);
                m_spawnBool = false;
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
                Instantiate(m_cubeDebugPrefab, positionOfHit + Vector3.up * .8f, Quaternion.identity);

            }
        }
    }
}
