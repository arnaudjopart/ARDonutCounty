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
        other.gameObject.layer = LayerMask.NameToLayer("HoleContent");
        Debug.Log("OnTriggerEnter - " + other.name) ;
    }

    private void OnTriggerExit(Collider other)
    {
        other.gameObject.layer = LayerMask.NameToLayer("DefaultPhysic");
        Debug.Log("OnTriggerExit - " + other.name);
    }

}
