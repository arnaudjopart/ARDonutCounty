using UnityEngine;

public class PhysicLayerSwitch : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.layer = LayerMask.NameToLayer("HoleContentPhysic");
        //Debug.Log("OnTriggerEnter - " + other.name) ;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("HoleContentPhysic"))
        {
            other.gameObject.layer = LayerMask.NameToLayer("DefaultPhysic");
            //Debug.Log("OnTriggerExit - " + other.name);
        }
    }

}
