using UnityEngine;

namespace NJ
{
    public class BallEnterHole : MonoBehaviour
    {
        public static event System.Action<bool> OnBallEnterHole;
        public static event System.Action OnBallExitHole;

        public float m_minFlyingDistance = 0.4f;
        private bool ballWasFlying;

        private void OnEnable()
        {
            ballWasFlying = false;
            GroundLeaveLand.OnLeaveGround += BallStartFlying;
            GroundLeaveLand.OnLandGround += CheckBallWasFlying;
        }

        private void BallStartFlying()
        {
            ballWasFlying = true;
        }

        private void OnDisable()
        {
            GroundLeaveLand.OnLeaveGround -= BallStartFlying;
            GroundLeaveLand.OnLandGround -= CheckBallWasFlying;
        }
        private void Update()
        {
            //if (ballWasFlying) { ballWasFlying = false; }
        }

        private void CheckBallWasFlying(float distanceTraveled = 0, float flightTime = 0)
        {
            if (distanceTraveled > m_minFlyingDistance)
                ballWasFlying = true;
            else ballWasFlying = false;
Debug.Log("CheckBallWasFlying:" + ballWasFlying);
        }

        private void OnTriggerEnter(Collider other)
        {
Debug.Log("In:" + other.name);
            if (other.CompareTag("Player"))
            {
                // Emit the event and provide the "wasFlying" flag
                OnBallEnterHole?.Invoke(ballWasFlying);
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