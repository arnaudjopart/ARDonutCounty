using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


namespace Christophe.Fanchamps
{
    public class HoleFalling : MonoBehaviour
    {
        Rigidbody rb;

        private void OnTriggerEnter(Collider collision)
        {
                Debug.Log("*****HOLE FALLING");
            if (collision.gameObject.CompareTag("Player"))
            {
                int LayerHoleContent = LayerMask.NameToLayer("HoleContent");
                gameObject.layer = LayerHoleContent;
                GetComponent<BallScore>().ScoretoAdd();

            }

        }
    }
}
