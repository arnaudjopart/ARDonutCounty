using Christophe.Fanchamps;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

namespace Christophe.fanchamps
{
    public class FollowController : MonoBehaviour
    {
        HoleFalling cubeComponent;
        GameObject cube;
        Vector3 startingPos;
        Vector3 endPosition;
        bool isFollowing = false;
        [SerializeField] float speed;

        private void Start()
        {
            SwipeController.OnCubeCreate += GetDestination;
        }

        private void Update()
        {
            if(isFollowing)
            {
                //Vector3 position = new Vector3(cube.transform.position.x, 0, cube.transform.position.z);
                //transform.position = position;
                //transform.position = Vector3.Lerp(transform.position, position, (Mathf.Abs(startingPos.y-cube.transform.position.y)/startingPos.y));
                transform.position = Vector3.Lerp(transform.position, endPosition, speed*Time.deltaTime);
                if (Vector3.SqrMagnitude(endPosition-transform.position) < 0.01)
                {
                    isFollowing = false;
                }
   

            }
        }

        void GetDestination(Vector3 _destination)
        {
            endPosition = _destination;
            Debug.Log("endposition : " + endPosition);
            isFollowing = true;
        }

        void GetCube(Vector3 destination)
        {
            cubeComponent = FindObjectOfType<HoleFalling>();
            cube = cubeComponent.gameObject;
            startingPos = cube.transform.position;
        }



    }





}

