using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Christophe.Fanchamps;

public class SwipeController : MonoBehaviour
{
    Vector2 startTouchPosition;
    Vector2 endTouchPosition;
    [SerializeField] GameObject cubePrefab;
    Vector3 direction;
    [SerializeField] float throwForce;
    public static event Action<Vector3> OnCubeCreate;
    bool isMouseDown;
    [SerializeField] Projection projectionPrediction;
    Vector3 destination;


    private void Update()
    {


#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            startTouchPosition = Input.mousePosition;
            isMouseDown = true;

        }

        if (isMouseDown)
        {

        if(Input.GetMouseButtonUp(0))
        {
            endTouchPosition = Input.mousePosition;

            

            //get player Input
            float playerVerticalInput = (endTouchPosition.y - startTouchPosition.y )* throwForce;
            float playerHorizontalInput = (endTouchPosition.x - startTouchPosition.x)* throwForce;

            //get camera normalized directional vectors
            Vector3 forward = Camera.main.transform.forward;
            forward.y = 0;
            Vector3 right = Camera.main.transform.right;

            //create direction-relative-input vectors
            Vector3 forwardRelativeVerticalInput = playerVerticalInput * forward;
            Vector3 rigthRelativeHorizontalInput = playerHorizontalInput * right;

            //create and apply camera relativeMovement
            Vector3 cameraRelativeMovement = forwardRelativeVerticalInput + rigthRelativeHorizontalInput;



            GameObject cube = Instantiate(cubePrefab, Camera.main.transform.position + Camera.main.transform.forward * 0.5f, Quaternion.identity);
                Throw throwParameters = cube.GetComponent<Throw>();
            throwParameters.ThrowObject(cameraRelativeMovement);
                if (projectionPrediction.isActiveAndEnabled)
                {

                destination =  projectionPrediction.SimulateTrajectory(throwParameters, Camera.main.transform.position + Camera.main.transform.forward * 0.5f, cameraRelativeMovement);

                OnCubeCreate?.Invoke(destination);

                }
                isMouseDown = false;
        }


        }

#else
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            startTouchPosition = Input.mousePosition;
            isMouseDown = true;

        }

         if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
           endTouchPosition = Input.mousePosition;

            

            //get player Input
            float playerVerticalInput = (endTouchPosition.y - startTouchPosition.y )* throwForce;
            float playerHorizontalInput = (endTouchPosition.x - startTouchPosition.x)* throwForce;

            //get camera normalized directional vectors
            Vector3 forward = Camera.main.transform.forward;
            forward.y = 0;
            Vector3 right = Camera.main.transform.right;

            //create direction-relative-input vectors
            Vector3 forwardRelativeVerticalInput = playerVerticalInput * forward;
            Vector3 rigthRelativeHorizontalInput = playerHorizontalInput * right;

            //create and apply camera relativeMovement
            Vector3 cameraRelativeMovement = forwardRelativeVerticalInput + rigthRelativeHorizontalInput;



            GameObject cube = Instantiate(cubePrefab, Camera.main.transform.position + Camera.main.transform.forward * 0.5f, Quaternion.identity);
                Throw throwParameters = cube.GetComponent<Throw>();
            throwParameters.ThrowObject(cameraRelativeMovement);
                if (projectionPrediction.isActiveAndEnabled)
                {

                destination =  projectionPrediction.SimulateTrajectory(throwParameters, Camera.main.transform.position + Camera.main.transform.forward * 0.5f, cameraRelativeMovement);

                OnCubeCreate?.Invoke(destination);

                }
                isMouseDown = false;
        }



#endif







    }


}
