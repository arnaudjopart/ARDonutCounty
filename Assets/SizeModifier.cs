using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeModifier : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Grow()
    {
        transform.localScale *= 1.2f;
    }
}
