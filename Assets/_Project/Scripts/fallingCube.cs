using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class fallingCube : MonoBehaviour
{
    int m_nbDestroy;
    private BoxCollider boxCollider;
    private float volume;
    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        Vector3 geometry = boxCollider.bounds.size;
        volume = geometry.x * geometry.y * geometry.z;
    }
    private void OnTriggerEnter(Collider other)
    {
        Vector3 geometry = other.bounds.size;
        float otherVolume = geometry.x * geometry.y * geometry.z;
        if (volume < otherVolume)
        {
            
        }
        else if (volume > otherVolume)
        {
            other.gameObject.GetComponent<BoxCollider>().isTrigger = true;
            other.gameObject.layer = LayerMask.NameToLayer("HoleContent");
            m_nbDestroy++;
        }
            
        }

    // Update is called once per frame
    void Update()
    {
        if (m_nbDestroy == 10)
        {
            gameObject.transform.localScale *= 2f;
            m_nbDestroy = 0;
        }
    }
}
