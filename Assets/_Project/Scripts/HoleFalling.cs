using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Christophe.Fanchamps
{
    public class HoleFalling : MonoBehaviour
    {
        Rigidbody rb;

        private void OnTriggerEnter(Collider collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                //GetComponent<Rigidbody>().useGravity = true;
                int LayerHoleContent = LayerMask.NameToLayer("HoleContent");
                gameObject.layer = LayerHoleContent;
            }

        }
    }
}
