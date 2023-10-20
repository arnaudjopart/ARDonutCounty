using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throw : MonoBehaviour
{

    public void ThrowObject(Vector3 _direction)
    {
        gameObject.GetComponent<Rigidbody>().AddForce(_direction, ForceMode.Impulse);
    }

}
