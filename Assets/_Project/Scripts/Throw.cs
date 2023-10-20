using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throw : MonoBehaviour
{

    Rigidbody rb;
    [SerializeField]
    bool applyTorque;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void ThrowObject(Vector3 _direction)
    {
        rb.AddForce(_direction, ForceMode.Impulse);
        if(applyTorque )
        {
            rb.AddTorque(Vector3.back);
        }
    }

}
