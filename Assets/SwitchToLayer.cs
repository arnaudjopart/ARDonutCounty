using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchToLayer : MonoBehaviour
{
    [SerializeField] private LayerMask from;
    [SerializeField] private LayerMask to;


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("SwitchToLayer - OnTriggerEnter - " + other.name);
        other.gameObject.layer = LayerMask.NameToLayer("HoleContent");
    }
}
