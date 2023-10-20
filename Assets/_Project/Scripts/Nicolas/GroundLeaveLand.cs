using UnityEngine;

public class GroundLeaveLand : MonoBehaviour
{
    public static event System.Action OnLeaveGround;
    public static event System.Action<float, float> OnLandGround;

    public float GroundDetectionDistance = 0.1f;

    //private Vector3 lastPosition;
    private Vector3 startLeaveGroundPosition;
    private float startTime;
    private float distanceTraveled;

    private bool isGrounded = true;

    private void Start()
    {
        //lastPosition = transform.position;
    }

    private void Update()
    {
        if (isGrounded)
        {
            if (!IsGroundedDistance())
            {
                isGrounded = false;
                startTime = Time.time;
                startLeaveGroundPosition = transform.position;
Debug.Log("OnLeaveGround");
                OnLeaveGround?.Invoke();
            }
        }
        else
        {
            if (IsGroundedDistance())
            {
                isGrounded = true;
                float flightTime = Time.time;
                flightTime = flightTime - startTime;
                distanceTraveled = Vector3.Distance(startLeaveGroundPosition, transform.position);
                startLeaveGroundPosition = Vector3.zero;
                startTime = 0;
Debug.Log("Ball grounded and traveled " + distanceTraveled + " units in the air, and spent " + flightTime + " seconds in the air.");
                OnLandGround?.Invoke(distanceTraveled, flightTime);
            }
        }

        //lastPosition = transform.position;
    }

    private bool IsGroundedDistance()
    {
        return transform.position.y < GroundDetectionDistance;
    }
}
