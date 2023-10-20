using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroscopeController : MonoBehaviour
{
    [SerializeField]
    float speed;
    Vector3 direction = new Vector3(0,0,0);


    void Start()
    {
        Input.gyro.enabled = true;
    }
    // Update is called once per frame
    void Update()
    {
        direction.x = Input.acceleration.x;
        direction.y = Input.acceleration.y;
        Debug.Log("X: " + direction.x);
        Debug.Log("Y: " + direction.y);
        gameObject.transform.Translate(new Vector3(-speed*direction.x,0, -speed*direction.y));
    }
}
