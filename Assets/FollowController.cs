using Christophe.Fanchamps;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Christophe.fanchamps
{
    public class FollowController : MonoBehaviour
    {
        HoleFalling cubeComponent;
        GameObject cube;

        private void Start()
        {
            cubeComponent = FindObjectOfType<HoleFalling>();
            cube = cubeComponent.gameObject;
        }

        private void Update()
        {
            Vector3 position = new Vector3(cube.transform.position.x, 0, cube.transform.position.z);
                transform.position = position;
        }
    }


}

