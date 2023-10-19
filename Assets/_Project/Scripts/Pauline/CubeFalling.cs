using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeFalling : MonoBehaviour
{
   
    void Update()
    {
        if (transform.position.y <= -0.5f)
        {
            Destroy(gameObject);
        }
    }
}
