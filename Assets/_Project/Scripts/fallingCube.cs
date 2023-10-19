using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class fallingCube : MonoBehaviour
{
    int m_nbDestroy;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        var Hole = gameObject.transform.localScale.magnitude;
        var Cube = other.transform.localScale.magnitude;
        if (Hole > Cube)
        {
            other.gameObject.layer = LayerMask.NameToLayer("HoleContent");
            Debug.Log("OnTriggerEnter - "+other.name);
            m_nbDestroy++;
            Debug.Log(m_nbDestroy);
        }
    }

    /*private void OnTriggerExit(Collider other)
    {
        other.gameObject.layer = LayerMask.NameToLayer("Default");
    }*/



    // Update is called once per frame
    void Update()
    {

        if (m_nbDestroy >= 10)
        {
            Debug.Log("jesuisdanslacondition");
            gameObject.transform.localScale *= 1.5f;
            m_nbDestroy = 0;
            
        }
    }
}
