using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Christophe.Fanchamps
{
    public class JoystickController : MonoBehaviour
    {
       Joystick joystick;
        [SerializeField]
        float speed;


        private void Start()
        {
            joystick = FindObjectOfType<Joystick>().GetComponent<Joystick>();
        }

        private void Update()
        {
            //get player Input
            float playerVerticalInput = joystick.Vertical*speed*Time.deltaTime;
            float playerHorizontalInput = joystick.Horizontal * speed * Time.deltaTime;

            //get camera normalized directional vectors
            Vector3 forward = Camera.main.transform.forward;
            forward.y = 0;
            Vector3 right = Camera.main.transform.right;

            //create direction-relative-input vectors
            Vector3 forwardRelativeVerticalInput = playerVerticalInput * forward;
            Vector3 rigthRelativeHorizontalInput = playerHorizontalInput * right;

            //create and apply camera relativeMovement
            Vector3 cameraRelativeMovement = forwardRelativeVerticalInput + rigthRelativeHorizontalInput;
            transform.Translate(cameraRelativeMovement, Space.World);
        }
    }
}


