using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class UserInputManager : MonoBehaviour
{
    [SerializeField] private ARRaycastManager m_raycastManager;
    [SerializeField] private GameObject m_holePrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
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
}
