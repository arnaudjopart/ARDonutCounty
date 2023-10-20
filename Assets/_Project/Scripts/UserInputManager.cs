using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;

public class UserInputManager : MonoBehaviour
{
    [SerializeField] private ARRaycastManager m_raycastManager;
    [SerializeField] private DoorController m_controller;
    [SerializeField] private GameObject m_cubeDebugPrefab;


    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if (IsClickingOnUIElement()) return;
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
                m_controller.ProcessTouch(positionOfHit);
                
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

    private bool IsClickingOnUIElement()
    {
        return EventSystem.current.IsPointerOverGameObject(); 
    }

    public void GenerateCubeAtViewPortPosition()
    {
        var position = Camera.main.ViewportToScreenPoint(new Vector2(.5f, .5f));

        var listOfHits = new List<ARRaycastHit>();
        if (m_raycastManager.Raycast(position, listOfHits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon))
        {
            var hit = listOfHits[0];
            var positionOfHit = hit.pose.position;
            Instantiate(m_cubeDebugPrefab, positionOfHit + Vector3.up * .8f, Quaternion.identity);

        }
    }
}
