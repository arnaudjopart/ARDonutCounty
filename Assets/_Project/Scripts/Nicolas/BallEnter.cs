using UnityEngine;

namespace NJ
{
    public class BallEnter : MonoBehaviour
    {
        public static event System.Action OnBallEnterHole;
        public static event System.Action OnBallExitHole;

        private void OnTriggerEnter(Collider other)
        {
Debug.Log("In:" + other.name);
            if (other.CompareTag("Player"))
            {
                OnBallEnterHole?.Invoke();
            }
        }

        private void OnTriggerExit(Collider other)
        {
Debug.Log("Out:" + other.name);
            if (other.CompareTag("Player"))
            {
                OnBallExitHole?.Invoke();
            }
        }
    }
}